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
    public class EFACTRegistro
    {
        public string Comprobante(
            string str_ComprobanteSerie, string str_ComprobanteNumero, DateTime dtm_ComprobanteFechaEmision,
            string str_ComprobanteHoraEmision, string str_ComprobanteTipoComprobante, string str_ComprobanteTipoOperacion,
            string str_ComprobanteTipoMoneda, string str_ComprobanteGlosa,
            decimal dec_ComprobanteTotalGravada, decimal dec_ComprobanteTotalNoGravada, decimal dec_ComprobanteTotalExonerada,
            decimal dec_ComprobanteTotalIGV, decimal dec_ComprobanteTotalGratuito,
            decimal dec_ComprobanteValorVenta, decimal dec_ComprobanteMontoBaseDescuentoGlobal, decimal dec_ComprobantePorcentajeDescuentoGlobal,
            decimal dec_ComprobanteTotalDescuentoGlobal, decimal dec_TotalDocumentoAnticipo, decimal dec_MontoBaseDsctoGlobalAnticipo,
            decimal dec_PorcentajeDsctoGlobalAnticipo, decimal dec_TotalDsctoGlobalesAnticipo, decimal dec_ComprobanteImporteTotal,
            string str_ComprobanteImporteTotalLetras, string str_EmpresaDepartamento, string str_EmpresaProvincia,
            string str_EmpresaDistrito, string str_EmpresaUbigeo, string str_EmpresaDireccion, string str_EmpresaUrbanizacion,
            string str_EmpresaRazonSocial, string str_EmpresaNombreComercial, string str_EmpresaCorreoElectronico,
            string str_EmpresaEstablecimientoSunat, string str_EmpresaTipoDocumento, string str_EmpresaNumeroDocumento,
            string str_FormaPagoCondicion, string str_ReceptorTipoDocumento, string str_ReceptorNumeroDocumento,
            string str_ReceptorRazonSocial, string str_ReceptorCorreoElectronico, string str_ReceptorDireccion,
            string str_ReceptorUbigeo, string Str_ReceptorUrbanizacion, string str_ReceptorProvincia,
            string str_ReceptorDepartamento, string str_ReceptorDistrito, string str_ReceptorPais,
            decimal dec_MontoDetraccion, decimal dec_DetraccionPorcentaje, string str_DetraccionNumeroCuenta,
            decimal dec_DetraccionValorReferencial, string str_DetraccionBienServicio, string str_DetraccionDescripcion,
            string str_VendedorNombre, string str_NumeroOrdenCompra,
            string str_NumeroPedido, string str_ComprobanteDetalleTrama, string str_ComprobantePrePagoTrama,
            string str_ComprobanteNotaDocRefTrama, string str_ComprobanteMotivoDocumento, string str_UbicacionXML,
            string str_URL, string str_TipoFormaPago, string str_GlosaFormaPago, decimal dec_MontoPendientePago,
            decimal dec_totalTributosOpeGratuitas, string str_regimenPercepcion, decimal dec_baseImponiblePercepcion,
            decimal dec_porcentajePercepcion, decimal dec_totalPercepcion, decimal dec_totalVentaConPercepcion, string str_sucursal,
            decimal dec_montoBaseICBPER, decimal dec_totalMontoICBPER, string str_lugarDespacho, decimal dec_totalPrecioVenta,
            int cantidadLineas, string str_ServicioUsuario, string str_ServicioClave, decimal dec_MontoRecargo, decimal dec_MontoTotalExportacion,
            int int_cantidadGuias, string str_guias, string str_fechaVencimiento, decimal ldc_montoOtroExportacion, decimal ldc_montoBruto,
            decimal ldc_montoFlete, decimal ldc_montoSeguro, decimal ldc_montoDetraccionOrigen, decimal ldc_montoGastosDestino
            )
        {
            StringBuilder stb_Resultado = new StringBuilder();
            string str_ResultadoComprobante = string.Empty;
            XmlDocument obj_XML = new XmlDocument();
            string str_MensajeSunat = string.Empty;
            string str_ResultadoPDF = string.Empty;
            string str_EstadoSunat = string.Empty;
            string str_CodigoHash = string.Empty;
            string str_RequestUrl = string.Empty;
            string str_EnlacePDF = string.Empty;
            //StreamWriter sw_Documento = null;
            string str_Resultado = string.Empty;
            string str_MotivoNC = string.Empty;
            string str_separador = ",";
            string str_vacio = "";

            try
            {
                /************ FILA 1 - Datos del Documento *************/

                stb_Resultado.AppendFormat("{0}{1}", dtm_ComprobanteFechaEmision.ToString("yyyy-MM-dd"), str_separador); //A
                stb_Resultado.AppendFormat("{0}-{1}{2}", str_ComprobanteSerie, str_ComprobanteNumero, str_separador); //B
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteTipoComprobante, str_separador); //C
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteTipoMoneda, str_separador); //D

                if (dec_ComprobanteTotalExonerada > 0 && dec_ComprobanteTotalGravada == 0)
                    stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //E
                else
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalGravada.ToString("F2"), str_separador); //E

                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalIGV.ToString("F2"), str_separador); //F
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteTipoMoneda, str_separador); //G

                //Datos ISC
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //H
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //I
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //J

                //Sumatoria Otros Tributos
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //K
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //L
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //M

                //Importe Total Comprobante
                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteImporteTotal.ToString("F2"), str_separador); //N

                //Campos en Plomo
                if (dec_TotalDsctoGlobalesAnticipo > 0 || str_ReceptorTipoDocumento == "0")
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //O
                else
                {
                    if (str_TipoFormaPago == "0")
                        stb_Resultado.AppendFormat("{0}{1}", "Contado", str_separador); //O}
                    else
                        stb_Resultado.AppendFormat("{0}{1}", "Credito", str_separador); //O
                }

                if (str_ComprobanteTipoOperacion == "1001" || str_TipoFormaPago == "1")
                    stb_Resultado.AppendFormat("{0}{1}", dec_MontoPendientePago.ToString("F2"), str_separador); //P
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //P

                //Tipo Operación
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteTipoOperacion, str_separador); //Q

                //Campos en Plomo
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //R
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //S

                //Sumatoria de Impuestos Operaciones Gratuitas
                if (dec_totalTributosOpeGratuitas > 0 || dec_ComprobanteTotalGratuito > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalGratuito.ToString("F2"), str_separador); //T
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //T

                //Total Operaciones Exportación
                if (ldc_montoBruto > 0)
                    stb_Resultado.AppendFormat("{0}{1}", (ldc_montoBruto).ToString("F2"), str_separador); //U
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //U

                //Total operaciones gravadas IGV o IVAP
                if (dec_ComprobanteTotalIGV > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalGravada.ToString("F2"), str_separador); //V
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //V

                //Total operaciones Inafectas
                if (dec_ComprobanteTotalNoGravada > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalNoGravada.ToString("F2"), str_separador); //W
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //W

                //Total operaciones Exonerada
                if (dec_ComprobanteTotalExonerada > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalExonerada.ToString("F2"), str_separador); //X
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //X

                //Total operaciones Gratuitas
                if (dec_ComprobanteTotalGratuito > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalGratuito.ToString("F2"), str_separador); //Y
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Y

                //Campos en Plomo
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Z

                //Datos Percepcion
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AA
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AB
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AC

                //Datos Detracción
                if (str_ComprobanteTipoComprobante == "01" && dec_MontoDetraccion > 0)
                {
                    stb_Resultado.AppendFormat("{0}{1}", str_DetraccionBienServicio, str_separador); //AD
                    stb_Resultado.AppendFormat("{0}{1}", dec_MontoDetraccion.ToString("F2"), str_separador); //AE
                    stb_Resultado.AppendFormat("{0}{1}", dec_DetraccionPorcentaje.ToString("F2"), str_separador); //AF
                    stb_Resultado.AppendFormat("{0}{1}", str_DetraccionNumeroCuenta, str_separador); //AG
                    stb_Resultado.AppendFormat("{0}{1}", (dec_ComprobanteImporteTotal - ldc_montoDetraccionOrigen).ToString("F2"), str_separador); //AH
                }
                else
                {
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AD

                    if (str_ComprobanteTipoComprobante == "01" || str_ComprobanteTipoComprobante == "03")
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AE
                    else
                        stb_Resultado.AppendFormat("{0}{1}", cantidadLineas.ToString(), str_separador); //AE

                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AF
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AG
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AH
                }

                //Campos en Plomo
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AI
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AJ
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AK

                //Cantidad de Lineas Detalle
                if (str_ComprobanteTipoComprobante == "01" || str_ComprobanteTipoComprobante == "03")
                    stb_Resultado.AppendFormat("{0}{1}", cantidadLineas.ToString(), str_separador); //AL
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio.ToString(), str_separador); //AL

                //Código Regimen Percepción
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AM

                //Cantidad de guias y otro documentos asociados                
                string[] str_ListaComprobanteCuotas = str_GlosaFormaPago.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                int cantidadRelacionados = str_ListaComprobanteCuotas.Length + int_cantidadGuias;

                if (str_ComprobanteTipoComprobante == "01" || str_ComprobanteTipoComprobante == "03")
                {
                    if (cantidadRelacionados > 0)
                        stb_Resultado.AppendFormat("{0}{1}", cantidadRelacionados.ToString(), str_separador); //AN
                    else
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AN
                }
                else
                {
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AN
                }

                //Cantidad Anticipos asociados
                if (dec_TotalDocumentoAnticipo > 0)
                    stb_Resultado.AppendFormat("{0}{1}", "1", str_separador); //AO
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AO

                //Total Anticipos
                if (dec_TotalDocumentoAnticipo > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_TotalDocumentoAnticipo, str_separador); //AP
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AO

                //Cantidad Punto Partida y LLegada
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AQ

                //Monto Descuento Global AB
                if (dec_ComprobanteTotalDescuentoGlobal > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalDescuentoGlobal.ToString("F2"), str_separador); //AR
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AR

                //Monto Descuento Global No AB
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AS

                //Monto Anticipo Gravado IGV o IVAP
                if (dec_TotalDsctoGlobalesAnticipo > 0 && dec_ComprobanteTotalGravada > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_TotalDsctoGlobalesAnticipo, str_separador); //AT
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AT

                //Monto Anticipo Exonerado
                if (dec_TotalDsctoGlobalesAnticipo > 0 && dec_ComprobanteTotalExonerada > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_TotalDsctoGlobalesAnticipo, str_separador); //AU
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AU

                //Monto Anticipo Inafecto
                if (dec_TotalDsctoGlobalesAnticipo > 0 && dec_ComprobanteTotalNoGravada > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_TotalDsctoGlobalesAnticipo, str_separador); //AV
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AV
                                                                                    //Monto Base FISE
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AW

                //Monto Total FISE
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AX

                //Recargo al Consumo y/o Propinas
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AY

                //Monto Cargo Global AB
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AZ

                //Monto Cargo Global No AB
                if (ldc_montoOtroExportacion > 0)
                    stb_Resultado.AppendFormat("{0}{1}", ldc_montoOtroExportacion, str_separador); //BA
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //BA

                //Monto Total Impuesto
                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalIGV.ToString("F2"), str_separador); //BB

                //Total Valor de Venta
                stb_Resultado.AppendFormat("{0}{1}", (dec_ComprobanteTotalGravada + dec_ComprobanteTotalNoGravada + dec_ComprobanteTotalExonerada + ldc_montoBruto + dec_ComprobanteTotalDescuentoGlobal + dec_TotalDsctoGlobalesAnticipo).ToString("F2"), str_separador); //BC

                //Total Precio de Venta
                if (ldc_montoBruto > 0)
                    stb_Resultado.AppendFormat("{0}{1}", (ldc_montoBruto + dec_TotalDocumentoAnticipo).ToString("F2"), str_separador); //BD
                else
                    stb_Resultado.AppendFormat("{0}{1}", (dec_ComprobanteImporteTotal + dec_TotalDocumentoAnticipo).ToString("F2"), str_separador); //BD

                //Total Descuento no AB
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //BE

                //Total Cargos no AB
                if (ldc_montoOtroExportacion > 0)
                    stb_Resultado.AppendFormat("{0}{1}", ldc_montoOtroExportacion, str_separador);//BF
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //BF


                //Monto para Redondeo del Importe Total
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //BG

                //Total Descuento AB
                if (dec_ComprobanteTotalDescuentoGlobal > 0)
                    stb_Resultado.AppendFormat("{0}{1}{2}", dec_ComprobanteTotalDescuentoGlobal.ToString("F2"), str_separador, Environment.NewLine); //BH
                else
                    stb_Resultado.AppendFormat("{0}{1}{2}", str_vacio, str_separador, Environment.NewLine); //BH

                /************ FILA 2 - Sustento de Traslado de Mercaderia *******************************/
                stb_Resultado.AppendFormat("{0}{1}{2}", str_vacio, str_separador, Environment.NewLine); //BH

                /************ FILA 3 - Información de Anticipos Asociados *******************************/

                string[] str_ListaPrePagoTrama = str_ComprobantePrePagoTrama.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_ListaPrePagoTrama.Length > 0)
                {
                    for (int int_Fila = 0; int_Fila < str_ListaPrePagoTrama.Length; int_Fila++)
                    {
                        string[] str_RegistroComprobanteCuotas = str_ListaPrePagoTrama[int_Fila].Split(Convert.ToChar("|"));
                        stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteTipoMoneda, str_separador); //A
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteCuotas[5], str_separador); //B
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteCuotas[3], str_separador); //C
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteCuotas[4], str_separador); //D
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteCuotas[6], str_separador); //E
                        stb_Resultado.AppendFormat("{0}{1}{2}", "PREPAID_DOC", str_separador, Environment.NewLine); //F
                    }
                }
                else
                {
                    stb_Resultado.AppendFormat("{0}{1}{2}", str_vacio, str_separador, Environment.NewLine); //BH
                }

                /************ FILA 4 - Información de Guias y Otros Documentos Relacionados *************/
                //Datos del detalle               
                if (cantidadRelacionados > 0)
                {
                    string[] str_ListaGuias = str_guias.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    for (int int_Fila = 0; int_Fila < str_ListaGuias.Length; int_Fila++)
                    {
                        string[] str_RegistroComprobanteCuotas = str_ListaGuias[int_Fila].Split(Convert.ToChar("|"));
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteCuotas[0], str_separador); //A
                        stb_Resultado.AppendFormat("{0}{1}", "09", str_separador); //B
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //C
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //D
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //E
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //F
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //G
                        stb_Resultado.AppendFormat("{0}{1}{2}", "ATTACH_DOC", str_separador, Environment.NewLine); //H

                    }

                    for (int int_Fila = 0; int_Fila < str_ListaComprobanteCuotas.Length; int_Fila++)
                    {
                        string[] str_RegistroComprobanteCuotas = str_ListaComprobanteCuotas[int_Fila].Split(Convert.ToChar("|"));
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //A
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //B
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //C
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //D
                        stb_Resultado.AppendFormat("{0}{1}", "Cuota" + (int_Fila + 1).ToString("D3"), str_separador); //E
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteCuotas[1], str_separador); //F
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteCuotas[0], str_separador); //G
                        stb_Resultado.AppendFormat("{0}{1}{2}", "ATTACH_DOC", str_separador, Environment.NewLine); //H
                    }
                }
                else
                {
                    stb_Resultado.AppendFormat("{0}{1}{2}", str_vacio, str_separador, Environment.NewLine); //H
                }

                /************ FILA 5 - Información del Emisor *******************************************/
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaRazonSocial.Replace(",", " "), str_separador); //A

                if (str_EmpresaNombreComercial.Trim() != string.Empty)
                    stb_Resultado.AppendFormat("{0}{1}", str_EmpresaRazonSocial.Replace(",", " "), str_separador); //B
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_EmpresaNombreComercial.Replace(",", " "), str_separador); //B

                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaNumeroDocumento, str_separador); //C
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaUbigeo, str_separador); //D
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaDireccion.Replace(",", " "), str_separador); //E

                if (str_EmpresaUrbanizacion.Trim() != string.Empty)
                    stb_Resultado.AppendFormat("{0}{1}", str_EmpresaUrbanizacion.Replace(",", " "), str_separador); //F
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //F

                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaDepartamento, str_separador); //G
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaProvincia, str_separador); //H
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaDistrito, str_separador); //I
                stb_Resultado.AppendFormat("{0}{1}", "PE", str_separador); //J
                stb_Resultado.AppendFormat("{0}{1}{2}", str_EmpresaEstablecimientoSunat, str_separador, Environment.NewLine); //K

                /************ FILA 6 - Información del Receptor *****************************************/
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorNumeroDocumento, str_separador); //A
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorTipoDocumento, str_separador); //B
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorRazonSocial.Replace(",", " "), str_separador); //C
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorRazonSocial.Replace(",", " "), str_separador); //D

                if (str_ReceptorUbigeo.Trim() != string.Empty)
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorUbigeo, str_separador); //E
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //E

                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorDireccion.Replace(",", " "), str_separador); //F

                if (str_ReceptorUbigeo.Trim() != string.Empty)
                {
                    stb_Resultado.AppendFormat("{0}{1}", Str_ReceptorUrbanizacion, str_separador); //G
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorDepartamento, str_separador); //H
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorProvincia, str_separador); //I
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorDistrito, str_separador); //J
                }
                else
                {
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //G
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //H
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //I
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //J
                }
                if (ldc_montoBruto > 0)
                    stb_Resultado.AppendFormat("{0}{1}", "PE", str_separador); //K
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador);

                stb_Resultado.AppendFormat("{0}{1}{2}", str_ReceptorCorreoElectronico, str_separador, Environment.NewLine); //L

                /************ FILA 7 - Leyendas *********************************************************/
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteImporteTotalLetras, str_separador); //A

                if (dec_ComprobanteTotalGratuito > 0 && dec_ComprobanteTotalGravada == 0 && dec_ComprobanteTotalNoGravada == 0 && dec_ComprobanteTotalExonerada == 0)
                    stb_Resultado.AppendFormat("{0}{1}", "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE", str_separador); //B
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //B

                stb_Resultado.AppendFormat("{0}{1}", ",,,,,", str_separador); //C

                if (dec_MontoDetraccion > 0)
                    stb_Resultado.AppendFormat("{0}{1}", "OperaciÃ³n sujeta a detracciÃ³n", str_separador); //B
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //C

                stb_Resultado.AppendFormat("{0}", Environment.NewLine); //ULTIMO

                /************ FILA 8 - Adicionales Globales *********************************************/
                if (str_ComprobanteTipoComprobante == "01" || str_ComprobanteTipoComprobante == "03")
                {
                    if (dec_MontoDetraccion > 0)
                        stb_Resultado.AppendFormat("{0}{1}{2}", str_ComprobanteGlosa + "," + str_NumeroOrdenCompra + "," + str_fechaVencimiento + ",,,,,,,,," + str_FormaPagoCondicion + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,001", str_separador, Environment.NewLine); //BH
                    else
                        stb_Resultado.AppendFormat("{0}{1}{2}", str_ComprobanteGlosa.Trim() + "," + str_NumeroOrdenCompra + "," + str_fechaVencimiento + ",,,,,,,,," + str_FormaPagoCondicion + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", str_separador, Environment.NewLine); //BH

                }
                else
                {
                    stb_Resultado.AppendFormat("{0}{1}{2}", ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", str_separador, Environment.NewLine); //BH
                }
                /************ FILA 9 - Datos de la Línea ************************************************/

                //Datos del detalle
                string[] str_ListaComprobanteDetalleTrama = str_ComprobanteDetalleTrama.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_ListaComprobanteDetalleTrama.Length > 0)
                {
                    for (int int_Fila = 0; int_Fila < str_ListaComprobanteDetalleTrama.Length; int_Fila++)
                    {
                        string[] str_RegistroComprobanteDetalleTrama = str_ListaComprobanteDetalleTrama[int_Fila].Split(Convert.ToChar("|"));

                        stb_Resultado.AppendFormat("{0}{1}", (int_Fila + 1).ToString(), str_separador); //A
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[4], str_separador); //B
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[3], str_separador); //C
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[2].Replace(",", string.Empty).Replace("\"", string.Empty), str_separador); //D
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[7], str_separador); //E
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[8], str_separador); //F

                        if (str_RegistroComprobanteDetalleTrama[13] == "13" || str_RegistroComprobanteDetalleTrama[13] == "21" || str_RegistroComprobanteDetalleTrama[13] == "32")
                        {
                            stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[15], str_separador); //G
                            stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[14], str_separador); //H
                        }
                        else
                        {
                            stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //G
                            stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //H
                        }

                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[5], str_separador); //I
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //J
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[13], str_separador); //K

                        switch (str_RegistroComprobanteDetalleTrama[13])
                        {
                            case "10":
                                stb_Resultado.AppendFormat("{0}{1}", "1000", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[10], str_separador); //M
                                break;
                            case "13":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[10], str_separador); //M
                                break;
                            case "15":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[10], str_separador); //M
                                break;
                            case "30":
                                stb_Resultado.AppendFormat("{0}{1}", "9998", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "32":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "20":
                                stb_Resultado.AppendFormat("{0}{1}", "9997", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "21":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "40":
                                stb_Resultado.AppendFormat("{0}{1}", "9995", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                        }

                        //Montos ISC
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //N
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //O
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //P
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Q

                        //Cod Producto Sunat
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[1], str_separador); //R

                        //Cod Interno Spring
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[0].Trim(), str_separador); //S

                        //Valor Unitario
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[6], str_separador); //T

                        //Valor Venta
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[5], str_separador); //U

                        //Otros Tributos
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //V
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //W
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //X

                        //Descuento
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Y
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Z
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AA
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AB
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AC
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AD

                        //Cargo
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AE
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AF
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AG
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AH
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AI
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AJ

                        switch (str_RegistroComprobanteDetalleTrama[13])
                        {
                            case "10":
                                //Monto Impuesto
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AK
                                //Total Linea
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[11], str_separador, Environment.NewLine); //AL
                                break;
                            case "13":
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //AK
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AL
                                break;
                            case "15":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AK
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AL
                                break;
                            case "30":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AK
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AL
                                break;
                            case "32":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AK
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AL
                                break;
                            case "20":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AK
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AL
                                break;
                            case "21":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AK
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AL
                                break;
                            case "40":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AK
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AL
                                break;
                        }
                    }
                }


                /************ FILA 10 - Separador Final *************************************************/
                stb_Resultado.AppendFormat("{0}{1}{2}", "FF00FF", str_separador, Environment.NewLine); //BH


                stb_Resultado.ToString();
                //Solo para ambiente de pruebas                
                if (str_UbicacionXML.Trim() != string.Empty)
                {
                    
                    //sw_Documento = new StreamWriter(str_UbicacionXML + str_EmpresaNumeroDocumento + "-" + str_ComprobanteTipoComprobante + "-" + str_ComprobanteSerie + "-" + str_ComprobanteNumero + ".csv");
                    using (StreamWriter sw_Documento = new StreamWriter(str_UbicacionXML + str_EmpresaNumeroDocumento + "-" + str_ComprobanteTipoComprobante + "-" + str_ComprobanteSerie + "-" + str_ComprobanteNumero + ".csv"))
                    {
                        sw_Documento.Write(stb_Resultado.ToString());
                        sw_Documento.Flush();
                        sw_Documento.Close();
                        sw_Documento.Dispose();
                    }

                    //    sw_Documento.Write(stb_Resultado.ToString());
                    //sw_Documento.Flush();
                    //sw_Documento.Close();
                }

                using (var obj_Cliente = new HttpClient())
                {
                    string str_ServicioUsuarioClave = "client" + ":" + "secret";
                    byte[] byt_UsuarioClave = Encoding.UTF8.GetBytes(str_ServicioUsuarioClave.ToCharArray());

                    obj_Cliente.DefaultRequestHeaders.Accept.Clear();
                    obj_Cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    obj_Cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byt_UsuarioClave));

                    var obj_Parametros = new Dictionary<string, string>();
                    obj_Parametros.Add("grant_type", "password");
                    obj_Parametros.Add("username", str_ServicioUsuario);
                    obj_Parametros.Add("password", str_ServicioClave);

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    var obj_Response = obj_Cliente.PostAsync(str_URL + "/oauth/token", new FormUrlEncodedContent(obj_Parametros)).Result;

                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;
                    var obj_Resultado = JObject.Parse(str_Resultado);

                    if (obj_Resultado["error"] != null)
                    {
                        str_Resultado = "01|EX|" + obj_Resultado["error"].ToString() + "-" + obj_Resultado["error_description"].ToString();
                        return str_Resultado;
                    }

                    string str_Token = obj_Resultado["access_token"].ToString();

                    obj_Cliente.DefaultRequestHeaders.Clear();
                    var frm_DatosEnviar = new MultipartFormDataContent();

                    Stream Stream = File.OpenRead(str_UbicacionXML + str_EmpresaNumeroDocumento + "-" + str_ComprobanteTipoComprobante + "-" + str_ComprobanteSerie + "-" + str_ComprobanteNumero + ".csv");

                    var obj_ArchivoCSV = new StreamContent(Stream);
                    obj_ArchivoCSV.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
                    obj_ArchivoCSV.Headers.ContentDisposition.Name = "file";
                    obj_ArchivoCSV.Headers.ContentDisposition.FileName = str_EmpresaNumeroDocumento + "-" + str_ComprobanteTipoComprobante + "-" + str_ComprobanteSerie + "-" + str_ComprobanteNumero + ".csv";

                    frm_DatosEnviar.Add(obj_ArchivoCSV);

                    obj_Cliente.DefaultRequestHeaders.Add("Authorization", "bearer " + str_Token);

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    obj_Response = obj_Cliente.PostAsync(str_URL + "/v1/document", frm_DatosEnviar).Result;
                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;

                    obj_Resultado = JObject.Parse(str_Resultado);

                    if (obj_Resultado["code"] != null && obj_Resultado["code"].ToString() == "0")
                    {
                        str_Token = obj_Resultado["description"].ToString();
                        str_Resultado = "00|EN|Codigo Hash generado:" + str_Token + "| ";
                    }
                    else if (obj_Resultado["description"] != null)
                    {
                        str_Resultado = "01|PE|Error en Registro:" + obj_Resultado["description"].ToString() + "| ";
                    }
                    else
                    {
                        str_Resultado = "01|PE|Error en Registro:" + obj_Resultado["error"].ToString();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                str_Resultado = "01|EX|" + ex.Message.ToString();
            }
            catch (WebException ex)
            {
                str_Resultado = "01|EX|" + ex.Message.ToString();
            }
            catch (Exception ex)
            {
                str_Resultado = "01|EX|" + ex.Message.ToString();
            }
            finally
            {
                //if (sw_Documento != null)
                //    sw_Documento.Dispose();
                stb_Resultado = null;
                //sw_Documento = null;
                obj_XML = null;
            }
            return str_Resultado;
        }

        public string RegistrarNotas(
    string str_ComprobanteSerie, string str_ComprobanteNumero, DateTime dtm_ComprobanteFechaEmision,
    string str_ComprobanteHoraEmision, string str_ComprobanteTipoComprobante, string str_ComprobanteTipoOperacion,
    string str_ComprobanteTipoMoneda, string str_ComprobanteGlosa,
    decimal dec_ComprobanteTotalGravada, decimal dec_ComprobanteTotalNoGravada, decimal dec_ComprobanteTotalExonerada,
    decimal dec_ComprobanteTotalIGV, decimal dec_ComprobanteTotalGratuito,
    decimal dec_ComprobanteValorVenta, decimal dec_ComprobanteMontoBaseDescuentoGlobal, decimal dec_ComprobantePorcentajeDescuentoGlobal,
    decimal dec_ComprobanteTotalDescuentoGlobal, decimal dec_TotalDocumentoAnticipo, decimal dec_MontoBaseDsctoGlobalAnticipo,
    decimal dec_PorcentajeDsctoGlobalAnticipo, decimal dec_TotalDsctoGlobalesAnticipo, decimal dec_ComprobanteImporteTotal,
    string str_ComprobanteImporteTotalLetras, string str_EmpresaDepartamento, string str_EmpresaProvincia,
    string str_EmpresaDistrito, string str_EmpresaUbigeo, string str_EmpresaDireccion, string str_EmpresaUrbanizacion,
    string str_EmpresaRazonSocial, string str_EmpresaNombreComercial, string str_EmpresaCorreoElectronico,
    string str_EmpresaEstablecimientoSunat, string str_EmpresaTipoDocumento, string str_EmpresaNumeroDocumento,
    string str_FormaPagoCondicion, string str_ReceptorTipoDocumento, string str_ReceptorNumeroDocumento,
    string str_ReceptorRazonSocial, string str_ReceptorCorreoElectronico, string str_ReceptorDireccion,
    string str_ReceptorUbigeo, string Str_ReceptorUrbanizacion, string str_ReceptorProvincia,
    string str_ReceptorDepartamento, string str_ReceptorDistrito, string str_ReceptorPais,
    decimal dec_MontoDetraccion, decimal dec_DetraccionPorcentaje, string str_DetraccionNumeroCuenta,
    decimal dec_DetraccionValorReferencial, string str_DetraccionBienServicio, string str_DetraccionDescripcion,
    string str_VendedorNombre, string str_NumeroOrdenCompra,
    string str_NumeroPedido, string str_ComprobanteDetalleTrama, string str_ComprobantePrePagoTrama,
    string str_ComprobanteNotaDocRefTrama, string str_ComprobanteMotivoDocumento, string str_UbicacionXML,
    string str_URL, string str_TipoFormaPago, string str_GlosaFormaPago, decimal dec_MontoPendientePago,
    decimal dec_totalTributosOpeGratuitas, string str_regimenPercepcion, decimal dec_baseImponiblePercepcion,
    decimal dec_porcentajePercepcion, decimal dec_totalPercepcion, decimal dec_totalVentaConPercepcion, string str_sucursal,
    decimal dec_montoBaseICBPER, decimal dec_totalMontoICBPER, string str_lugarDespacho, decimal dec_totalPrecioVenta,
    int cantidadLineas, string str_ServicioUsuario, string str_ServicioClave, decimal dec_MontoRecargo, decimal dec_MontoTotalExportacion,
    int int_cantidadGuias, string str_guias, string str_fechaVencimiento, decimal ldc_montoOtroExportacion, decimal ldc_montoBruto,
    decimal ldc_montoFlete, decimal ldc_montoSeguro, decimal ldc_montoGastosDestino
    )
        {
            string str_TipoDocumentoReferenciaPrincipal = string.Empty;
            StringBuilder stb_Resultado = new StringBuilder();
            string str_ResultadoComprobante = string.Empty;
            XmlDocument obj_XML = new XmlDocument();
            string str_MensajeSunat = string.Empty;
            string str_ResultadoPDF = string.Empty;
            string str_EstadoSunat = string.Empty;
            string str_CodigoHash = string.Empty;
            string str_RequestUrl = string.Empty;
            string str_EnlacePDF = string.Empty;
            StreamWriter sw_Documento = null;
            string str_Resultado = string.Empty;
            string str_MotivoNC = string.Empty;
            string str_separador = ",";
            string str_vacio = "";

            try
            {
                /************ FILA 1 - Datos del Documento *************/

                stb_Resultado.AppendFormat("{0}{1}", dtm_ComprobanteFechaEmision.ToString("yyyy-MM-dd"), str_separador); //A
                stb_Resultado.AppendFormat("{0}-{1}{2}", str_ComprobanteSerie, str_ComprobanteNumero, str_separador); //B                
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteTipoMoneda, str_separador); //C
                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalGravada.ToString("F2"), str_separador); //D


                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalIGV.ToString("F2"), str_separador); //E
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteTipoMoneda, str_separador); //F

                //Datos ISC
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //G
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //H
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //I

                //Sumatoria Otros Tributos
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //J
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //K
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //L

                //Importe Total Comprobante
                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteImporteTotal.ToString("F2"), str_separador); //M

                //Otros Cargos
                if (ldc_montoOtroExportacion > 0)
                    stb_Resultado.AppendFormat("{0}{1}", ldc_montoOtroExportacion.ToString("F2"), str_separador); //N
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //N  

                //Total operaciones exportación
                if (dec_MontoTotalExportacion > 0)
                    stb_Resultado.AppendFormat("{0}{1}", ldc_montoBruto.ToString("F2"), str_separador); //O
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //O

                //Total operaciones gravadas IGV o IVAP               
                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalGravada.ToString("F2"), str_separador); //P


                //Monto Anticipo Inafecto
                if (dec_ComprobanteTotalNoGravada > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalNoGravada.ToString("F2"), str_separador); //Q
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Q

                //Monto Anticipo Exonerado
                if (dec_ComprobanteTotalExonerada > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalExonerada.ToString("F2"), str_separador); //R
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //R


                //Total operaciones Gratuitas
                if (dec_ComprobanteTotalGratuito > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalGratuito.ToString("F2"), str_separador); //S
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //S


                //Sumatoria de Impuestos Operaciones Gratuitas
                if (dec_totalTributosOpeGratuitas > 0 || dec_ComprobanteTotalGratuito > 0)
                    stb_Resultado.AppendFormat("{0}{1}", dec_totalTributosOpeGratuitas.ToString("F2"), str_separador); //T
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //T

                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //U
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //V
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //X
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Y
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Z
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AA
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AB
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AC
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AD
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AE
                stb_Resultado.AppendFormat("{0}{1}", cantidadLineas.ToString(), str_separador); //AE
                stb_Resultado.AppendFormat("{0}{1}", "1", str_separador); //AF Cantidad documentos asociados
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AG Cantidad guías asociadas y otros documentos asociados
                stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AH Monto para Redondeo

                //Monto Total Impuesto
                stb_Resultado.AppendFormat("{0}{1}", dec_ComprobanteTotalIGV.ToString("F2"), str_separador); //AI

                //Total Valor de Venta 
                stb_Resultado.AppendFormat("{0}{1}{2}", (dec_ComprobanteTotalGravada + dec_ComprobanteTotalNoGravada +
                                                      dec_ComprobanteTotalExonerada + ldc_montoBruto + dec_ComprobanteTotalDescuentoGlobal +
                                                      dec_TotalDsctoGlobalesAnticipo).ToString("F2"), str_separador, Environment.NewLine); //AJ


                /************ FILA 2 - FILA 2 - INFORMACION GUIAS Y OTROS DOCUMENTOS RELACIONADOS *******************************/
                stb_Resultado.AppendFormat("{0}{1}{2}", str_vacio, str_separador, Environment.NewLine); //BH


                /************ FILA 3 - Información del Emisor *******************************************/
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaRazonSocial.Replace(",", " "), str_separador); //A

                if (str_EmpresaNombreComercial.Trim() != string.Empty)
                    stb_Resultado.AppendFormat("{0}{1}", str_EmpresaRazonSocial.Replace(",", " "), str_separador); //B
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_EmpresaNombreComercial.Replace(",", " "), str_separador); //B

                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaNumeroDocumento, str_separador); //C
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaUbigeo, str_separador); //D
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaDireccion.Replace(",", " "), str_separador); //E

                if (str_EmpresaUrbanizacion.Trim() != string.Empty)
                    stb_Resultado.AppendFormat("{0}{1}", str_EmpresaUrbanizacion.Replace(",", " "), str_separador); //F
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //F

                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaDepartamento, str_separador); //G
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaProvincia, str_separador); //H
                stb_Resultado.AppendFormat("{0}{1}", str_EmpresaDistrito, str_separador); //I
                stb_Resultado.AppendFormat("{0}{1}", "PE", str_separador); //J
                stb_Resultado.AppendFormat("{0}{1}{2}", str_EmpresaEstablecimientoSunat, str_separador, Environment.NewLine); //K


                /************ FILA 4 - Información del Receptor *****************************************/
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorNumeroDocumento, str_separador); //A
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorTipoDocumento, str_separador); //B
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorRazonSocial.Replace(",", " "), str_separador); //C
                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorRazonSocial.Replace(",", " "), str_separador); //D

                if (str_ReceptorUbigeo.Trim() != string.Empty)
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorUbigeo, str_separador); //E
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //E

                stb_Resultado.AppendFormat("{0}{1}", str_ReceptorDireccion.Replace(",", " "), str_separador); //F

                if (str_ReceptorUbigeo.Trim() != string.Empty)
                {
                    stb_Resultado.AppendFormat("{0}{1}", Str_ReceptorUrbanizacion.Replace(",", " "), str_separador); //G
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorDepartamento, str_separador); //H
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorProvincia, str_separador); //I
                    stb_Resultado.AppendFormat("{0}{1}", str_ReceptorDistrito, str_separador); //J
                }
                else
                {
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //G
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //H
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //I
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //J
                }

                stb_Resultado.AppendFormat("{0}{1}", "PE", str_separador); //K
                stb_Resultado.AppendFormat("{0}{1}{2}", str_ReceptorCorreoElectronico, str_separador, Environment.NewLine); //L

                /************ FILA 5 - Leyendas *********************************************************/
                stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteImporteTotalLetras, str_separador); //A

                if (dec_ComprobanteTotalGratuito > 0 && dec_ComprobanteTotalGravada == 0 && dec_ComprobanteTotalNoGravada == 0 && dec_ComprobanteTotalExonerada == 0)
                    stb_Resultado.AppendFormat("{0}{1}", "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE", str_separador); //B
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //B

                stb_Resultado.AppendFormat("{0}{1}", ",,,,,", str_separador); //C

                if (dec_MontoDetraccion > 0)
                    stb_Resultado.AppendFormat("{0}{1}", "OperaciÃ³n sujeta a detracciÃ³n", str_separador); //B
                else
                    stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //C

                stb_Resultado.AppendFormat("{0}", Environment.NewLine); //ULTIMO

                /************ FILA 6 - ADICIONALES GLOBALES *******************************/
                // stb_Resultado.AppendFormat("{0}{1}{2}", str_ComprobanteGlosa, str_separador, Environment.NewLine); //BH
                stb_Resultado.AppendFormat("{0}{1}{2}", str_ComprobanteGlosa + "," + str_NumeroOrdenCompra + "," + str_fechaVencimiento + ",,,,,,,,," + str_FormaPagoCondicion + ",,,,,,,,,,,,,,,,,,,,,,,,,,,," + ldc_montoFlete.ToString("F2") + "," + ldc_montoSeguro.ToString("F2") + "," + ldc_montoGastosDestino.ToString("F2") + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", str_separador, Environment.NewLine); //BH

                /************ FILA 7 - DATOS DEL DOCUMENTO QUE SE MODIFICA *******************************/
                string[] str_Lista = str_ComprobanteNotaDocRefTrama.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_Lista.Length > 0)
                {
                    for (int int_Fila = 0; int_Fila < str_Lista.Length; int_Fila++)
                    {
                        string[] str_Registro = str_Lista[int_Fila].Split(Convert.ToChar("|"));
                        stb_Resultado.AppendFormat("{0}{1}", str_Registro[0], str_separador); //A
                        stb_Resultado.AppendFormat("{0}{1}", str_Registro[1], str_separador); //B
                        stb_Resultado.AppendFormat("{0}{1}", str_Registro[2], str_separador); //C
                        stb_Resultado.AppendFormat("{0}{1}", str_ComprobanteMotivoDocumento, str_separador); //D
                        stb_Resultado.AppendFormat("{0}{1}", str_Registro[3], str_separador); //E
                        stb_Resultado.AppendFormat("{0}{1}{2}", "RELATED_DOC", str_separador, Environment.NewLine); //F
                    }
                }
                /************ FILA 8 - Datos de la Línea ************************************************/

                //Datos del detalle
                string[] str_ListaComprobanteDetalleTrama = str_ComprobanteDetalleTrama.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_ListaComprobanteDetalleTrama.Length > 0)
                {
                    for (int int_Fila = 0; int_Fila < str_ListaComprobanteDetalleTrama.Length; int_Fila++)
                    {
                        string[] str_RegistroComprobanteDetalleTrama = str_ListaComprobanteDetalleTrama[int_Fila].Split(Convert.ToChar("|"));

                        stb_Resultado.AppendFormat("{0}{1}", (int_Fila + 1).ToString(), str_separador); //A
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[4], str_separador); //B
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[3], str_separador); //C
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[2].Replace(",", string.Empty).Replace("\"", string.Empty), str_separador); //D
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[7], str_separador); //E
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[8], str_separador); //F

                        if (str_RegistroComprobanteDetalleTrama[13] == "13" || str_RegistroComprobanteDetalleTrama[13] == "21" || str_RegistroComprobanteDetalleTrama[13] == "32")
                        {
                            stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[15], str_separador); //G
                            stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[14], str_separador); //H
                        }
                        else
                        {
                            stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //G
                            stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //H
                        }

                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[5], str_separador); //I
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //J
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[13], str_separador); //K

                        switch (str_RegistroComprobanteDetalleTrama[13])
                        {
                            case "10":
                                stb_Resultado.AppendFormat("{0}{1}", "1000", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[10], str_separador); //M
                                break;
                            case "13":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[10], str_separador); //M
                                break;
                            case "15":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[10], str_separador); //M
                                break;
                            case "30":
                                stb_Resultado.AppendFormat("{0}{1}", "9998", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "32":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "20":
                                stb_Resultado.AppendFormat("{0}{1}", "9997", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "21":
                                stb_Resultado.AppendFormat("{0}{1}", "9996", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                            case "40":
                                stb_Resultado.AppendFormat("{0}{1}", "9995", str_separador); //L
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //M
                                break;
                        }

                        //Montos ISC
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //N
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //O
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //P
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Q

                        //Cod Producto Sunat
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[1], str_separador); //R

                        //Cod Interno Spring
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[0].Trim(), str_separador); //S

                        //Valor Unitario
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[6], str_separador); //T

                        //Valor Venta
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[5], str_separador); //U

                        //Otros Tributos
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //V
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //W
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //X

                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Y
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //Z
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AA
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AB
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AC
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AD
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AE
                        stb_Resultado.AppendFormat("{0}{1}", str_vacio, str_separador); //AF

                        switch (str_RegistroComprobanteDetalleTrama[13])
                        {
                            case "10":
                                //Monto Impuesto
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AG
                                //Total Linea
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[11], str_separador, Environment.NewLine); //AH
                                break;
                            case "13":
                                stb_Resultado.AppendFormat("{0}{1}", "0.00", str_separador); //AG
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AH
                                break;
                            case "15":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AG
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AH
                                break;
                            case "30":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AG
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AH
                                break;
                            case "32":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AG
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AH
                                break;
                            case "20":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AG
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AH
                                break;
                            case "21":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AG
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AH
                                break;
                            case "40":
                                stb_Resultado.AppendFormat("{0}{1}", str_RegistroComprobanteDetalleTrama[12], str_separador); //AG
                                stb_Resultado.AppendFormat("{0}{1}{2}", str_RegistroComprobanteDetalleTrama[5], str_separador, Environment.NewLine); //AH
                                break;
                        }
                    }
                }


                /************ FILA 10 - Separador Final *************************************************/
                stb_Resultado.AppendFormat("{0}{1}{2}", "FF00FF", str_separador, Environment.NewLine); //BH


                stb_Resultado.ToString();
                //Solo para ambiente de pruebas                
                if (str_UbicacionXML.Trim() != string.Empty)
                {
                    sw_Documento = new StreamWriter(str_UbicacionXML + str_EmpresaNumeroDocumento + "-" + str_ComprobanteTipoComprobante + "-" + str_ComprobanteSerie + "-" + str_ComprobanteNumero + ".csv");
                    sw_Documento.Write(stb_Resultado.ToString());
                    sw_Documento.Flush();
                    sw_Documento.Close();
                }

                using (var obj_Cliente = new HttpClient())
                {
                    string str_ServicioUsuarioClave = "client" + ":" + "secret";
                    byte[] byt_UsuarioClave = Encoding.UTF8.GetBytes(str_ServicioUsuarioClave.ToCharArray());

                    obj_Cliente.DefaultRequestHeaders.Accept.Clear();
                    obj_Cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    obj_Cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byt_UsuarioClave));

                    var obj_Parametros = new Dictionary<string, string>();
                    obj_Parametros.Add("grant_type", "password");
                    obj_Parametros.Add("username", str_ServicioUsuario);
                    obj_Parametros.Add("password", str_ServicioClave);

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    var obj_Response = obj_Cliente.PostAsync(str_URL + "/oauth/token", new FormUrlEncodedContent(obj_Parametros)).Result;

                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;
                    var obj_Resultado = JObject.Parse(str_Resultado);

                    if (obj_Resultado["error"] != null)
                    {
                        str_Resultado = "01|EX|" + obj_Resultado["error"].ToString() + "-" + obj_Resultado["error_description"].ToString();
                        return str_Resultado;
                    }

                    string str_Token = obj_Resultado["access_token"].ToString();

                    obj_Cliente.DefaultRequestHeaders.Clear();
                    var frm_DatosEnviar = new MultipartFormDataContent();

                    Stream Stream = File.OpenRead(str_UbicacionXML + str_EmpresaNumeroDocumento + "-" + str_ComprobanteTipoComprobante + "-" + str_ComprobanteSerie + "-" + str_ComprobanteNumero + ".csv");

                    var obj_ArchivoCSV = new StreamContent(Stream);
                    obj_ArchivoCSV.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
                    obj_ArchivoCSV.Headers.ContentDisposition.Name = "file";
                    obj_ArchivoCSV.Headers.ContentDisposition.FileName = str_EmpresaNumeroDocumento + "-" + str_ComprobanteTipoComprobante + "-" + str_ComprobanteSerie + "-" + str_ComprobanteNumero + ".csv";

                    frm_DatosEnviar.Add(obj_ArchivoCSV);

                    obj_Cliente.DefaultRequestHeaders.Add("Authorization", "bearer " + str_Token);

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    obj_Response = obj_Cliente.PostAsync(str_URL + "/v1/document", frm_DatosEnviar).Result;
                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;

                    obj_Resultado = JObject.Parse(str_Resultado);

                    if (obj_Resultado["code"] != null && obj_Resultado["code"].ToString() == "0")
                    {
                        str_Token = obj_Resultado["description"].ToString();
                        str_Resultado = "00|EN|Codigo Hash generado:" + str_Token + "| ";
                    }
                    else if (obj_Resultado["description"] != null)
                    {
                        str_Resultado = "01|PE|Error en Registro:" + obj_Resultado["description"].ToString() + "| ";
                    }
                    else
                    {
                        str_Resultado = "01|PE|Error en Registro:" + obj_Resultado["error"].ToString();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                str_Resultado = "01|EX|" + ex.Message.ToString();
            }
            catch (WebException ex)
            {
                str_Resultado = "01|EX|" + ex.Message.ToString();
            }
            catch (Exception ex)
            {
                str_Resultado = "01|EX|" + ex.Message.ToString();
            }
            finally
            {
                if (sw_Documento != null)
                    sw_Documento.Dispose();
                stb_Resultado = null;
                sw_Documento = null;
                obj_XML = null;
            }
            return str_Resultado;
        }

        
    }
}
