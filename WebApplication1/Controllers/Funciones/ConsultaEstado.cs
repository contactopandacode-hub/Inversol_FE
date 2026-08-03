using COBE;
using COBEC;
using CODAT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace ServicioRSNetCore.Controllers.Funciones
{
    public class ConsultaEstado:DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;

        public ConsultaEstado(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        //public COBEc_Error Obtener(request request)
        //{
        //    COBEc_Error resultado = new COBEc_Error();
        //    HttpWebRequest obj_Request = default(HttpWebRequest);
        //    StringBuilder stb_Resultado = new StringBuilder();          
        //    byte[] obj_FileByte = null;
        //    XmlDocument obj_XML = new XmlDocument();
        //    string str_resultado = string.Empty;

        //    try
        //    {
        //        CODAT_ComprobanteRegistrar obj_datos = new CODAT_ComprobanteRegistrar(configuration, this.context);
        //        DatosCompania compania = obj_datos.DatosCompania(request);

        //        //stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://ws.ce.ebiz.com/\">");
        //        stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://pse.bizlinks.com/\">");
        //        stb_Resultado.AppendFormat("<soapenv:Header/>");
        //        stb_Resultado.AppendFormat("<soapenv:Body>");
        //        stb_Resultado.AppendFormat("<ws:invoke>");
        //        stb_Resultado.AppendFormat("<command><![CDATA[<ConsultCmd output=\"PDF\">");
        //        stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"idEmisor\"/>", compania.DocumentoFiscal.Trim());
        //        stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"tipoDocumento\"/>", request.tipoDocumento.Trim());

        //        //string numero = request.numeroDocumento;
        //        // Separar serie y correlativo
        //        string[] partes = // numero.Split('-');
        //        string serie = partes[0];
        //        string correlativo = partes[1];

        //        // Completar correlativo a 8 dígitos
        //        string correlativoFormateado = correlativo.PadLeft(8, '0');

        //        stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"serieGrupoDocumento\"/>", serie.Trim());
        //        stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"numeroCorrelativoInicio\"/>", correlativoFormateado.Trim());
        //        stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"numeroCorrelativoFin\"/>", correlativoFormateado.Trim());
        //        stb_Resultado.Append("</ConsultCmd>]]></command>");
        //        stb_Resultado.Append("</ws:invoke>");
        //        stb_Resultado.Append("</soapenv:Body>");
        //        stb_Resultado.Append("</soapenv:Envelope>");

        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        CookieContainer myContainer = new CookieContainer();
        //        obj_Request = (HttpWebRequest)HttpWebRequest.Create(compania.urlWebService.Trim());
        //        obj_Request.Method = "POST";
        //        obj_Request.ContentType = "text/xml;charset=UTF-8";
        //        //obj_Request.Credentials = new NetworkCredential(compania.URLUsuario.Trim(), compania.URLPassword.Trim());

        //        // --- SOLO ESTO PARA AUTENTICACIÓN (IGUAL QUE POSTMAN) ---
        //        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(compania.URLUsuario.Trim() + ":" + compania.URLPassword.Trim()));
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

        //        str_resultado = XMLLectura(str_resultado, "1", string.Empty);
        //        string[] str_ListaResultado = str_resultado.Split('|');
        //        if (str_ListaResultado[0] == "AC_03")
        //            resultado.codigo = "AP";
        //        else if (str_ListaResultado[0] == "RC_05")
        //            resultado.codigo = "RE";
        //        else if (str_ListaResultado[0] == "AN_04")
        //            resultado.codigo = "AN";
        //        else
        //            resultado.codigo = "EN";

        //        resultado.mensaje = str_ListaResultado[1];

        //    }
        //    catch (Exception ex)
        //    {


        //        str_resultado = ex.ToString();
        //    }

        //    return resultado;
        //}

        private static string XMLLectura(string str_xml, string str_tipoEstado, string str_ruta)
        {
            XmlDocument obj_XML = new XmlDocument();
            XmlNodeList obj_Respuesta = null;
            string str_resultado = string.Empty;

            try
            {
                obj_XML.LoadXml(str_xml.Replace("???", String.Empty));
                obj_Respuesta = obj_XML.GetElementsByTagName("return");

                foreach (XmlElement xmlEle_Elemento in obj_Respuesta)
                {
                    str_resultado = xmlEle_Elemento.InnerText;
                    obj_XML.LoadXml(str_resultado.Replace("???", String.Empty));
                    XDocument xdoc = XDocument.Parse(str_resultado);

                    if (str_tipoEstado == "2" || str_tipoEstado == "3")
                    {

                        if (str_tipoEstado == "2")
                        {
                            var pdfFileUrl = xdoc.Root
                           .Element("genericInvokeResponse")
                           .Element("xmlResult")
                           .Element("document")
                           .Element("pdfFileUrl")
                           ?.Value;
                            str_resultado = pdfFileUrl;
                        }
                        else
                        {
                            var pdfFileUrl = xdoc.Root
                            .Element("genericInvokeResponse")
                            .Element("xmlResult")
                            .Element("document")
                            .Element("xmlFileSignUrl")
                            ?.Value;
                            str_resultado = pdfFileUrl;
                        }

                        Uri obj_Uri = null;
                        WebClient obj_Cliente = new WebClient();
                        byte[] obj_ArchivoPDF = null;
                        FileStream fs_ArchivoPDF = null;
                        obj_Uri = new Uri(str_resultado);

                        obj_ArchivoPDF = obj_Cliente.DownloadData(obj_Uri);

                        if (obj_ArchivoPDF == null)
                        {
                            str_resultado = "1|No se logro descargar el PDF";
                        }
                        else
                        {
                            fs_ArchivoPDF = new FileStream(str_ruta, FileMode.Create, FileAccess.Write);
                            foreach (byte b in obj_ArchivoPDF)
                                fs_ArchivoPDF.WriteByte(b);

                            fs_ArchivoPDF.Close();
                            str_resultado = "0";
                        }
                    }
                    else
                    {
                        var statusSunat = xdoc.Root
                        .Element("genericInvokeResponse")
                        .Element("xmlResult")
                        .Element("document")
                        .Element("statusSunat")
                        ?.Value;

                        if (string.IsNullOrEmpty(statusSunat) == true)
                            statusSunat = " ";

                        var messageSunat = xdoc.Root
                        .Element("genericInvokeResponse")
                        .Element("xmlResult")
                        .Element("document")
                        .Element("messageSunat")
                        ?.Value;

                        if (string.IsNullOrEmpty(messageSunat) == true)
                            messageSunat = " ";
                        else
                        {
                            if (statusSunat != "RC_05")
                            {
                                Respuesta respuesta = JsonConvert.DeserializeObject<Respuesta>(messageSunat);
                                messageSunat = respuesta.mensaje;
                            }
                        }


                            str_resultado = statusSunat + "|" + messageSunat;

                        if (string.IsNullOrEmpty(messageSunat.Trim()) == true && string.IsNullOrEmpty(statusSunat.Trim()) == true)
                        {
                            var status = xdoc.Root
                                .Element("genericInvokeResponse")
                                .Element("xmlResult")
                                .Element("document")
                                .Element("status")
                                ?.Value;

                            if (string.IsNullOrEmpty(status) == true)
                                status = " ";

                            var descriptionDetail = xdoc.Root
                                .Element("genericInvokeResponse")
                                .Element("xmlResult")
                                .Element("document")
                                .Element("messages")
                                .Element("descriptionDetail")
                                ?.Value;

                            if (string.IsNullOrEmpty(descriptionDetail) == true)
                                descriptionDetail = " ";

                            str_resultado = status + "|" + descriptionDetail;

                        }


                    }
                }

            }
            catch (Exception ex)
            {
                str_resultado = 1 + "|" + ex.Message;
            }

            return str_resultado;

        }

        public class Respuesta
        {
            public string codigo { get; set; }
            public string mensaje { get; set; }
        }
    }
}
