//using COBE;
//using COBEC;
//using CODAT;
//using System;
//using System.Configuration;
//using System.IO;
//using System.Net;
//using System.Text;

//namespace ServicioRSNetCore.Controllers.Funciones
//{
//    public class percepcionv2
//    {
//        public COBEc_Error Registrar(COBEC_Percepcion cOBEC_Percepcion)
//        {
//            StringBuilder stb_Resultado = new StringBuilder();
//            StreamWriter sw_Documento = null;
//            HttpWebRequest obj_Request = default(HttpWebRequest);
//            byte[] obj_FileByte = null;
//            string str_Resultado = string.Empty;
//            COBEc_Error cOBEc_Error = new COBEc_Error();
//            CODAT_Percepcion obj_datos = new CODAT_Percepcion(configuration, this.context);

//            try
//            {
//                CompaniaInfo compania = obj_datos.DatosCompania(cOBEC_Percepcion);
//                cls_DatosPercepcion percepcion = obj_datos.DatosPercepcion(cOBEC_Percepcion);

//                stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://ws.ce.ebiz.com/\">");
//                stb_Resultado.AppendFormat("<soapenv:Header/>");
//                stb_Resultado.AppendFormat("<soapenv:Body>");
//                stb_Resultado.AppendFormat("<ws:invoke>");
//                stb_Resultado.AppendFormat("<command><![CDATA[<ConsultCmd output=\"PDF\">");
//                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"idEmisor\"/>", compania.DocumentoFiscal.Trim());
//                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"tipoDocumento\"/>", 40);

//                stb_Resultado.Append("<documento>");

//                stb_Resultado.AppendFormat("<tipoDocumentoEmisor>{0}</tipoDocumentoEmisor>", '-');
//                stb_Resultado.AppendFormat("<numeroDocumentoEmisor>{0}</numeroDocumentoEmisor>", compania.DocumentoFiscal);
//                stb_Resultado.AppendFormat("<serieNumeroPercepcion>{0}</serieNumeroPercepcion>", '-');
//                stb_Resultado.AppendFormat("<tipoDocumento>{0}</tipoDocumento>", '-');
//                stb_Resultado.AppendFormat("<correoEmisor>{0}</correoEmisor>", compania.CorreoElectronico);
//                stb_Resultado.AppendFormat("<correoAdquiriente>{0}</correoAdquiriente>", 11);
//                stb_Resultado.AppendFormat("<fechaEmision>{0}</fechaEmision>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
//                stb_Resultado.AppendFormat("<nombreComercialEmisor>{0}</nombreComercialEmisor>", '-');
//                stb_Resultado.AppendFormat("<ubigeoEmisor>{0}</ubigeoEmisor>", compania.Ubigeo);
//                stb_Resultado.AppendFormat("<direccionEmisor>{0}</direccionEmisor>", compania.DireccionComun);
//                stb_Resultado.AppendFormat("<provinciaEmisor>{0}</provinciaEmisor>", compania.Provincia);
//                stb_Resultado.AppendFormat("<departamentoEmisor>{0}</departamentoEmisor>", compania.Departamento);
//                stb_Resultado.AppendFormat("<distritoEmisor>{0}</distritoEmisor>", compania.Distrito);
//                stb_Resultado.AppendFormat("<codigoPaisEmisor>{0}</codigoPaisEmisor>", "PE");
//                stb_Resultado.AppendFormat("<razonSocialEmisor>{0}</razonSocialEmisor>", compania.DescripcionLarga);
//                stb_Resultado.AppendFormat("<numeroDocumentoCliente>{0}</numeroDocumentoCliente>", percepcion.DocumentoReceptor);
//                stb_Resultado.AppendFormat("<tipoDocumentoCliente>{0}</tipoDocumentoCliente>", percepcion.TipoDocumentoReceptor);
//                stb_Resultado.AppendFormat("<direccionCliente>{0}</direccionCliente>", percepcion.DireccionReceptor);
//                stb_Resultado.AppendFormat("<provinciaCliente>{0}</provinciaCliente>", percepcion.ProvinciaReceptor);
//                stb_Resultado.AppendFormat("<departamentoCliente>{0}</departamentoCliente>", percepcion.DepartamentoReceptor);
//                stb_Resultado.AppendFormat("<distritoCliente>{0}</distritoCliente>", "");
//                stb_Resultado.AppendFormat("<codigoPaisCliente>{0}</codigoPaisCliente>", "PE");
//                stb_Resultado.AppendFormat("<razonSocialCliente>{0}</razonSocialCliente>", percepcion.RazonSocialReceptor);
//                stb_Resultado.AppendFormat("<regimenPercepcion>{0}</regimenPercepcion>", 01);
//                stb_Resultado.AppendFormat("<tasaPercepcion>{0}</tasaPercepcion>", 2.00);
//                stb_Resultado.AppendFormat("<importeTotalPercibido>{0}</importeTotalPercibido>", percepcion.MontoPercepcion);
//                stb_Resultado.AppendFormat("<tipoMonedaTotalPercibido>{0}</tipoMonedaTotalPercibido>", percepcion.Moneda);
//                stb_Resultado.AppendFormat("<importeTotalCobrado>{0}</importeTotalCobrado>", percepcion.MontoTotal + percepcion.MontoPercepcion);
//                stb_Resultado.AppendFormat("<tipoMonedaTotalCobrado>{0}</tipoMonedaTotalCobrado>", percepcion.Moneda);
//                stb_Resultado.AppendFormat("<horaEmision>{0}</horaEmision>", percepcion.FechaDocumento.ToString("HH:mm:ss"));
//                stb_Resultado.AppendFormat("<isNotificationLocal>{0}</isNotificationLocal>", "false");

