using COBE;
using Serilog;
using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Policy;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace ServicioRSNetCore.Controllers.Funciones
{
    public class RegistroBizlink
    {
        public string Registro(ComprobanteElectronico comprobante)
        {
            string str_Resultado = string.Empty;
            StringBuilder stb_Resultado = new StringBuilder();
            string str_TipoDocumentoReferenciaPrincipal = string.Empty;
            string str_MotivoNC = string.Empty;
            long ll_inicio = 1;
            int ldc_Viaje;
            long ll_lineasdescripcion = 1;
            string str_linea01 = string.Empty;
            string str_linea02 = string.Empty;
            string str_linea03 = string.Empty;
            string str_linea04 = string.Empty;
            string str_linea05 = string.Empty;
            string str_linea06 = string.Empty;
            string str_linea07 = string.Empty;
            string str_linea08 = string.Empty;
            string str_linea09 = string.Empty;
            string str_linea10 = string.Empty;
            string str_URL = string.Empty;
            try
            {
                //stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://ws.ce.ebiz.com/\">");
                stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://pse.bizlinks.com/\">");
                stb_Resultado.AppendFormat("<soapenv:Header/>");
                stb_Resultado.AppendFormat("<soapenv:Body>");
                stb_Resultado.AppendFormat("<ws:invoke>");
                stb_Resultado.AppendFormat("<command><![CDATA[<SignOnLineCmd declare-direct-sunat=\"1\" output=\"PDF\">");
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"idEmisor\"/>", comprobante.EmpresaRuc.Trim());
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"tipoDocumento\"/>", comprobante.TipoComprobante.Trim());
                stb_Resultado.Append("<documento>");

                stb_Resultado.AppendFormat("<version>{0}</version>", "2.0");
                stb_Resultado.AppendFormat("<versionUBL>{0}</versionUBL>", "2.1");
                stb_Resultado.AppendFormat("<fechaEmision>{0}</fechaEmision>", comprobante.FechaEmision.ToString("yyyy-MM-dd"));
                stb_Resultado.AppendFormat("<horaEmision>{0}</horaEmision>", comprobante.FechaEmision.ToString("hh:MM:ss"));
                stb_Resultado.AppendFormat("<serieNumero>{0}</serieNumero>", comprobante.Serie + "-" + comprobante.Numero);
                //stb_Resultado.AppendFormat("<serieNumero>{0}</serieNumero>", "F301-00000054"); // para pruebas
                stb_Resultado.AppendFormat("<tipoDocumento>{0}</tipoDocumento>", comprobante.TipoComprobante);
                stb_Resultado.AppendFormat("<tipoOperacion>{0}</tipoOperacion>", comprobante.TipoOperacion);
                stb_Resultado.AppendFormat("<coTipoEmision>{0}</coTipoEmision>", "RE");
                stb_Resultado.AppendFormat("<tipoMoneda>{0}</tipoMoneda>", comprobante.TipoMoneda);

                //ISC
                if (comprobante.TotalISC > 0)
                {
                    stb_Resultado.AppendFormat("<montoBaseIsc>{0}</montoBaseIsc>", comprobante.MontoBaseISC.ToString("F2"));
                    stb_Resultado.AppendFormat("<totalIsc>{0}</totalIsc>", comprobante.TotalISC);
                }
                if (comprobante.MontoAdicionalObligMonto1 > 0)
                {
                    stb_Resultado.AppendFormat("<totalValorVentaNetoOpGravadas>{0}</totalValorVentaNetoOpGravadas>", comprobante.MontoAdicionalObligMonto1.ToString("F"));
                }
                if (comprobante.MontoAdicionalObligMonto2 > 0)
                {
                    stb_Resultado.AppendFormat("<totalValorVentaNetoOpNoGravada>{0}</totalValorVentaNetoOpNoGravada>", comprobante.MontoAdicionalObligMonto2.ToString("F"));
                }
                if (comprobante.MontoAdicionalObligMonto3 > 0)
                {
                    stb_Resultado.AppendFormat("<totalValorVentaNetoOpExoneradas>{0}</totalValorVentaNetoOpExoneradas>", comprobante.MontoAdicionalObligMonto3.ToString("F"));
                }


                if (comprobante.TotalGratuito > 0)
                    stb_Resultado.AppendFormat("<totalValorVentaNetoOpGratuitas>{0}</totalValorVentaNetoOpGratuitas>", comprobante.TotalGratuito.ToString("F"));

                //Información del Anticipo -- preguntar
                //if (dec_TotalDocumentoAnticipo > 0)
                //    stb_Resultado.AppendFormat("<totalDocumentoAnticipo>{0}</totalDocumentoAnticipo>", dec_TotalDocumentoAnticipo.ToString("F"));
                //if (dec_MontoBaseDsctoGlobalAnticipo > 0)
                //    stb_Resultado.AppendFormat("<montoBaseDsctoGlobalAnticipo>{0}</montoBaseDsctoGlobalAnticipo>", dec_MontoBaseDsctoGlobalAnticipo.ToString("F"));
                //if (dec_PorcentajeDsctoGlobalAnticipo > 0)
                //    stb_Resultado.AppendFormat("<porcentajeDsctoGlobalAnticipo>{0}</porcentajeDsctoGlobalAnticipo>", dec_PorcentajeDsctoGlobalAnticipo);
                //if (dec_TotalDsctoGlobalesAnticipo > 0)
                //    stb_Resultado.AppendFormat("<totalDsctoGlobalesAnticipo>{0}</totalDsctoGlobalesAnticipo>", dec_TotalDsctoGlobalesAnticipo.ToString("F"));

                //stb_Resultado.AppendFormat("<totalImpuestos>{0}</totalImpuestos>", comprobante.TotalImpuestoGratuito.ToString("F"));
                stb_Resultado.AppendFormat("<totalImpuestos>{0}</totalImpuestos>", (comprobante.MontoImpuestoVentas + comprobante.TotalISC).ToString("F"));
                stb_Resultado.AppendFormat("<totalIgv>{0}</totalIgv>", comprobante.MontoImpuestoVentas.ToString("F"));
                stb_Resultado.AppendFormat("<totalVenta>{0}</totalVenta>", comprobante.ImporteTotal.ToString("F"));
                stb_Resultado.AppendFormat("<codigoLeyenda_1>{0}</codigoLeyenda_1>", 1000);
                stb_Resultado.AppendFormat("<textoLeyenda_1>{0}</textoLeyenda_1>", comprobante.ImporteTotalLetras.Trim());

                // Información del Emisor
                stb_Resultado.AppendFormat("<paisEmisor>{0}</paisEmisor>", "PE");
                if (comprobante.EmpresaDepartamento != string.Empty)
                    stb_Resultado.AppendFormat("<departamentoEmisor>{0}</departamentoEmisor>", comprobante.EmpresaDepartamento);
                if (comprobante.EmpresaProvincia != string.Empty)
                    stb_Resultado.AppendFormat("<provinciaEmisor>{0}</provinciaEmisor>", comprobante.EmpresaProvincia);
                if (comprobante.EmpresaDistrito != string.Empty)
                    stb_Resultado.AppendFormat("<distritoEmisor>{0}</distritoEmisor>", comprobante.EmpresaDistrito);
                //if (comprobante.EmpresaUbigeo != string.Empty) // existe punto llegada o punto partida
                //    stb_Resultado.AppendFormat("<ubigeoEmisor>{0}</ubigeoEmisor>", str_EmpresaUbigeo);

                stb_Resultado.AppendFormat("<direccionEmisor>{0}</direccionEmisor>", CambiarCaracterEspecial(comprobante.EmpresaCalle));

                if ((comprobante.EmpresaUrbanizacion ?? string.Empty).Trim() != string.Empty)
                    stb_Resultado.AppendFormat("<urbanizacion>{0}</urbanizacion>", CambiarCaracterEspecial(comprobante.EmpresaUrbanizacion.Trim()));

                stb_Resultado.AppendFormat("<razonSocialEmisor>{0}</razonSocialEmisor>", CambiarCaracterEspecial(comprobante.EmpresaRazonSocial));
                stb_Resultado.AppendFormat("<nombreComercialEmisor>{0}</nombreComercialEmisor>", CambiarCaracterEspecial(comprobante.EmpresaNombreComercial));
                if (string.IsNullOrEmpty(comprobante.EmpresaCorreo) == false)
                    stb_Resultado.AppendFormat("<correoEmisor>{0}</correoEmisor>", "-"); //comprobante.EmpresaCorreo

                stb_Resultado.AppendFormat("<codigoLocalAnexoEmisor>{0}</codigoLocalAnexoEmisor>", comprobante.EmpresaCodigoEstablecimientoSunat);
                stb_Resultado.AppendFormat("<tipoDocumentoEmisor>{0}</tipoDocumentoEmisor>", comprobante.EmpresaCodigoTipoDocumento);
                stb_Resultado.AppendFormat("<numeroDocumentoEmisor>{0}</numeroDocumentoEmisor>", comprobante.EmpresaRuc);

                // Información del Receptor
                stb_Resultado.AppendFormat("<tipoDocumentoAdquiriente>{0}</tipoDocumentoAdquiriente>", comprobante.TipoDocumentoIdentidad.Trim());
                stb_Resultado.AppendFormat("<numeroDocumentoAdquiriente>{0}</numeroDocumentoAdquiriente>", comprobante.Ruc.Trim());
                stb_Resultado.AppendFormat("<razonSocialAdquiriente>{0}</razonSocialAdquiriente>", CambiarCaracterEspecial(comprobante.RazonSocial.Trim()));
                stb_Resultado.AppendFormat("<correoAdquiriente>{0}</correoAdquiriente>", comprobante.CorreoElectronico);
                if (comprobante.Serie.Substring(0, 1) == "F")
                    stb_Resultado.AppendFormat("<direccionAdquiriente>{0}</direccionAdquiriente>", CambiarCaracterEspecial(comprobante.ClienteDireccion));
                else
                    stb_Resultado.AppendFormat("<lugarDestino>{0}</lugarDestino>", CambiarCaracterEspecial(comprobante.ClienteDireccion));

                //if (str_ReceptorUbigeo != String.Empty)
                //    stb_Resultado.AppendFormat("<ubigeoAdquiriente>{0}</ubigeoAdquiriente>", str_ReceptorUbigeo);

                if (comprobante.ReceptorUrbanizacion?.Trim() != String.Empty)
                    stb_Resultado.AppendFormat("<urbanizacionAdquiriente>{0}</urbanizacionAdquiriente>", comprobante.ReceptorUrbanizacion.Trim());

                if (comprobante.ReceptorProvincia != String.Empty)
                    stb_Resultado.AppendFormat("<provinciaAdquiriente>{0}</provinciaAdquiriente>", comprobante.ReceptorProvincia);

                if (comprobante.ReceptorDepartamento != String.Empty)
                    stb_Resultado.AppendFormat("<departamentoAdquiriente>{0}</departamentoAdquiriente>", comprobante.ReceptorDepartamento);

                if (comprobante.ReceptorDistrito != String.Empty)
                    stb_Resultado.AppendFormat("<distritoAdquiriente>{0}</distritoAdquiriente>", comprobante.ReceptorDistrito);

                if (comprobante.ReceptorCodigoPais != String.Empty)
                    stb_Resultado.AppendFormat("<paisAdquiriente>{0}</paisAdquiriente>", "PE");//comprobante.ReceptorCodigoPais) -- revisar

                //Solo para facturas y notas de crédito - Orden de Compra   
                if ((comprobante.TipoComprobante == "01" || comprobante.TipoComprobante == "07") && comprobante.NumeroOrdenCompra.Trim() != String.Empty)
                {
                    if (comprobante.NumeroOrdenCompra.Trim() != string.Empty)
                        stb_Resultado.AppendFormat("<ordenCompra>{0}</ordenCompra>", comprobante.NumeroOrdenCompra.Trim());
                }

                string[] str_RegistroComprobanteNotaDocRefTrama = comprobante.NotaDocRefTrama.Split(new String[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_RegistroComprobanteNotaDocRefTrama.Length > 0)
                {
                    str_TipoDocumentoReferenciaPrincipal = str_RegistroComprobanteNotaDocRefTrama[2];
                    str_MotivoNC = str_RegistroComprobanteNotaDocRefTrama[1];

                    if (comprobante.TipoComprobante == "08" && str_MotivoNC == "03")
                    {
                        stb_Resultado.AppendFormat("<codigoSerieNumeroAfectado>{0}</codigoSerieNumeroAfectado>", str_RegistroComprobanteNotaDocRefTrama[1]);
                        stb_Resultado.AppendFormat("<motivoDocumento>{0}</motivoDocumento>", comprobante.MotivoDocumento);
                    }
                    else
                    {
                        stb_Resultado.AppendFormat("<serieNumeroAfectado>{0}</serieNumeroAfectado>", str_RegistroComprobanteNotaDocRefTrama[0] + "-" + Convert.ToInt32(str_RegistroComprobanteNotaDocRefTrama[1]).ToString("00000000"));
                        stb_Resultado.AppendFormat("<codigoSerieNumeroAfectado>{0}</codigoSerieNumeroAfectado>", comprobante.MotivoDocumento);
                        // stb_Resultado.AppendFormat("<motivoDocumento>{0}</motivoDocumento>", comprobante.MotivoSustento);
                        stb_Resultado.AppendFormat("<tipoDocumentoReferenciaPrincipal>{0}</tipoDocumentoReferenciaPrincipal>", str_RegistroComprobanteNotaDocRefTrama[2]);
                        stb_Resultado.AppendFormat("<numeroDocumentoReferenciaPrincipal>{0}</numeroDocumentoReferenciaPrincipal>", str_RegistroComprobanteNotaDocRefTrama[0] + "-" + Convert.ToInt32(str_RegistroComprobanteNotaDocRefTrama[1]).ToString("00000000"));
                        stb_Resultado.AppendFormat("<motivoDocumento>{0}</motivoDocumento>", CambiarCaracterEspecial(comprobante.MotivoSustento));
                        stb_Resultado.AppendFormat("<documentoreferencialadicional1>{0}</documentoreferencialadicional1>", str_RegistroComprobanteNotaDocRefTrama[0] + "-" + Convert.ToInt32(str_RegistroComprobanteNotaDocRefTrama[1]).ToString("00000000"));
                        stb_Resultado.AppendFormat("<tipodocumentoreferencialadicional1>{0}</tipodocumentoreferencialadicional1>", str_RegistroComprobanteNotaDocRefTrama[2]);
                    }
                }

                // Información de Detracción
                if (comprobante.Detraccion > 0)
                {
                    stb_Resultado.AppendFormat("<codigoDetraccion>{0}</codigoDetraccion>", comprobante.DetraccionBienesServicios);
                    stb_Resultado.AppendFormat("<porcentajeDetraccion>{0}</porcentajeDetraccion>", (comprobante.DetraccionPorcentaje / 100).ToString());
                    stb_Resultado.AppendFormat("<totalDetraccion>{0}</totalDetraccion>", comprobante.Detraccion.ToString("F"));
                    stb_Resultado.AppendFormat("<numeroCtaBancoNacion>{0}</numeroCtaBancoNacion>", comprobante.DetraccionNumeroCuenta);
                    stb_Resultado.AppendFormat("<formaPago>{0}</formaPago>", "001");
                    stb_Resultado.AppendFormat("<codigoLeyenda_2>{0}</codigoLeyenda_2>", "2006");
                    stb_Resultado.AppendFormat("<textoLeyenda_2>{0}</textoLeyenda_2>", CambiarCaracterEspecial("Operación sujeta a Detracción"));

                }
                else if (comprobante.ImporteTotal == 0 && comprobante.PrePagoTrama == String.Empty && !(comprobante.TipoComprobante == "07" && str_MotivoNC == "03") && !(comprobante.TipoComprobante == "07" && str_MotivoNC == "13"))
                {
                    stb_Resultado.AppendFormat("<codigoLeyenda_2>{0}</codigoLeyenda_2>", 1002);
                    stb_Resultado.AppendFormat("<textoLeyenda_2>{0}</textoLeyenda_2>", "Transferencia gratuita.");
                }


                if (comprobante.TipoOperacion == "2001" && comprobante.TipoFormaPago == "0")
                {
                    stb_Resultado.AppendFormat("<regimenPercepcion>{0}</regimenPercepcion>", comprobante.PercepcionCodigoRegimen);
                    stb_Resultado.AppendFormat("<baseImponiblePercepcion>{0}</baseImponiblePercepcion>", comprobante.BasePercepcion.ToString("F2"));
                    stb_Resultado.AppendFormat("<porcentajePercepcion>{0}</porcentajePercepcion>", comprobante.PercepcionTasa.ToString("F2"));
                    stb_Resultado.AppendFormat("<totalPercepcion>{0}</totalPercepcion>", comprobante.MontoPercepcion.ToString("F2"));
                    stb_Resultado.AppendFormat("<totalVentaConPercepcion>{0}</totalVentaConPercepcion>", comprobante.MontoTotalPercepcion.ToString("F2"));
                }
                

                if (comprobante.MontoBaseIVAP > 0)
                {
                    stb_Resultado.AppendFormat("<ventaArrozPilado>{0}</ventaArrozPilado>", 1);
                    stb_Resultado.AppendFormat("<codigoLeyenda_2>{0}</codigoLeyenda_2>", 2007);
                    stb_Resultado.AppendFormat("<textoLeyenda_2>{0}</textoLeyenda_2>", "Operación sujeta al IVAP");
                }
                string[] str_RegistroCuentaTrama = comprobante.GrillaCuentaTrama.Split(new String[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_RegistroCuentaTrama.Length > 0)
                {
                    stb_Resultado.AppendFormat("<codigoAuxiliar500_1>{0}</codigoAuxiliar500_1>", 9840);//BCP Soles
                    stb_Resultado.AppendFormat("<textoAuxiliar500_1>{0}</textoAuxiliar500_1>", str_RegistroCuentaTrama[0]);

                    //stb_Resultado.AppendFormat("<codigoAuxiliar500_2>{0}</codigoAuxiliar500_2>", 9838);//BCP Dolares
                    //stb_Resultado.AppendFormat("<textoAuxiliar500_2>{0}</textoAuxiliar500_2>", str_RegistroComprobanteNotaDocRefTrama[0]);

                    //stb_Resultado.AppendFormat("<codigoAuxiliar500_3>{0}</codigoAuxiliar500_3>", 9877);//BCP Soles
                    //stb_Resultado.AppendFormat("<textoAuxiliar500_3>{0}</textoAuxiliar500_3>", str_RegistroComprobanteNotaDocRefTrama[0]);

                    //stb_Resultado.AppendFormat("<codigoAuxiliar500_4>{0}</codigoAuxiliar500_4>", 9875);//Bco. BBVA Dolares
                    //stb_Resultado.AppendFormat("<textoAuxiliar500_4>{0}</textoAuxiliar500_4>", str_RegistroComprobanteNotaDocRefTrama[0]);
                }

                if (!string.IsNullOrWhiteSpace(comprobante.MultiGlosa.Trim()))
                {
                    stb_Resultado.AppendFormat("<codigoAuxiliar500_5>{0}</codigoAuxiliar500_5>", 9998);//Comentarios
                    stb_Resultado.AppendFormat("<textoAuxiliar500_5>{0}</textoAuxiliar500_5>", comprobante.MultiGlosa.Trim());
                }

                if (!string.IsNullOrWhiteSpace(comprobante.VendedorNombre))
                {
                    stb_Resultado.AppendFormat("<codigoAuxiliar500_2>{0}</codigoAuxiliar500_2>", 9426);//Comentarios
                    stb_Resultado.AppendFormat("<textoAuxiliar500_2>{0}</textoAuxiliar500_2>", comprobante.VendedorNombre.Trim());
                }
                stb_Resultado.AppendFormat("<inHabilitado>{0}</inHabilitado>", "1");

                string[] str_ListaComprobanteDetalleTrama = comprobante.DetalleTrama.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_ListaComprobanteDetalleTrama.Length > 0)
                {
                    for (int int_Fila = 0; int_Fila < str_ListaComprobanteDetalleTrama.Length; int_Fila++)
                    {
                        string[] str_RegistroComprobanteDetalleTrama = str_ListaComprobanteDetalleTrama[int_Fila].Split(Convert.ToChar("|"));

                        decimal monto_isc = 0;
                        if (Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[16]) > 0)
                            monto_isc = Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[16]);

                        //armando xml
                        stb_Resultado.AppendFormat("<item>");
                        stb_Resultado.AppendFormat("<numeroOrdenItem>{0}</numeroOrdenItem>", int_Fila + 1);
                        stb_Resultado.AppendFormat("<codigoProducto>{0}</codigoProducto>", str_RegistroComprobanteDetalleTrama[1].Trim());
                        stb_Resultado.AppendFormat("<codigoProductoSUNAT>{0}</codigoProductoSUNAT>", str_RegistroComprobanteDetalleTrama[2].Trim());
                        stb_Resultado.AppendFormat("<descripcion>{0}</descripcion>", CambiarCaracterEspecial(str_RegistroComprobanteDetalleTrama[3]));
                        stb_Resultado.AppendFormat("<cantidad>{0}</cantidad>", str_RegistroComprobanteDetalleTrama[4]);
                        stb_Resultado.AppendFormat("<unidadMedida>{0}</unidadMedida>", str_RegistroComprobanteDetalleTrama[5]);
                        stb_Resultado.AppendFormat("<importeTotalSinImpuesto>{0}</importeTotalSinImpuesto>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[6]).ToString());
                        stb_Resultado.AppendFormat("<importeUnitarioSinImpuesto>{0}</importeUnitarioSinImpuesto>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[7]).ToString());
                        stb_Resultado.AppendFormat("<importeUnitarioConImpuesto>{0}</importeUnitarioConImpuesto>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[8]).ToString());
                        stb_Resultado.AppendFormat("<codigoImporteUnitarioConImpuesto>{0}</codigoImporteUnitarioConImpuesto>", str_RegistroComprobanteDetalleTrama[9]);
                        stb_Resultado.AppendFormat("<montoBaseIgv>{0}</montoBaseIgv>", (Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[10]) + monto_isc).ToString());

                        if (str_RegistroComprobanteDetalleTrama[14] == "20")
                        {
                            stb_Resultado.AppendFormat("<tasaIgv>{0}</tasaIgv>", "0.00");
                            stb_Resultado.AppendFormat("<importeIgv>{0}</importeIgv>", "0.00");
                            stb_Resultado.AppendFormat("<importeTotalImpuestos>{0}</importeTotalImpuestos>", "0.00");
                        }

                        else
                        {
                            stb_Resultado.AppendFormat("<tasaIgv>{0}</tasaIgv>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[11]).ToString());
                            stb_Resultado.AppendFormat("<importeIgv>{0}</importeIgv>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[12]).ToString());
                            stb_Resultado.AppendFormat("<importeTotalImpuestos>{0}</importeTotalImpuestos>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[13]).ToString());
                        }

                        stb_Resultado.AppendFormat("<codigoRazonExoneracion>{0}</codigoRazonExoneracion>", str_RegistroComprobanteDetalleTrama[14]);

                        //ISC
                        if (Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[16]) > 0)
                        {
                            stb_Resultado.AppendFormat("<montoBaseIsc>{0}</montoBaseIsc>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[6]).ToString("F2"));
                            stb_Resultado.AppendFormat("<tipoSistemaImpuestoISC>{0}</tipoSistemaImpuestoISC>", "02");
                            stb_Resultado.AppendFormat("<tasaIsc>{0}</tasaIsc>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[15]).ToString("F2"));
                            stb_Resultado.AppendFormat("<importeIsc>{0}</importeIsc>", Convert.ToDecimal(str_RegistroComprobanteDetalleTrama[16]).ToString());
                            
                            //if (str_RegistroComprobanteDetalleTrama[14].Trim() != string.Empty)
                            //    stb_Resultado.AppendFormat("<codigoImporteReferencial>{0}</codigoImporteReferencial>", str_RegistroComprobanteDetalleTrama[14]);
                            
                        }

                        stb_Resultado.AppendFormat("</item>");
                    }
                }

                if (comprobante.TipoComprobante == "01" || (comprobante.TipoComprobante == "07" && str_MotivoNC == "13"))
                {
                    if (comprobante.TotalGratuito > 0 && comprobante.ImporteTotal == 0)
                    {
                        stb_Resultado.AppendFormat("<formaPagoNegociable>{0}</formaPagoNegociable>", "0");
                        //stb_Resultado.AppendFormat("<totalTributosOpeGratuitas>{0}</totalTributosOpeGratuitas>", dec_totalTributosOpeGratuitas);
                    }
                    else
                    {
                        //stb_Resultado.AppendFormat("<formaPagoNegociable>{0}</formaPagoNegociable>", "0"); // si es 2 necesita mas datos , revisar
                        stb_Resultado.AppendFormat("<formaPagoNegociable>{0}</formaPagoNegociable>", comprobante.TipoFormaPago);
                        if (comprobante.TipoFormaPago == "1")
                        {
                            stb_Resultado.AppendFormat("<montoNetoPendiente>{0}</montoNetoPendiente>", comprobante.MontoPendientePago.ToString("F"));
                            string[] str_ListaGlosaFormaPago = comprobante.GlosaFormaPago.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int int_Fila = 0; int_Fila < str_ListaGlosaFormaPago.Length; int_Fila++)
                            {
                                string[] str_RegistroGlosaFormaPago = str_ListaGlosaFormaPago[int_Fila].Split(Convert.ToChar("|"));
                                //stb_Resultado.AppendFormat("<montoPagoCuota{0}>{1}</montoPagoCuota{2}>", int_Fila + 1, str_RegistroGlosaFormaPago[1], int_Fila + 1);
                                //stb_Resultado.AppendFormat("<fechaPagoCuota{0}>{1}</fechaPagoCuota{2}>", int_Fila + 1, str_RegistroGlosaFormaPago[0], int_Fila + 1);
                                // Nueva estructura jerárquica
                                stb_Resultado.Append("<cuota>");
                                stb_Resultado.AppendFormat("<numeroCuota>{0}</numeroCuota>", int_Fila + 1);
                                stb_Resultado.AppendFormat("<fechaPago>{0}</fechaPago>", str_RegistroGlosaFormaPago[0]); // Fecha
                                stb_Resultado.AppendFormat("<montoPago>{0}</montoPago>", str_RegistroGlosaFormaPago[1]); // Monto
                                stb_Resultado.Append("</cuota>");
                            }
                        }
                    }

                }
                else if (comprobante.TipoComprobante == "03")
                {
                    if (comprobante.TotalGratuito > 0 && comprobante.ImporteTotal == 0)
                    {
                        //stb_Resultado.AppendFormat("<totalTributosOpeGratuitas>{0}</totalTributosOpeGratuitas>", dec_totalTributosOpeGratuitas);
                    }
                }

                //if (dec_totalValorVentaNetoOpExportacion > 0) // TotalExportacion o TotalValorVenta
                //{
                //    stb_Resultado.AppendFormat("<totalValorVentaNetoOpExportacion>{0}</totalValorVentaNetoOpExportacion>", dec_totalValorVentaNetoOpExportacion);
                //}

                stb_Resultado.AppendFormat("<totalValorVenta>{0}</totalValorVenta>", comprobante.TotalValorVenta);
                stb_Resultado.AppendFormat("<totalPrecioVenta>{0}</totalPrecioVenta>", comprobante.TotalPrecioVenta);

                stb_Resultado.Append("</documento>");
                stb_Resultado.Append("</SignOnLineCmd>]]></command>");
                stb_Resultado.Append("</ws:invoke>");
                stb_Resultado.Append("</soapenv:Body>");
                stb_Resultado.Append("</soapenv:Envelope>");

                string str_UbicacionXML = @"C:\TRAMA\EDO.TXT";
                HttpWebRequest obj_Request = default(HttpWebRequest);
                StreamWriter sw_Documento = null;
                XmlNodeList obj_Respuesta = null;
                byte[] obj_FileByte = null;
                str_URL = comprobante.urlWebService;
                string str_WebUsuario = comprobante.URLUsuario;
                string str_WebClave = comprobante.URLPassword;
                //string str_WebUsuario = "20521787547";
                //string str_WebClave = "20521787547";
                XmlDocument obj_XML = new XmlDocument();
                string str_status = string.Empty;
                string str_Documents = string.Empty;


                //Solo para ambiente de pruebas                
                if (str_UbicacionXML.Trim() != string.Empty)
                {
                    sw_Documento = new StreamWriter(str_UbicacionXML + comprobante.EmpresaRuc + "-" + comprobante.TipoComprobante + "-" + comprobante.Serie + "-" + comprobante.Numero + ".xml");
                    sw_Documento.Write(stb_Resultado.ToString());
                    sw_Documento.Flush();
                    sw_Documento.Close();
                }

                //str_URL = "https://psetest.bizlinks.com.pe/ws/invoker";


                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) => true;

                ServicePointManager.SecurityProtocol.ToString();

                CookieContainer myContainer = new CookieContainer();
                obj_Request = (HttpWebRequest)HttpWebRequest.Create(str_URL);

                obj_Request.Method = "POST";
                obj_Request.ContentType = "text/xml;charset=UTF-8";
                //obj_Request.Credentials = new NetworkCredential(str_WebUsuario, str_WebClave);

                // --- SOLO ESTO PARA AUTENTICACIÓN (IGUAL QUE POSTMAN) ---
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(str_WebUsuario + ":" + str_WebClave));
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

                //   return "0|" + str_Respuesta;
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


            }
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);

            //    throw;
            //}
            catch (Exception e)
            {
                // ===== CAPTURAR TODA LA CADENA DE ERRORES =====
                var sb = new System.Text.StringBuilder();
                var error = e;
                int nivel = 0;

                sb.AppendLine("========== ERROR COMPLETO ==========");
                sb.AppendLine($"Fecha/Hora: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"URL: {str_URL}");
                sb.AppendLine();

                while (error != null)
                {
                    sb.AppendLine($"===== NIVEL {nivel} =====");
                    sb.AppendLine($"Tipo Exception: {error.GetType().FullName}");
                    sb.AppendLine($"Mensaje: {error.Message}");
                    sb.AppendLine($"HResult: {error.HResult}");

                    // Verificar tipos específicos de error SIN usar StatusCode
                    if (error is HttpRequestException)
                    {
                        sb.AppendLine($"Tipo: HttpRequestException detectado");
                    }

                    if (error is System.Net.Sockets.SocketException sockEx)
                    {
                        sb.AppendLine($"SocketErrorCode: {sockEx.SocketErrorCode}");
                        sb.AppendLine($"ErrorCode: {sockEx.ErrorCode}");
                    }

                    if (error is System.Security.Authentication.AuthenticationException)
                    {
                        sb.AppendLine($"Tipo: AuthenticationException - Error de SSL/TLS");
                    }

                    if (error is System.IO.IOException)
                    {
                        sb.AppendLine($"Tipo: IOException - Error de lectura/escritura");
                    }

                    if (!string.IsNullOrEmpty(error.StackTrace))
                    {
                        sb.AppendLine("StackTrace (primeras 3 líneas):");
                        var stackLines = error.StackTrace.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < Math.Min(3, stackLines.Length); i++)
                        {
                            sb.AppendLine($"  {stackLines[i].Trim()}");
                        }
                    }

                    sb.AppendLine();
                    error = error.InnerException;
                    nivel++;
                }

                string errorCompleto = sb.ToString();

                // Mostrar en consola
                Console.WriteLine(errorCompleto);

                // Guardar en archivo
                try
                {
                    string logPath = $"C:\\Logs\\BizLinks_Error_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                    System.IO.Directory.CreateDirectory("C:\\Logs");
                    System.IO.File.WriteAllText(logPath, errorCompleto);
                    Console.WriteLine($"\n✅ Log guardado en: {logPath}");
                }
                catch
                {
                    // Si falla guardar archivo, al menos se ve en consola
                }

                // Retornar JSON con el error (NO hacer throw)
                throw;
            }

            return str_Resultado;
        }

        public string ConsultarEstado(string str_documentoFiscal, string str_tipoDocumento, string str_serie, string str_numero, string str_URL, string str_WebUsuario, string str_WebClave,
                                        string str_ruta, string str_tipoEstado)
        {
            string str_resultado = string.Empty;
            HttpWebRequest obj_Request = default(HttpWebRequest);
            StringBuilder stb_Resultado = new StringBuilder();
            StreamWriter sw_Documento = null;
            byte[] obj_FileByte = null;
            XmlDocument obj_XML = new XmlDocument();
            XmlNodeList obj_Respuesta = null;
            try
            {
                stb_Resultado.AppendFormat("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://ws.ce.ebiz.com/\">");
                stb_Resultado.AppendFormat("<soapenv:Header/>");
                stb_Resultado.AppendFormat("<soapenv:Body>");
                stb_Resultado.AppendFormat("<ws:invoke>");
                stb_Resultado.AppendFormat("<command><![CDATA[<ConsultCmd output=\"PDF\">");
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"idEmisor\"/>", str_documentoFiscal.Trim());
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"tipoDocumento\"/>", str_tipoDocumento.Trim());
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"serieGrupoDocumento\"/>", str_serie.Trim());
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"numeroCorrelativoInicio\"/>", str_numero.Trim());
                stb_Resultado.AppendFormat("<parameter value=\"{0}\" name=\"numeroCorrelativoFin\"/>", str_numero.Trim());
                stb_Resultado.Append("</ConsultCmd>]]></command>");
                stb_Resultado.Append("</ws:invoke>");
                stb_Resultado.Append("</soapenv:Body>");
                stb_Resultado.Append("</soapenv:Envelope>");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                CookieContainer myContainer = new CookieContainer();
                obj_Request = (HttpWebRequest)HttpWebRequest.Create(str_URL);
                obj_Request.Method = "POST";
                obj_Request.ContentType = "text/xml;charset=UTF-8";
                obj_Request.Credentials = new NetworkCredential(str_WebUsuario, str_WebClave);
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
                            str_resultado = obj_Reader.ReadToEnd();
                        }
                    }
                }

                str_resultado = XMLLectura(str_resultado, str_tipoEstado, str_ruta);

            }
            catch (Exception ex)
            {


                str_resultado = ex.ToString();
            }

            return str_resultado;
        }

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

        private static string CambiarCaracterEspecial(string str_Texto)
        {
            if (string.IsNullOrEmpty(str_Texto))
                return str_Texto;

            return str_Texto.Replace("&", "&#38;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&#39;").Replace("\"", "&quot;").Replace("á", "&#225;").Replace("é", "&#233;").Replace("í", "&#237;").Replace("ó", "&#243;").Replace("ú", "&#250;").Replace("Á", "&#193;").Replace("É", "&#201;").Replace("Í", "&#205;").Replace("Ó", "&#211;").Replace("Ú", "&#218;").Replace("º", "&#186;").Replace("°", "&#176;").Replace("ñ", "&#241;").Replace("Ñ", "&#209;");
        }
    }
}
