using System.Xml;

namespace CommonUser.Transmission
{
    class XMLPhaser
    {
        public static XmlDocument StringToXml(string xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            return xmlDoc;
        }
        public static string XmlToString(XmlDocument xmlDoc)
        {
            return xmlDoc.InnerText;
        }
    }
}
