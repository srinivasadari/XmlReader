using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XmlReader.FileWatcher.Constants;
using XmlReader.FileWatcher.Extensions;
using XmlReader.FileWatcher.Models;

namespace XmlReader.FileWatcher.XmlFileHandling
{
    public class FileContentHandler : IFileContentHandler
    {
        private readonly IXmlContentReader _xmlContentReader;

        public FileContentHandler(IXmlContentReader xmlContentReader)
        {
            _xmlContentReader = xmlContentReader;
        }

        public List<CsvFileData> ProcessCsvIntervalBlockData(aseXML aseXmlToObj)
        {
            var meterReadingData = aseXmlToObj.Transactions.Transaction.MeterDataNotification.CSVIntervalData
                                                   .Split("\n",StringSplitOptions.TrimEntries).ToList().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            aseXmlToObj.CsvIntervalBlockData = meterReadingData;

            var csvFilesData = ValidateAndConvertTOCsvObj(meterReadingData);

            return csvFilesData;
        }

        public bool ValidateCsvInterValBlockData(List<string> csvMeterRecords)
        {
            bool isValidCsvData = false;

            var csvRecords = csvMeterRecords.Where(x => !string.IsNullOrWhiteSpace(x)).ToList()
                                            .Select(x => x.GetCommaSperatedAndConvertToInt())
                                            .Select(x => x.FirstOrDefault()).ToList();

            var definedRowNumbers = ((DefinedRowNumbers[])Enum.GetValues(typeof(DefinedRowNumbers))).Select(x => (int)x).ToList();

            // Point-1 and Point-2
            // Valid rows within the CSVIntervalData element can only start with "100", "200", "300","900"
            // The CSVIntervalData element should contain at least 1 row for each of "100", "200", "300", "900"
            // If there is a new numbers exists in the records then following condition returns true

            // var isAllRowsShouldStartsWithAsExpected = csvRecords.Except(definedRowNumbers).Any();
            var isAllRowsShouldStartsWithAsExpected = csvRecords.All(x => !x.Equals((int)DefinedRowNumbers.Hundred)
                                                       || !x.Equals((int)DefinedRowNumbers.TwoHundred)
                                                       || !x.Equals((int)DefinedRowNumbers.ThreeHundred)
                                                       || !x.Equals((int)DefinedRowNumbers.NineHundred));


            Console.WriteLine($"Is CSVIntervalData element should contain at least 1 row for each of 100, 200, 300, 900 : { isAllRowsShouldStartsWithAsExpected}");

            // Point - 3
            // "100", "900" rows should only appear once inside the CSVIntervalData element
            // If either one record not present then the following condition not meet the expected the count = 2 
            var hundredAndNineHundredShouldPresentOnce = csvRecords.Where(x => x.Equals((int)DefinedRowNumbers.Hundred)
                                                                               || x.Equals((int)DefinedRowNumbers.NineHundred))
                                                                               .Distinct().Count() == 2;

            Console.WriteLine($"Is CSVIntervalData 100 and 900 presented once : { hundredAndNineHundredShouldPresentOnce}");

            // Point - 4
            // "200" and "300" can repeat and will be within the header and trailer rows 
            // Each CSV file should have the "100" row as a header, and the "900" row as the trailer
            var headerPresentedWithHundred = csvRecords.First().Equals((int)DefinedRowNumbers.Hundred);

            Console.WriteLine($"Is CSVIntervalData header with 100: { headerPresentedWithHundred}");

            var footerPresentedWithNineHundred = csvRecords.Last().Equals((int)DefinedRowNumbers.NineHundred);

            Console.WriteLine($"Is CSVIntervalData footer with 900: { footerPresentedWithNineHundred}");

            // Point -5
            // "200" row must be followed by at least 1 "300" row
            bool isThreeHundredRowExists = false;
            for (var i = 0; i < csvRecords.Count; i++)
            {
                if (i != (csvRecords.Count - 1) && csvRecords[i].Equals((int)DefinedRowNumbers.TwoHundred))
                {
                    isThreeHundredRowExists = csvRecords[i + 1].Equals((int)DefinedRowNumbers.ThreeHundred);
                }
            }

            Console.WriteLine($"Is CSVIntervalData 200 row must be followed by at least 1 300 row: { isThreeHundredRowExists}");

            if (isAllRowsShouldStartsWithAsExpected && hundredAndNineHundredShouldPresentOnce
                && headerPresentedWithHundred && footerPresentedWithNineHundred
                && isThreeHundredRowExists)
            {
                isValidCsvData = true;
            }

            return isValidCsvData;
        }

