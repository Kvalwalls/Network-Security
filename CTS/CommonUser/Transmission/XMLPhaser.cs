using System;
using System.Xml;

namespace CommonUser.Transmission
{
    class XMLPhaser
    {
        /// <summary>
        /// xml字符串转换xmlDocument方法
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns>xmlDocument</returns>
        public static XmlDocument StringToXml(string xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            return xmlDoc;
        }

        /// <summary>
        /// xmlDocument转换xmlDocument方法
        /// </summary>
        /// <param name="xmlDoc">xmlDocument</param>
        /// <returns>xml字符串</returns>
        public static string XmlToString(XmlDocument xmlDoc)
        {
            return xmlDoc.InnerXml;
        }
    }
}
