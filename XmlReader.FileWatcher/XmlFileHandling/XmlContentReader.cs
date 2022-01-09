using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XmlReader.FileWatcher.XmlFileHandling
{
    public class XmlContentReader : IXmlContentReader
    {
        private readonly  IConfiguration _configuration;
        
        public XmlContentReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool ValidateXmlFile(aseXML aseXmlToObj)
        {
            var isValidXml = aseXmlToObj.Header != null && aseXmlToObj.Transactions?.Transaction?.transactionDate != default(DateTime).ToUniversalTime()
                                                           && aseXmlToObj.Transactions?.Transaction?.transactionID != null
                                                           && aseXmlToObj.Transactions?.Transaction?.MeterDataNotification?.CSVIntervalData != null
                                                           ? true : false;
            return isValidXml;
        }

        public aseXML GetParsedXmlToObj()
        {
            var aseXmlToObj = new aseXML();

            try
            {
                var filePath = GetXmlFilePath();

                XDocument xDocument = XDocument.Load(filePath);

                xDocument.ToString();

                var xmlSerializer = new XmlSerializer(typeof(aseXML));

                using (var streamReader = new StreamReader(filePath))
                {
                    aseXmlToObj = (aseXML)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return aseXmlToObj;
        }

        public string TestFilesFolderLocation()
        {
            var isReadingFromUserDefinedFolderLocation = Convert.ToBoolean(_configuration.GetSection("FileWatcher")["ReadFromLocalFolder"]);
            var filePath = isReadingFromUserDefinedFolderLocation ? _configuration.GetSection("FileWatcher")["XmlFilePath"]
                                                : Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net5.0", @"\XmlFilies\");
            return filePath;
        }

        /// <summary>
        /// If FilewatchSystem mode is enabled then it will read file from the defined folder location else it will read from the 
        /// current project "XMLFiles" folder location which contains test.xml file for this test purpose
        /// </summary>
        /// <returns></returns>
        public string GetXmlFilePath()
        {
            var filePath = Convert.ToBoolean(_configuration.GetSection("FileWatcher")["ReadFromLocalFolder"]) ? TestFilesFolderLocation() : TestFilesFolderLocation() + "testfile.xml";
            //var isFileWatcherEnabled = Convert.ToBoolean(_configuration.GetSection("FileWatcher")["ReadFromLocalFolder"]);
            //var filePath = isFileWatcherEnabled ? _configuration.GetSection("FileWatcher")["XmlFilePath"]
            //                                    : Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net5.0", @"\XmlFilies\testfile.xml");

            return filePath;
        }
    }
}