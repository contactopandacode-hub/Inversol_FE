using COBE;
using COBEC;
using CODAT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ServicioRSNetCore.Controllers.Funciones
{
    public class RegistroPercepcion : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;

        public RegistroPercepcion(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public COBEc_Error Registrar(COBEC_Percepcion cOBEC_Percepcion)
        {
            StringBuilder stb_Resultado = new StringBuilder();
            StreamWriter sw_Documento = null;
            HttpWebRequest obj_Request = default(HttpWebRequest);
            byte[] obj_FileByte = null;
            string str_Resultado = string.Empty;
            COBEc_Error cOBEc_Error = new COBEc_Error();
            CODAT_Percepcion obj_datos = new CODAT_Percepcion(configuration, this.context);
            try
             {
                CompaniaInfo compania = obj_datos.DatosCompania(cOBEC_Percepcion);
                cls_DatosPercepcion percepcion = obj_datos.DatosPercepcion(cOBEC_Percepcion);

                //stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://ws.ce.ebiz.com/\">");
                stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://pse.bizlinks.com/\">");
                stb_Resultado.AppendFormat("<soapenv:Header/>");
                stb_Resultado.AppendFormat("<soapenv:Body>");
                stb_Resultado.AppendFormat("<ws:invoke>");
                stb_Resultado.AppendFormat("<command><![CDATA[<SignOnLinePerceptionCmd declare-sunat=\"1\" declare-direct-sunat=\"1\" publish=\"1\" output=\"PDF\">");

                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"idEmisor\"/>", compania.DocumentoFiscal.Trim());
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"tipoDocumento\"/>", 40);
                stb_Resultado.Append("<documento>");
                stb_Resultado.AppendFormat("<tipoDocumentoEmisor>{0}</tipoDocumentoEmisor>", 6);
                stb_Resultado.AppendFormat("<numeroDocumentoEmisor>{0}</numeroDocumentoEmisor>", compania.DocumentoFiscal);
                stb_Resultado.AppendFormat("<serieNumeroPercepcion>{0}</serieNumeroPercepcion>", cOBEC_Percepcion.numeroDocumento);
                //stb_Resultado.AppendFormat("<serieNumeroPercepcion>{0}</serieNumeroPercepcion>", "P001-00000016");
                stb_Resultado.AppendFormat("<tipoDocumento>{0}</tipoDocumento>", 40);
                stb_Resultado.AppendFormat("<correoEmisor>{0}</correoEmisor>", compania.CorreoElectronico);
                stb_Resultado.AppendFormat("<correoAdquiriente>{0}</correoAdquiriente>", "-");
                stb_Resultado.AppendFormat("<fechaEmision>{0}</fechaEmision>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
                stb_Resultado.AppendFormat("<nombreComercialEmisor>{0}</nombreComercialEmisor>", "-");
                stb_Resultado.AppendFormat("<ubigeoEmisor>{0}</ubigeoEmisor>", compania.Ubigeo);
                stb_Resultado.AppendFormat("<direccionEmisor>{0}</direccionEmisor>", compania.DireccionComun);
                stb_Resultado.AppendFormat("<provinciaEmisor>{0}</provinciaEmisor>", compania.Provincia);
                stb_Resultado.AppendFormat("<departamentoEmisor>{0}</departamentoEmisor>", compania.Departamento);
                stb_Resultado.AppendFormat("<distritoEmisor>{0}</distritoEmisor>", compania.Distrito);
                stb_Resultado.AppendFormat("<codigoPaisEmisor>{0}</codigoPaisEmisor>", "PE");
                stb_Resultado.AppendFormat("<razonSocialEmisor>{0}</razonSocialEmisor>", compania.DescripcionLarga);
                stb_Resultado.AppendFormat("<numeroDocumentoCliente>{0}</numeroDocumentoCliente>", percepcion.DocumentoReceptor);
                stb_Resultado.AppendFormat("<tipoDocumentoCliente>{0}</tipoDocumentoCliente>", percepcion.TipoDocumentoReceptor);
                stb_Resultado.AppendFormat("<nombreComercialCliente>{0}</nombreComercialCliente>", "-");
                stb_Resultado.AppendFormat("<direccionCliente>{0}</direccionCliente>", percepcion.DireccionReceptor);
                stb_Resultado.AppendFormat("<provinciaCliente>{0}</provinciaCliente>", percepcion.ProvinciaReceptor);
                stb_Resultado.AppendFormat("<departamentoCliente>{0}</departamentoCliente>", percepcion.DepartamentoReceptor);
                stb_Resultado.AppendFormat("<distritoCliente>{0}</distritoCliente>", percepcion.DistritoReceptor);
                stb_Resultado.AppendFormat("<codigoPaisCliente>{0}</codigoPaisCliente>", "PE");
                stb_Resultado.AppendFormat("<razonSocialCliente>{0}</razonSocialCliente>", percepcion.RazonSocialReceptor);
                stb_Resultado.AppendFormat("<regimenPercepcion>{0}</regimenPercepcion>", "01");
                stb_Resultado.AppendFormat("<tasaPercepcion>{0}</tasaPercepcion>", "2.00");
                stb_Resultado.AppendFormat("<importeTotalPercibido>{0}</importeTotalPercibido>", percepcion.MontoPercepcion.ToString("F2"));
                stb_Resultado.AppendFormat("<tipoMonedaTotalPercibido>{0}</tipoMonedaTotalPercibido>", percepcion.Moneda);
                stb_Resultado.AppendFormat("<importeTotalCobrado>{0}</importeTotalCobrado>", (percepcion.MontoTotal + percepcion.MontoPercepcion).ToString("F2"));
                stb_Resultado.AppendFormat("<tipoMonedaTotalCobrado>{0}</tipoMonedaTotalCobrado>", percepcion.Moneda);
                stb_Resultado.AppendFormat("<horaEmision>{0}</horaEmision>", percepcion.FechaDocumento.ToString("HH:mm:ss"));
                stb_Resultado.AppendFormat("<isNotificationLocal>{0}</isNotificationLocal>", "false");

                stb_Resultado.Append("<PercepcionItem>");
                stb_Resultado.AppendFormat("<numeroOrdenItem>{0}</numeroOrdenItem>", 1);
                stb_Resultado.AppendFormat("<tipoDocumentoRelacionado>{0}</tipoDocumentoRelacionado>", "01");
                stb_Resultado.AppendFormat("<numeroDocumentoRelacionado>{0}</numeroDocumentoRelacionado>", percepcion.DocumentoRelacionadoPX);                
                stb_Resultado.AppendFormat("<fechaEmisionDocumentoRelacionado>{0}</fechaEmisionDocumentoRelacionado>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
                stb_Resultado.AppendFormat("<importeTotalDocumentoRelacionado>{0}</importeTotalDocumentoRelacionado>", percepcion.MontoTotal.ToString("F2"));
                stb_Resultado.AppendFormat("<tipoMonedaDocumentoRelacionado>{0}</tipoMonedaDocumentoRelacionado>", percepcion.Moneda);
                stb_Resultado.AppendFormat("<fechaCobro>{0}</fechaCobro>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
                stb_Resultado.AppendFormat("<numeroCobro>{0}</numeroCobro>", 1);
                stb_Resultado.AppendFormat("<importeCobro>{0}</importeCobro>", percepcion.MontoTotal.ToString("F2"));
                stb_Resultado.AppendFormat("<monedaCobro>{0}</monedaCobro>", percepcion.Moneda);
                stb_Resultado.AppendFormat("<importePercibido>{0}</importePercibido>", percepcion.MontoPercepcion.ToString("F2"));
                stb_Resultado.AppendFormat("<monedaImportePercibido>{0}</monedaImportePercibido>", percepcion.Moneda);
                stb_Resultado.AppendFormat("<fechaPercepcion>{0}</fechaPercepcion>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
                stb_Resultado.AppendFormat("<importeTotalCobrar>{0}</importeTotalCobrar>", (percepcion.MontoTotal + percepcion.MontoPercepcion).ToString("F2"));
                stb_Resultado.AppendFormat("<monedaMontoTotalCobrar>{0}</monedaMontoTotalCobrar>", percepcion.Moneda);
                stb_Resultado.Append("</PercepcionItem>");
              
                stb_Resultado.Append("</documento>");
                stb_Resultado.Append("</SignOnLinePerceptionCmd>]]></command>");
                stb_Resultado.Append("</ws:invoke>");
                stb_Resultado.Append("</soapenv:Body>");
                stb_Resultado.Append("</soapenv:Envelope>");


                //Solo para ambiente de pruebas
                string str_UbicacionXML;
                string str_URL = compania.urlWebService;
                str_UbicacionXML = @"C:\TRAMA\EDO.TXT";

                //str_URL = "https://testing.bizlinks.com.pe/integrador21/ws/invoker?wsdl";

                if (str_UbicacionXML.Trim() != string.Empty)
                {                    
                    sw_Documento = new StreamWriter(str_UbicacionXML + cOBEC_Percepcion.numeroDocumento + "-" + "40" + "-" + cOBEC_Percepcion.proveedor + ".xml");
                    sw_Documento.Write(stb_Resultado.ToString());
                    sw_Documento.Flush();
                    sw_Documento.Close();
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                CookieContainer myContainer = new CookieContainer();
                obj_Request = (HttpWebRequest)HttpWebRequest.Create(str_URL);
                obj_Request.Method = "POST";
                obj_Request.ContentType = "text/xml;charset=UTF-8";
                //obj_Request.Credentials = new NetworkCredential(compania.URLUsuario, compania.URLPassword);
                // --- SOLO ESTO PARA AUTENTICACIÓN (IGUAL QUE POSTMAN) ---
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(compania.URLUsuario + ":" + compania.URLPassword));
                obj_Request.Headers["Authorization"] = "Basic " + credentials;
                //--- FIN.
                obj_Request.CookieContainer = myContainer;
                obj_FileByte = System.Text.Encoding.UTF8.GetBytes(stb_Resultado.ToString());
                obj_Request.ContentLength = obj_FileByte.Length;

                using (Stream obj_RequestStream = obj_Request.GetRequestStream())
                {
                    obj_RequestStream.Write(obj_FileByte, 0, obj_FileByte.Length);
                }

                //Invocar al Servicio REST
                using (HttpWebResponse obj_Response = (HttpWebResponse)obj_Request.GetResponse())
                {
                    using (Stream obj_ResponseStream = obj_Response.GetResponseStream())
                    {
                        using (StreamReader obj_Reader = new StreamReader(obj_ResponseStream))
                        {
                            str_Resultado = obj_Reader.ReadToEnd();
                        }
                    }
                }
                
                
                XmlNodeList obj_Respuesta = null;                             
                XmlDocument obj_XML = new XmlDocument();
                string str_status = string.Empty;
                string str_Documents = string.Empty;
                long ll_inicio = 1;

                //Cargado la Respuesta
                obj_XML.LoadXml(str_Resultado.Replace("???", String.Empty));
                obj_Respuesta = obj_XML.GetElementsByTagName("return");

                foreach (XmlElement xmlEle_Elemento in obj_Respuesta)
                {
                    str_Resultado = xmlEle_Elemento.InnerText;
                    obj_XML.LoadXml(str_Resultado.Replace("???", String.Empty));

                    //Mensaje de Registro
                    XmlNodeList obj_document = obj_XML.GetElementsByTagName("document");

                    if (obj_document.Count > 0)
                    {
                        foreach (XmlElement xmlEle_Respuesta in obj_document)
                        {
                            if (obj_document.Item(0).OuterXml.Contains("status"))
                                str_status = xmlEle_Respuesta.GetElementsByTagName("status")[0].InnerText;

                            if (str_status == "ERROR")
                                str_Documents = "1|";
                            else
                            {
                                if (obj_document.Item(0).OuterXml.Contains("statusSunat"))
                                    str_Documents = xmlEle_Respuesta.GetElementsByTagName("statusSunat")[0].InnerText + "|";
                                else
                                    str_Documents += "-|";

                                if (obj_document.Item(0).OuterXml.Contains("hashCode"))
                                    str_Documents += xmlEle_Respuesta.GetElementsByTagName("hashCode")[0].InnerText + "|";
                                else
                                    str_Documents += "-|";

                                if (obj_document.Item(0).OuterXml.Contains("pdfFileUrl"))
                                {
                                    str_Documents += xmlEle_Respuesta.GetElementsByTagName("pdfFileUrl")[0].InnerText + "|";
                                    str_Documents += xmlEle_Respuesta.GetElementsByTagName("xmlFileSignUrl")[0].InnerText + "|";
                                }
                                else
                                    str_Documents += "-|-|";
                            }

                        }
                    }
                    else
                        str_Documents = "1|";

                    //Mensaje de respuesta
                    XmlNodeList obj_return = obj_XML.GetElementsByTagName("messages");
                    if (obj_return.Count > 0)
                    {
                        foreach (XmlElement xmlEle_Respuesta in obj_return)
                        {
                            if (ll_inicio == 1)
                                str_Resultado = str_Documents;
                            else
                                str_Resultado += str_Documents;

                            str_Resultado += "Código: " + xmlEle_Respuesta.GetElementsByTagName("codeStatus")[0].InnerText + Environment.NewLine;
                            str_Resultado += "Descripción: " + xmlEle_Respuesta.GetElementsByTagName("descriptionStatus")[0].InnerText + Environment.NewLine;
                            str_Resultado += "Código Detalle: " + xmlEle_Respuesta.GetElementsByTagName("codeDetail")[0].InnerText + Environment.NewLine;
                            str_Resultado += "Descripción Detalle: " + xmlEle_Respuesta.GetElementsByTagName("descriptionDetail")[0].InnerText + Environment.NewLine;

                            ll_inicio++;
                        }
                    }
                    else
                        str_Resultado = "0|" + str_Documents;
                }

                string[] str_ListaResultado = str_Resultado.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                bool existe = str_ListaResultado[1].Contains("Descripción Detalle: El documento ya fue firmado",
                          StringComparison.OrdinalIgnoreCase);

                if (str_ListaResultado[0] == "0")
                {
                    string str_estado = string.Empty;
                    if (str_ListaResultado[1] == "AC_03")
                        str_estado = "AC";
                    else if (str_ListaResultado[1] == "RC_05")
                        str_estado = "RE";
                    else if (str_ListaResultado[1] == "AN_04")
                        str_estado = "AN";
                    else
                        str_estado = "EN";

                    cOBEc_Error.codigo = "00";
                    cOBEc_Error.mensaje = "Registrado con Hash:" + str_ListaResultado[2];

                    ComprobanteREgistroActualiza(cOBEC_Percepcion, "0", str_estado, str_ListaResultado[2], "Registrado correctamente.", str_ListaResultado[3], str_ListaResultado[4]);
                }
                else
                {
                    if (existe == true)
                    {
                        ComprobanteREgistroActualiza(cOBEC_Percepcion, "0", "EN", "Registrado correctamente.", "Registrado correctamente.", string.Empty, string.Empty);
                    }
                    else
                    {
                        ComprobanteREgistroActualiza(cOBEC_Percepcion, "1", string.Empty, string.Empty, str_ListaResultado[1], string.Empty, string.Empty);
                        cOBEc_Error.codigo = "01";
                        cOBEc_Error.mensaje = "Error en el registro:" + str_ListaResultado[1];
                    }
                }


                //if (str_ListaResultado[0] == "0")
                //{
                //    cOBEc_Error.codigo = "00";
                //    cOBEc_Error.mensaje = "Procesado Correctamente:" + str_ListaResultado[1];

                //}
                //else
                //{
                //    cOBEc_Error.codigo = "01";
                //    cOBEc_Error.mensaje = "Error en el registro:" + str_ListaResultado[1];

                //}


            }
            catch (Exception e)
            {
                cOBEc_Error.mensaje = e.ToString();
                throw;
            }
            return cOBEc_Error;
        }

        public string ComprobanteREgistroActualiza(COBEC_Percepcion cOBEC_Percepcion, string par_tipo, string par_feEstado, string par_fehascode, string observaciones,
                                                   string par_rutaPdf, string par_rutaXml)
        {
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
            string str_return = string.Empty;

            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_API_DatosActualizar";
                _command.Connection.Open();

                SqlParameter pCompania = new SqlParameter("@pCompania", cOBEC_Percepcion.companiaSocio);
                _command.Parameters.Add(pCompania);

                SqlParameter pTipoDocumento = new SqlParameter("@pTipoDocumento", "PX");
                _command.Parameters.Add(pTipoDocumento);

                SqlParameter pNumeroDocumento = new SqlParameter("@pNumeroDocumento", cOBEC_Percepcion.numeroDocumento);
                _command.Parameters.Add(pNumeroDocumento);

                SqlParameter pParTipo = new SqlParameter("@pParTipo", par_tipo);
                _command.Parameters.Add(pParTipo);

                SqlParameter pFeEstado = new SqlParameter("@pFeEstado", par_feEstado);
                _command.Parameters.Add(pFeEstado);

                SqlParameter pHashCode = new SqlParameter("@pHashCode", par_fehascode);
                _command.Parameters.Add(pHashCode);

                SqlParameter pObservaciones = new SqlParameter("@pObservaciones", observaciones);
                _command.Parameters.Add(pObservaciones);

                SqlParameter pRutaPDF = new SqlParameter("@pRutaPDF", par_rutaPdf);
                _command.Parameters.Add(pRutaPDF);

                SqlParameter pRutaXML = new SqlParameter("@pRutaXML", par_rutaXml);
                _command.Parameters.Add(pRutaXML);

                _command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine("Consultas Adicionales - " + e.Message);
                _command.Connection.Close();
            }

            return str_return;
        }


    }
}
