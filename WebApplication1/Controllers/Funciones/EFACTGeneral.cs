using COBE;
using COBEC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace ServicioRSNetCore.Controllers.Funciones
{
    public class EFACTGeneral : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;

        public EFACTGeneral(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public COBEc_Error GeneraToken(COBEC_Generico request )
        {
            string str_resultado = string.Empty;
            COBEc_Error obj_retorno = new COBEc_Error();
            try
            {
                using (var obj_Cliente = new HttpClient())
                {
                    string str_ServicioUsuarioClave = "client" + ":" + "secret";
                    byte[] byt_UsuarioClave = Encoding.UTF8.GetBytes(str_ServicioUsuarioClave.ToCharArray());

                    obj_Cliente.DefaultRequestHeaders.Accept.Clear();
                    obj_Cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    obj_Cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byt_UsuarioClave));

                    var obj_Parametros = new Dictionary<string, string>();
                    obj_Parametros.Add("grant_type", "password");
                    obj_Parametros.Add("username", request.tokenUsuario);
                    obj_Parametros.Add("password", request.tokenClave);

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    var obj_Response = obj_Cliente.PostAsync(configuration["WebService"] + "/oauth/token", new FormUrlEncodedContent(obj_Parametros)).Result;
                    str_resultado = obj_Response.Content.ReadAsStringAsync().Result;

                    var obj_Resultado = JObject.Parse(str_resultado);

                    if (obj_Resultado["error"] != null)
                    {                      
                        obj_retorno.codigo = "01";
                        obj_retorno.mensaje = obj_Resultado["error"].ToString() + "-" + obj_Resultado["error_description"].ToString()   ;
                        return obj_retorno;
                    }
                    obj_retorno.codigo = "00";
                    obj_retorno.mensaje = obj_Resultado["access_token"].ToString();                   
                }
            }
            catch (Exception ex)
            {              
                obj_retorno.codigo = "01";
                obj_retorno.mensaje = ex.Message;
            }

            return obj_retorno;
        }

        public COBEc_Error EfactConsultaEstado(string identificador, string par_token)
        {
            string str_Resultado = string.Empty;
            string str_Token = "-";
            COBEc_Error obj_retorno = new COBEc_Error();

            try
            {
                using (var obj_Cliente = new HttpClient())
                {

                    obj_Cliente.DefaultRequestHeaders.Add("Authorization", "bearer " + par_token);
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    var obj_Response = obj_Cliente.GetAsync(configuration["WebService"] + "/v1/cdr/" + identificador).Result;
                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;

                    if (str_Resultado.Substring(0, 5) == "<?xml")
                    {
                        var obj_XmlSunat = new XmlDocument();
                        obj_XmlSunat.LoadXml(str_Resultado);

                        string str_CodigoRespuestaSunat = obj_XmlSunat.GetElementsByTagName("ns3:DocumentResponse")[0].ChildNodes[0].ChildNodes[1].InnerText;
                        string str_DescripcionRespuestaSunat = obj_XmlSunat.GetElementsByTagName("ns3:DocumentResponse")[0].ChildNodes[0].ChildNodes[2].InnerText;

                        var nsManager = new XmlNamespaceManager(obj_XmlSunat.NameTable);
                        nsManager.AddNamespace("ns3", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                        nsManager.AddNamespace("bc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

                        XmlNode hashNode = obj_XmlSunat.SelectSingleNode("//ns3:DocumentResponse/ns3:DocumentReference/ns3:Attachment/ns3:ExternalReference/bc:DocumentHash", nsManager);

                        string str_hashCode = string.Empty;
                        if (hashNode != null)
                        {
                            str_hashCode = hashNode.InnerText;
                        }

                        if (str_CodigoRespuestaSunat == "0")
                        {
                            obj_retorno.codigo = "00";
                            obj_retorno.mensaje="AP|" + str_hashCode + "|" + str_DescripcionRespuestaSunat + "|" + str_Token;
                        }
                        else
                        {
                            obj_retorno.codigo = "00";
                            obj_retorno.mensaje = "PE|" + str_DescripcionRespuestaSunat;
                        }
                    }
                    else
                    {
                        var obj_Estado = JObject.Parse(str_Resultado);                                                                        
                        obj_retorno.codigo = "01";
                        obj_retorno.mensaje = obj_Estado["code"].ToString()  + obj_Estado["description"].ToString(); 
                    }
                }
            }
            catch (Exception ex)
            {
                obj_retorno.codigo = "01";
                obj_retorno.mensaje = ex.Message;
            }
            return obj_retorno;
        }

        public byte[] ObtenerAdjuntosByte(COBEC_DatosAdjunto request)
        {
            string str_Resultado = string.Empty;
            COBEc_Error obj_retorno = new COBEc_Error();
            byte[] pdfBytes = new byte[0];
            try
            {
                using (var obj_Cliente = new HttpClient())
                {
                    string str_ServicioUsuarioClave = "client" + ":" + "secret";
                    byte[] byt_UsuarioClave = Encoding.UTF8.GetBytes(str_ServicioUsuarioClave.ToCharArray());

                    obj_Cliente.DefaultRequestHeaders.Accept.Clear();
                    obj_Cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    obj_Cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byt_UsuarioClave));

                    var obj_Parametros = new Dictionary<string, string>();
                    obj_Parametros.Add("grant_type", "password");
                    obj_Parametros.Add("username", request.username);
                    obj_Parametros.Add("password", request.password);

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    var obj_Response = obj_Cliente.PostAsync(request.url + "/oauth/token", new FormUrlEncodedContent(obj_Parametros)).Result;

                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;
                    var obj_Resultado = JObject.Parse(str_Resultado);

                    if (obj_Resultado["error"] != null)
                    {

                        //obj_retorno.codigo = "01";
                        //obj_retorno.mensaje = obj_Resultado["error"].ToString() + "-" + obj_Resultado["error_description"].ToString();
                    }

                    string str_Token = obj_Resultado["access_token"].ToString();

                    using (var pdfClient = new HttpClient())
                    {
                        pdfClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", str_Token);
                        pdfClient.DefaultRequestHeaders.Accept.Clear();
                        pdfClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));

                        string pdfUrl = string.Empty;

                        if (request.tipo == "1")
                            pdfUrl = request.url + "/v1/pdf/" + request.identificador;

                        if (request.tipo == "2")
                            pdfUrl = request.url + "/v1/xml/" + request.identificador;

                        if (request.tipo == "3")
                            pdfUrl = request.url + "/v1/cdr/" + request.identificador;

                        var pdfResponse = pdfClient.GetAsync(pdfUrl).Result;
                        pdfResponse.EnsureSuccessStatusCode();

                       pdfBytes = pdfResponse.Content.ReadAsByteArrayAsync().Result;


                        File.WriteAllBytes("C:\\PANDA\\INVERSOL\\prueba.pdf", pdfBytes);

                        //str_Resultado = "00|Adjunto descargando.";
                        return pdfBytes;                       
                    }
                }

            }
            catch (HttpRequestException ex)
            {

                //obj_retorno.codigo = "01";
                //obj_retorno.mensaje = "Error al descargar el Adjunto: " + ex.Message;
            }
            catch (Exception ex)
            {
               

                //obj_retorno.codigo = "02";
                //obj_retorno.mensaje = "Error no controlado: " + ex.Message;
            }

            return pdfBytes;
        }
    }
}
