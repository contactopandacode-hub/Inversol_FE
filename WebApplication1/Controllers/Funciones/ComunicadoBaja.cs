using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using COBE;
using CODAT;
using COBEC;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ServicioRSNetCore.Controllers.Funciones
{
    public class ComunicadoBaja:DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;

        public ComunicadoBaja(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        //public COBEc_Error Registrar(RegistrarComprobanteRequest request)
        //{
        //    string str_resultado = string.Empty;
        //    COBEc_Error retorno = new COBEc_Error();
        //    HttpWebRequest obj_Request = default(HttpWebRequest);
        //    StringBuilder stb_Resultado = new StringBuilder();
        //    StreamWriter sw_Documento = null;
        //    byte[] obj_FileByte = null;
        //    XmlDocument obj_XML = new XmlDocument();
        //    XmlNodeList obj_Respuesta = null;
        //    long ll_inicio = 1;
        //    CODAT_ComunicadoBaja obj_datos = new CODAT_ComunicadoBaja(configuration, this.context);
        //    try
        //    {
        //        retorno.codigo = "00";
        //        retorno.mensaje = "Procesado correctamente";

        //        cls_ComunicadoBaja baja = obj_datos.Datos(request);

        //        //stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://ws.ce.ebiz.com/\">");
        //        stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://pse.bizlinks.com/\">");
        //        stb_Resultado.AppendFormat("<soapenv:Header/>");
        //        stb_Resultado.AppendFormat("<soapenv:Body>");
        //        stb_Resultado.AppendFormat("<ws:invoke>");
        //        stb_Resultado.AppendFormat("<command><![CDATA[<SignOnLineSummaryCmd declare-sunat=\"1\" replicate=\"1\" output=\"\">");
        //        stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"idEmisor\"/>", baja.rucEmisor.Trim());
        //        stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"tipoDocumento\"/>", "RA");
        //        stb_Resultado.Append("<documento>");
        //        stb_Resultado.AppendFormat("<numeroDocumentoEmisor>{0}</numeroDocumentoEmisor>", baja.rucEmisor.Trim());
        //        stb_Resultado.AppendFormat("<version>{0}</version>", "1.0");
        //        stb_Resultado.AppendFormat("<versionUBL>{0}</versionUBL>", "2.0");
        //        stb_Resultado.AppendFormat("<tipoDocumentoEmisor>{0}</tipoDocumentoEmisor>", "6");
                
        //        string str_resumenId ="RA-" + DateTime.Now.ToString("yyyyMMdd") +"-" + baja.resumenId.ToString();

        //        stb_Resultado.AppendFormat("<resumenId>{0}</resumenId>", str_resumenId.Trim());
        //        stb_Resultado.AppendFormat("<fechaEmisionComprobante>{0}</fechaEmisionComprobante>", baja.fechaEmision.ToString("yyyy-MM-dd"));
        //        stb_Resultado.AppendFormat("<fechaGeneracionResumen>{0}</fechaGeneracionResumen>", DateTime.Now.ToString("yyyy-MM-dd"));
        //        stb_Resultado.AppendFormat("<razonSocialEmisor>{0}</razonSocialEmisor>", baja.razonSocial.Trim());
        //        stb_Resultado.AppendFormat("<correoEmisor>{0}</correoEmisor>", "-");
        //        stb_Resultado.AppendFormat("<inHabilitado>{0}</inHabilitado>", "1");
        //        stb_Resultado.AppendFormat("<resumenTipo>{0}</resumenTipo>", "RA");
        //        stb_Resultado.AppendFormat("<ResumenItem>");
        //        stb_Resultado.AppendFormat("<numeroFila>{0}</numeroFila>", "1");
        //        stb_Resultado.AppendFormat("<tipoDocumento>{0}</tipoDocumento>", baja.tipoDocumento.Trim());

        //        string numero = request.numeroDocumento;

        //        // Separar serie y correlativo
        //        string[] partes = numero.Split('-');
        //        string serie = partes[0];
        //        string correlativo = partes[1];

        //        // Completar correlativo a 8 dígitos
        //        string correlativoFormateado = correlativo.PadLeft(8, '0');

        //        stb_Resultado.AppendFormat("<serieDocumentoBaja>{0}</serieDocumentoBaja>", serie.Trim());
        //        stb_Resultado.AppendFormat("<numeroDocumentoBaja>{0}</numeroDocumentoBaja>", correlativoFormateado.Trim());
        //        stb_Resultado.AppendFormat("<motivoBaja>{0}</motivoBaja>", baja.motivoBaja.Trim());
        //        stb_Resultado.AppendFormat("</ResumenItem>");
        //        stb_Resultado.AppendFormat("</documento>");
        //        stb_Resultado.AppendFormat("</SignOnLineSummaryCmd>]]>");
        //        stb_Resultado.AppendFormat("</command>");
        //        stb_Resultado.AppendFormat("</ws:invoke>");
        //        stb_Resultado.AppendFormat("</soapenv:Body>");
        //        stb_Resultado.AppendFormat("</soapenv:Envelope>");

        //        if (baja.rutaXml.Trim() != string.Empty)
        //        {
        //            sw_Documento = new StreamWriter(baja.rutaXml + baja.rucEmisor.Trim() + "-RA-" + baja.tipoDocumento.Trim() + "-" + request.numeroDocumento+ ".xml");
        //            sw_Documento.Write(stb_Resultado.ToString());
        //            sw_Documento.Flush();
        //            sw_Documento.Close();
        //        }

        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        CookieContainer myContainer = new CookieContainer();
        //        obj_Request = (HttpWebRequest)HttpWebRequest.Create(baja.urlWebService.Trim());
        //        obj_Request.Method = "POST";
        //        obj_Request.ContentType = "text/xml;charset=UTF-8";
        //        //obj_Request.Credentials = new NetworkCredential(baja.URLUsuario.Trim(), baja.URLPassword.Trim());

        //        // --- SOLO ESTO PARA AUTENTICACIÓN (IGUAL QUE POSTMAN) ---
        //        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(baja.URLUsuario.Trim() + ":" + baja.URLPassword.Trim()));
        //        obj_Request.Headers["Authorization"] = "Basic " + credentials;
        //        //--- FIN.

        //        obj_Request.CookieContainer = myContainer;
        //        obj_FileByte = System.Text.Encoding.UTF8.GetBytes(stb_Resultado.ToString());
        //        obj_Request.ContentLength = obj_FileByte.Length;

        //        using (Stream obj_RequestStream = obj_Request.GetRequestStream())
        //        {
        //            obj_RequestStream.Write(obj_FileByte, 0, obj_FileByte.Length);
        //        }

        //        //Invocar al Servicio REST
        //        using (HttpWebResponse obj_Response = (HttpWebResponse)obj_Request.GetResponse())
        //        {
        //            using (Stream obj_ResponseStream = obj_Response.GetResponseStream())
        //            {
        //                using (StreamReader obj_Reader = new StreamReader(obj_ResponseStream))
        //                {
        //                    str_resultado = obj_Reader.ReadToEnd();
        //                }
        //            }
        //        }

        //        //Cargado la Respuesta
        //        string str_status = string.Empty;
        //        string str_Documents = string.Empty;

        //        obj_XML.LoadXml(str_resultado.Replace("???", String.Empty));
        //        obj_Respuesta = obj_XML.GetElementsByTagName("return");

        //        foreach (XmlElement xmlEle_Elemento in obj_Respuesta)
        //        {
        //            str_resultado = xmlEle_Elemento.InnerText;
        //            obj_XML.LoadXml(str_resultado.Replace("???", String.Empty));

        //            //Mensaje de Registro
        //            XmlNodeList obj_document = obj_XML.GetElementsByTagName("document");
        //            foreach (XmlElement xmlEle_Respuesta in obj_document)
        //            {
        //                if (obj_document.Item(0).OuterXml.Contains("status"))
        //                    str_status = xmlEle_Respuesta.GetElementsByTagName("status")[0].InnerText;

        //                if (str_status == "ERROR")
        //                    str_Documents = "FE_00|";
        //                else
        //                {
        //                    if (obj_document.Item(0).OuterXml.Contains("statusSunat"))
        //                        str_Documents = xmlEle_Respuesta.GetElementsByTagName("statusSunat")[0].InnerText + "|";
        //                    else
        //                        str_Documents += "-|";

        //                    if (obj_document.Item(0).OuterXml.Contains("hashCode"))
        //                        str_Documents += xmlEle_Respuesta.GetElementsByTagName("hashCode")[0].InnerText + "|";
        //                    else
        //                        str_Documents += "-|";

        //                    if (obj_document.Item(0).OuterXml.Contains("xmlFileSunatUrl"))
        //                    {
        //                        str_Documents += xmlEle_Respuesta.GetElementsByTagName("xmlFileSunatUrl")[0].InnerText + "|";
        //                    }
        //                    else
        //                        str_Documents += "-|";
        //                }

        //                string[] lecturaXML = str_Documents.Split('|');
        //                if (lecturaXML[0]== "PE_02")
        //                    retorno = obj_datos.Update(request, "1", "EN", lecturaXML[1], string.Empty, str_resumenId);


        //                //Mensaje de respuesta
        //                XmlNodeList obj_return = obj_XML.GetElementsByTagName("messages");
        //                if (obj_return.Count > 0)
        //                {
        //                    foreach (XmlElement xmlEle_message in obj_return)
        //                    {
        //                        if (ll_inicio == 1)
        //                            str_resultado = str_Documents;
        //                        else
        //                            str_resultado += str_Documents;

        //                        str_resultado += xmlEle_message.GetElementsByTagName("codeDetail")[0].InnerText + "|";
        //                        str_resultado += xmlEle_message.GetElementsByTagName("descriptionDetail")[0].InnerText + "||";
        //                        ll_inicio++;
        //                    }

        //                    retorno = obj_datos.Update(request, "1", "EX", lecturaXML[1], string.Empty, str_resumenId);
        //                }
        //                else
        //                    str_resultado += str_Documents + "-|-|-|-||";



        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        retorno.codigo = "01";
        //        retorno.mensaje = e.Message;    
        //    }

        //    return retorno;

        //}
    }
}
