package TransmissionUtils;

import org.w3c.dom.Document;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;

public class XMLBuilder {
    /**
     * 创建xmlDocument方法
     * @return xmlDocument
     * @throws Exception 创建异常
     */
    public static Document buildXMLDoc() throws Exception {
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        Document document = builder.newDocument();
        document.setXmlStandalone(true);
        return document;
    }
}
