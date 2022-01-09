using System;
using System.Collections.Generic;
using XmlReader.FileWatcher.Models;

namespace XmlReader.FileWatcher.XmlFileHandling
{
   public interface IFileContentHandler
    {
        List<CsvFileData> ProcessCsvIntervalBlockData(aseXML aseXmlToObj);
        bool ValidateCsvInterValBlockData(List<string> csvMeterRecords);
        List<CsvFileData> ValidateAndConvertTOCsvObj(List<string> meterReadingData);
        void SaveToCsvFile(CsvFileData csvFileData);
        List<CsvFileData> ConvertXmlToCsv();
    }
}
