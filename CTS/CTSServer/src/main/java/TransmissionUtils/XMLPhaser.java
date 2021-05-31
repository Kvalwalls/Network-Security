package TransmissionUtils;

import org.w3c.dom.Document;
import org.xml.sax.InputSource;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import java.io.ByteArrayOutputStream;
import java.io.StringReader;

public class XMLPhaser {
    /**
     * xml字符串转换xmlDocument方法
     *
     * @param xmlStr xml字符串
     * @return xmlDocument
     * @throws Exception 转换异常
     */
    public static Document StringToDoc(String xmlStr) throws Exception {
        InputSource is = new InputSource(new StringReader(xmlStr));
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        return builder.parse(is);
    }

    /**
     * xmlDocument转换xmlDocument方法
     *
     * @param xmlDoc xmlDocument
     * @return xml字符串
     * @throws Exception 转换异常
     */
    public static String DocToString(Document xmlDoc) throws Exception {
        TransformerFactory factory = TransformerFactory.newInstance();
        Transformer transformer = factory.newTransformer();
        transformer.setOutputProperty(OutputKeys.OMIT_XML_DECLARATION, "yes");
        ByteArrayOutputStream bos = new ByteArrayOutputStream();
        transformer.transform(new DOMSource(xmlDoc), new StreamResult(bos));
        return bos.toString();
    }
}