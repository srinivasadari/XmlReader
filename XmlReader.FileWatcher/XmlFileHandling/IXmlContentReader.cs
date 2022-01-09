
namespace XmlReader.FileWatcher.XmlFileHandling
{
    public interface IXmlContentReader
    {
        bool ValidateXmlFile(aseXML aseXmlObj);
        aseXML GetParsedXmlToObj();
        string GetXmlFilePath();
        string TestFilesFolderLocation();
    }
}