        public void SaveToCsvFile(CsvFileData csvFileData)
        {
            var blockData = csvFileData.BlockMeterReading;

            List<List<string>> fileData = blockData.Select(item => item.GetCommaSperatedValues()).ToList();

            var filePath = $"{_xmlContentReader.TestFilesFolderLocation()}{csvFileData.FileName}.csv";

            string strSeperator = ",";

            var rowItem = new StringBuilder();

            rowItem.AppendLine(string.Join(strSeperator, csvFileData.Header));

            foreach (var item in fileData)
            {
                rowItem.AppendLine(string.Join(strSeperator, item));
            }

            rowItem.AppendLine(string.Join(strSeperator, csvFileData.Footer));

            Console.WriteLine($"The File Name :{csvFileData.FileName} saved into the {filePath}");

            File.WriteAllText(filePath, rowItem.ToString());
        }

        public List<CsvFileData> ValidateAndConvertTOCsvObj(List<string> meterReadingData)
        {
            var csvFilesData = new List<CsvFileData>();

            var isCsvBlockMetTheValidCriteria = ValidateCsvInterValBlockData(meterReadingData);

            Console.WriteLine(
                isCsvBlockMetTheValidCriteria
                    ? $"The CSVInterval validation criteria is all met and processing to save to CSV files"
                    : $"The CSVInterval validation criteria is not met so not processing to CSV files");

            if (isCsvBlockMetTheValidCriteria)
            {
                var headerNodeName = meterReadingData.FirstOrDefault(x => x.StartsWith(EnergyXmlConstants.StartsWithHundred));
                var footerNodeName = meterReadingData.FirstOrDefault(x => x.StartsWith(EnergyXmlConstants.StartsWithNineHundred));

                var csvIntervalData = meterReadingData.Select((v, i) => new { Index = i, Value = v })
                                             .Where(x => x.Value.StartsWith(EnergyXmlConstants.StartsWithTwoHundred)
                                             || x.Value.StartsWith(EnergyXmlConstants.StartsWithNineHundred))
                                             .Select(x => x.Index).ToList();

                for (int i = 1; i < csvIntervalData.Count; i++)
                {
                    var secondIndex = csvIntervalData[i];
                    var firstIndex = csvIntervalData[i - 1];
                    var blockData = meterReadingData.Skip(firstIndex).Take(secondIndex - firstIndex).ToList();
                    var fileName = blockData[0].GetCommaSperatedValues()[1];
                    var csvFileData = new CsvFileData
                    {
                        Header = headerNodeName,
                        Footer = footerNodeName,
                        BlockMeterReading = blockData,
                        FileName = fileName
                    };
                    csvFilesData.Add(csvFileData);
                }
            }
           
            return csvFilesData;
        }

        public List<CsvFileData> ConvertXmlToCsv()
        {
            List<CsvFileData> csvFilesData;
            try
            {
                Console.WriteLine($"*********The File convertion process started*******");

                var aseXmlToObj = _xmlContentReader.GetParsedXmlToObj();

                var isValidXmlContent = _xmlContentReader.ValidateXmlFile(aseXmlToObj);

                csvFilesData = ProcessCsvIntervalBlockData(aseXmlToObj);

                if(csvFilesData.Any())
                {
                    foreach (var item in csvFilesData)
                    {
                        SaveToCsvFile(item);
                    }

                    Console.WriteLine($"*********The File convertion process completed successfully*******");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Unable to proces the xml file due to following excepiton : {exception.Message}");

                throw;
            }
            return csvFilesData;
        }
    }
}
