using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit;
using NUnit.Framework;
using XmlReader.FileWatcher.XmlFileHandling;

namespace XmlReader.FileWatcher.Tests
{
    [TestFixture]
    public class XmlContentReaderTests
    {
        string folderLocation;
        string validXmlFile;
        [SetUp]
        public void Setup()
        {
            folderLocation = Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net5.0", @"\TestProviders\");
            validXmlFile = $"{ folderLocation }{ "validtestfile.xml"}";
        }

        [Test]
        public void GetXmlFilePath_LoadXml_ShouldReturnValidXmlObj()
        {
            var xmlContentReader = new Mock<IXmlContentReader>();
            
            xmlContentReader.Setup(xm => xm.GetXmlFilePath()).Returns(validXmlFile);

            var result = xmlContentReader.Object.GetXmlFilePath() ;
        }
    }
}