//                stb_Resultado.Append("<PercepcionItem>");
//                stb_Resultado.AppendFormat("<numeroOrdenItem>{0}</numeroOrdenItem>", 1);
//                stb_Resultado.AppendFormat("<tipoDocumentoRelacionado>{0}</tipoDocumentoRelacionado>", percepcion.TipoRelacionado);
//                stb_Resultado.AppendFormat("<numeroDocumentoRelacionado>{0}</numeroDocumentoRelacionado>", percepcion.DocumentoRelacionadoPX);
//                stb_Resultado.AppendFormat("<fechaEmisionDocumentoRelacionado>{0}</fechaEmisionDocumentoRelacionado>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
//                stb_Resultado.AppendFormat("<importeTotalDocumentoRelacionado>{0}</importeTotalDocumentoRelacionado>", percepcion.MontoTotal);
//                stb_Resultado.AppendFormat("<tipoMonedaDocumentoRelacionado>{0}</tipoMonedaDocumentoRelacionado>", percepcion.Moneda);
//                stb_Resultado.AppendFormat("<fechaCobro>{0}</fechaCobro>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
//                stb_Resultado.AppendFormat("<numeroCobro>{0}</numeroCobro>", 1);
//                stb_Resultado.AppendFormat("<importeCobro>{0}</importeCobro>", percepcion.MontoTotal);
//                stb_Resultado.AppendFormat("<monedaCobro>{0}</monedaCobro>", percepcion.Moneda);
//                stb_Resultado.AppendFormat("<importePercibido>{0}</importePercibido>", percepcion.MontoPercepcion);
//                stb_Resultado.AppendFormat("<monedaImportePercibido>{0}</monedaImportePercibido>", percepcion.Moneda);
//                stb_Resultado.AppendFormat("<fechaPercepcion>{0}</fechaPercepcion>", percepcion.FechaDocumento.ToString("yyyy-MM-dd"));
//                stb_Resultado.AppendFormat("<importeTotalCobrar>{0}</importeTotalCobrar>", percepcion.MontoTotal + percepcion.MontoPercepcion);
//                stb_Resultado.AppendFormat("<monedaMontoTotalCobrar>{0}</monedaMontoTotalCobrar>", percepcion.Moneda);
//                stb_Resultado.Append("</PercepcionItem>");

//                stb_Resultado.Append("</documento>");
//                stb_Resultado.Append("</SignOnLineDespatchCmd>]]></command>");
//                stb_Resultado.Append("</ws:invoke>");
//                stb_Resultado.Append("</soapenv:Body>");
//                stb_Resultado.Append("</soapenv:Envelope>");

//            //    //Solo para ambiente de pruebas
//            //    string str_UbicacionXML;
//            //    string str_URL;
//            //    str_UbicacionXML = @"C:\TRAMA\EDO.TXT";
//            //    str_URL = "https://testing.bizlinks.com.pe/integrador21/ws/invoker?wsdl";
//            //    if (str_UbicacionXML.Trim() != string.Empty)
//            //    {
//            //        //sw_Documento = new StreamWriter(str_UbicacionXML + str_numeroDocumentoRemitente + "-" + "09" + "-" + str_serieNumeroGuia + ".xml");
//            //        sw_Documento = new StreamWriter(str_UbicacionXML + 99 + "-" + "09" + "-" + 99 + ".xml");
//            //        sw_Documento.Write(stb_Resultado.ToString());
//            //        sw_Documento.Flush();
//            //        sw_Documento.Close();
//            //    }

//            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//            //    CookieContainer myContainer = new CookieContainer();
//            //    obj_Request = (HttpWebRequest)HttpWebRequest.Create(str_URL);
//            //    obj_Request.Method = "POST";
//            //    obj_Request.ContentType = "text/xml;charset=UTF-8";
//            //    obj_Request.Credentials = new NetworkCredential(compania.URLUsuario, compania.URLPassword);
//            //    obj_Request.CookieContainer = myContainer;
//            //    obj_FileByte = System.Text.Encoding.UTF8.GetBytes(stb_Resultado.ToString());
//            //    obj_Request.ContentLength = obj_FileByte.Length;

//            //    using (Stream obj_RequestStream = obj_Request.GetRequestStream())
//            //    {
//            //        obj_RequestStream.Write(obj_FileByte, 0, obj_FileByte.Length);
//            //    }

//            //    //Invocar al Servicio REST
//            //    using (HttpWebResponse obj_Response = (HttpWebResponse)obj_Request.GetResponse())
//            //    {
//            //        using (Stream obj_ResponseStream = obj_Response.GetResponseStream())
//            //        {
//            //            using (StreamReader obj_Reader = new StreamReader(obj_ResponseStream))
//            //            {
//            //                str_Resultado = obj_Reader.ReadToEnd();
//            //            }
//            //        }
//            //    }


//            //}
//            //catch (Exception)
//            //{

//            //    throw;
//            //}
//            //return cOBEc_Error;
//        }

//        }

//}
