//Prueba correo
using Azure.Identity;
using COBE;
using COBEC;
using CODAT;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//
namespace ServicioRSNetCore.Controllers.Funciones
{
    public class ProcesarComprobantes : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;

        public ProcesarComprobantes(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public COBEc_Error Registrar(RegistrarComprobanteRequest request)
        {
            COBEc_Error obj_return = new COBEc_Error();
            obj_return.codigo = "00";
            obj_return.mensaje = "Procesado Correctamente";
            CODAT_ComprobanteRegistrar obj_datos = new CODAT_ComprobanteRegistrar(configuration, this.context);
            ComprobanteElectronico comprobante = new ComprobanteElectronico();
            string str_ImpuestoTramaISC = string.Empty;
            

            try
            {
                DatosCompania compania = obj_datos.DatosCompania(request);
                DatosDocumento documentos = obj_datos.DatosDocumento(request);
                List<DocumentoImpuesto> impuestos = obj_datos.DatosImpuesto(request);
                List<DatosDocumentoDetalle> Detalle = obj_datos.DocumentoDetalle(request);
                //List<cls_CuentaBancaria> cuenta = obj_datos.CuentaBancaria(request);                

                //Empieza logica - Inversol
                string str_ComprobanteTipoComprobante = documentos.CodigoFiscal;
                string str_pVersionUBL = string.Empty;
                string str_pTipoCodigoProducto = string.Empty;
                decimal str_globaligv;

                str_globaligv = documentos.IGVGlobal;
                str_pVersionUBL = documentos.VersionUbl;
                str_pTipoCodigoProducto = documentos.TipoCodigoProducto;
                string str_ubicacionCSV = configuration["RutaAdjuntos"];


                //Recorremos detalle
                string ls_descripcion = string.Empty;
                string ls_ImpuestoISC = string.Empty;
                string ls_CodigoPrepago = string.Empty;
                string ls_NumeroPrepago = string.Empty;
                string ls_PrepagoSinIgv = string.Empty;
                string str_ComprobanteDetalleTrama = string.Empty;
                int ll_contador = 0;
                string ls_CodigoUbigeoDestino = string.Empty;
                string ls_CodigoUbigeoOrigen = string.Empty;
                string ls_DetalleViaje = string.Empty;
                string ls_DireccionDestino = string.Empty;
                string ls_DireccionOrigen = string.Empty;
                decimal ldc_BolsaItemCantidad = 0;
                decimal ldc_BolsaItemValorImpuesto = 0;
                decimal ldc_BolsaItemValorImpuestoUnitario = 0;
                string ls_TipoMoneda = string.Empty;
                string ls_PrepagoConIgv = string.Empty;
                string ls_CodigoAdelanto = string.Empty;
                string ls_NumeroPrepago2 = string.Empty;
                string ls_TipDocAnticipo = string.Empty;
                string ls_NumDocAnticipo = string.Empty;
                string str_ComprobantePrePagoTrama = string.Empty;
                string str_ComprobantePrePagoTrama2 = string.Empty;
                string str_ComprobanteDescuentoCargo = string.Empty;
                string ls_DescripcionComponente = string.Empty;
                string ls_TramaDescripcionComponente = string.Empty;
                string ls_Descripcion = string.Empty;
                string ls_unidad2 = string.Empty;
                decimal ldc_MontoTotal = 0;
                string ls_DescuentoCargoDetalleIndicador = string.Empty;
                string ls_DescuentoCargoDetalleCodigoAplicado = string.Empty;
                decimal ldc_DescuentoCargoDetalleMontoBase = 0;
                decimal ldc_DescuentoCargoDetalleMonto = 0;
                decimal ldc_DescuentoCargoDetallePorcentaje = 0;
                decimal ldc_ValorVentaUnitario = 0;
                string ls_item = string.Empty;
                string str_CodigoSunat = string.Empty;
                string ls_codigodetraccion = string.Empty;
                decimal ldc_PorcentajeIGV = 0;
                string str_EmpresaDepartamento = string.Empty;
                string str_EmpresaProvincia = string.Empty;
                string str_EmpresaCodigoDistrito = string.Empty;
                string str_EmpresaDistrito = string.Empty;
                string str_EmpresaCalle = string.Empty;
                string str_EmpresaTelefono = string.Empty;
                string str_EmpresaRazonSocial = string.Empty;
                string str_EmpresaNombreComercial = string.Empty;
                string str_EmpresaCodigoTipoDocumento = string.Empty;
                string str_EmpresaCorreo = string.Empty;
                string str_EmpresaWeb = string.Empty;
                string str_EmpresaRuc = string.Empty;
                string str_FormaPagoCodigoFormaPago = string.Empty;
                string str_FormaPagoNotaInstruccion = string.Empty;
                string str_Texto2 = string.Empty;
                string str_ReceptorCalle = string.Empty;
                int IdComprobanteCliente;
                string ls_EstablecimientoCodigo = string.Empty;
                string str_EmpresaCodigoEstablecimientoSunat = string.Empty;
                string str_SucursalDireccion = string.Empty;
                string ls_PuntoPartidaUbigeo = string.Empty;
                string str_SucursalTelefono = string.Empty;
                string ls_UbigeoDescripcionLocal = string.Empty;
                string str_DireccionEntregaDepartamento = string.Empty;
                string str_DireccionEntregaProvincia = string.Empty;
                string str_DireccionEntregaDistrito = string.Empty;
                string str_ComprobanteMontoTotalLetras = string.Empty;
                string str_ComprobanteObservacion1 = string.Empty;
                string str_VendedorCodigo = string.Empty;
                string str_ComprobanteVendedor = string.Empty;
                string str_Url = string.Empty;
                decimal dec_ComprobanteTotalValorVenta = 0;
                decimal dec_ComprobanteTotalPrecioVenta = 0;
                string str_TipoFormaPago = string.Empty;
                string str_GlosaFormaPago = string.Empty;
                decimal dec_MontoPendientePago = 0;
                string str_EmpresaCodigoPais = string.Empty;
                string str_ClaveAutenticacion = string.Empty;
                decimal dec_RetencionMonto = 0;
                decimal dec_RetencionPorcentaje = 0;
                decimal dec_RetencionBase;               
                decimal ldc_PrecioUnitarioOriginal = 0m;
                decimal dec_ComprobanteTotalGratuito = 0;
                decimal ldc_IgvDetalle = 0m;
                decimal ldc_igv = 0.18m;
                decimal ldc_ValorUnitarioIGV = 0m;
                string ls_Determinate = string.Empty;
                string ls_ImpuestoGratuito = string.Empty;
                string ls_TipoImpuesto = string.Empty;
                decimal dec_TotalGratuito = 0m;                
                string ls_ImpuestoIvap = string.Empty;
                decimal ldc_TotalValorVenta = 0m;                        
                decimal ldc_ImpuestoDetalle = str_globaligv;                            
                string ls_ImpuestoIGV = string.Empty;
                string ls_Monto = string.Empty;
                string ls_MontoBase = string.Empty;
                decimal ldc_FleteDetalle = 0;
                string ls_PrecioVentaItem = string.Empty;
                string ls_PrecioUnitarioOriginal = string.Empty;
                decimal ldc_IgvDetalleBolsa = 0;
                decimal ldc_IgvFlete2 = 0;
                decimal dec_ComprobanteTotalImpuestoGratuito = 0;
                DateTime w_FechaRelacionada;
                string ls_NotaCreditoDocumento;
                string ls_DocumentoRelacionado;
                string w_work = string.Empty ;
                string w_nc_tipo = string.Empty ;
                string ls_LetraAval;
                List<string> w_DocRef = new List<string>();  // ✅ Lista dinámica
                int ll_Pos, ll_Con;
                string ls_CompaniaRelacionada = string.Empty;
                string str_ComprobanteNotaDocRefTrama = string.Empty;
                string str_ComprobanteNotaCodigoMotivo = string.Empty;
                string str_ComprobanteNotaSustento = string.Empty;
                string str_ComprobanteImpuestoTrama = string.Empty;
                decimal dec_ComprobanteMontoDescuento = 0;
                string str_ComprobanteTipoOperacion = string.Empty;
                decimal dec_DetraccionMonto = 0;
                string str_DetraccionNumeroCuenta = string.Empty;
                string str_DetraccionNumeroCuentaFormaPago = string.Empty;
                string str_DetraccionCodigoBienServicio = string.Empty;
                string str_DetraccionValorBienServicio = string.Empty;
                decimal ldc_TipoCambio = 0;
                decimal ldc_MontoBoleta = 0;
                string str_ComprobanteCorreoElectronico = string.Empty;
                string str_ComprobanteRuc = string.Empty;
                string str_ComprobanteNroOrdenCompra = string.Empty;
                decimal dec_ComprobanteImporteTotal = 0;
                string str_MontoAdicionalObligCod = string.Empty;
                decimal dec_MontoAdicionalObligMonto = 0;
                string str_MontoAdicionalObligCod2 = string.Empty;
                string str_ComprobanteRazonSocial = string.Empty;
                string str_ComprobanteTipoDocumentoIdentidad = string.Empty;
                string str_MontoAdicionalObligCod3 = string.Empty;
                decimal dec_MontoAdicionalObligMonto2;
                decimal dec_MontoAdicionalObligMonto3;
                decimal dec_DetraccionPorcentaje = 0;
                string ls_detraccioncodigo = string.Empty;
                decimal ldc_PrecioUnitarioOriginalIGV = 0;
                
                //Fin de variables


                foreach (DatosDocumentoDetalle det in Detalle)
                {
                    ll_contador = ll_contador + 1;

                    //Montos detalle
                    ldc_PrecioUnitarioOriginal = det.PrecioUnitario;
                    ldc_IgvDetalle = Math.Round(det.CantidadPedida * (ldc_PrecioUnitarioOriginal * ldc_igv), 2);
                    ldc_ValorUnitarioIGV = ldc_PrecioUnitarioOriginal * (1 + ldc_igv);

                    if(det.PrecioUnitarioFinal == 0 && det.TransferenciaGratuitaFlag == "N")
                    {
                        ldc_PrecioUnitarioOriginalIGV = Math.Round(det.PrecioUnitario * (1 + ldc_igv),4);
                    }
                    else 
                    {
                        ldc_PrecioUnitarioOriginalIGV = Math.Round(det.PrecioUnitarioFinal,4);
                    }


                    //Transferencia Gratuita
                    if (det.TransferenciaGratuitaFlag == "N")
                    {
                        ls_Determinate = "01";
                        ls_ImpuestoGratuito = "0";
                        ls_TipoImpuesto = "10";
                    }
                    else
                    {
                        //Gravada
                        dec_ComprobanteTotalGratuito = dec_ComprobanteTotalGratuito + Math.Round(Math.Abs(det.PrecioUnitarioGratuito) * Math.Abs(det.CantidadPedida), 2);
                        ls_Determinate = "02";
                        ls_ImpuestoGratuito = "1";
                        ls_TipoImpuesto = "13";
                        ldc_IgvDetalle = Math.Round((Math.Round(det.CantidadPedida * det.PrecioUnitarioGratuito, 2)) * ldc_igv, 2);
                        ldc_ValorUnitarioIGV = Math.Round(det.PrecioUnitarioGratuito, 2);

                    }

                    //Impuestos Inafecto

                    if (det.IgvExoneradoFlag == "S")
                    {
                        ls_TipoImpuesto = "30";
                        ldc_IgvDetalle = 0m;
                        if (det.TransferenciaGratuitaFlag == "N")
                            ldc_ValorUnitarioIGV = ldc_PrecioUnitarioOriginal;
                    }

                    //Inafecta y Gratuita
                    if (det.IgvExoneradoFlag == "S" && det.TransferenciaGratuitaFlag == "S")
                    {
                        ls_TipoImpuesto = "32";
                        ldc_IgvDetalle = 0m;
                    }

                    //Impuesto Exonerado
                    if (det.IgvExoneradoFlag == "E")
                    {
                        ls_TipoImpuesto = "20";
                        ldc_IgvDetalle = 0m;
                        ldc_ValorUnitarioIGV = ldc_PrecioUnitarioOriginal;
                    }

                    //Exonerada y Gratuita
                    if (det.IgvExoneradoFlag == "E" && det.TransferenciaGratuitaFlag == "S")
                    {
                        ls_TipoImpuesto = "21";
                        ldc_IgvDetalle = 0m;
                    }

                    //Bonificacion Gravada
                    if (documentos.TipoFacturacion == "GBG" && det.TransferenciaGratuitaFlag == "S")
                    {
                        ls_TipoImpuesto = "15";
                        ldc_IgvDetalle = 0m;
                    }

                    //Exonerado y Gratuita
                    if (documentos.TipoFacturacion == "GBI" && det.TransferenciaGratuitaFlag == "S")
                    {
                        ls_TipoImpuesto = "31";
                        ldc_IgvDetalle = 0m;
                    }


                    //Detalle Tama
                    ls_ImpuestoISC = "0|0|0|0| |  | ";
                    ls_descripcion = det.Descripcion;

                    //Adelanto + Trama                   


                    if (documentos.TipoFacturacion.Trim() != "ADE" && det.ItemCodigo.Trim() == documentos.Advaservpg)
                    {
                        ls_CodigoPrepago = det.Descripcion.Length >= 16 ? det.Descripcion.Substring(13, 2) : "00";
                        ls_NumeroPrepago = det.Descripcion.Length >= 17 ? det.Descripcion.Substring(16).Trim() : string.Empty;
                        ls_PrepagoSinIgv = det.Monto.ToString("###0.00");

                        if (ls_TipoImpuesto == "10")
                        {
                            ls_PrepagoConIgv = (det.Monto * (1 + ldc_igv)).ToString("###0.00");
                            ls_CodigoAdelanto = "04";
                        }
                        else
                        {
                            if (documentos.TipoVenta == "EXO")
                            {
                                ls_CodigoAdelanto = "05";
                            }
                            else
                            {
                                ls_CodigoAdelanto = "06";
                            }

                            ls_PrepagoConIgv = ls_PrepagoSinIgv;
                        }

                        ls_NumeroPrepago2 = ls_NumeroPrepago.Substring(6);
                        if (ls_NumeroPrepago.Substring(0, 1) == "F" || ls_NumeroPrepago.Substring(0, 1) == "B")
                        {
                            ls_NumeroPrepago = ls_NumeroPrepago.Substring(3) + "-" + Convert.ToInt32(ls_NumeroPrepago2).ToString();
                        }
                        else
                        {
                            ls_NumeroPrepago = "0" + ls_NumeroPrepago.Substring(3) + Convert.ToInt32(ls_NumeroPrepago2).ToString();
                        }

                        if (ls_NumeroPrepago.Substring(1) == "F")
                        {
                            ls_CodigoPrepago = "02";
                        }
                        else
                        {
                            ls_CodigoPrepago = "03";
                        }


                        str_ComprobantePrePagoTrama2 = ls_CodigoPrepago + "|" + ls_NumeroPrepago + "|" + ls_PrepagoConIgv + "|" + ls_PrepagoSinIgv + "|" + compania.DocumentoFiscal + "|" + "6" + det.FechaAnticipo.ToString("YYYY-MM-DD") + "|" + det.Descripcion + "||";
                        str_ComprobantePrePagoTrama = str_ComprobantePrePagoTrama + str_ComprobantePrePagoTrama2;
                        str_ComprobanteDescuentoCargo = "0|ANTICIPO|" + ls_CodigoAdelanto + "|100.00|" + Math.Abs(det.Monto).ToString("#########0.00") + "|" + Math.Abs(det.Monto).ToString("#########0.00");
                    }
                    else
                    {
                        if (det.TransferenciaGratuitaFlag != "N")
                            dec_TotalGratuito = dec_TotalGratuito + Math.Round(det.CantidadPedida * det.PrecioUnitario, 2);


                        ls_DescripcionComponente = det.Descripcion;

                        int li_posicion;

                        if (!string.IsNullOrEmpty(ls_DescripcionComponente))
                        {
                            string separador = "\r\n";
                            li_posicion = ls_DescripcionComponente.IndexOf(separador);

                            while (li_posicion > 0)
                            {
                                ls_TramaDescripcionComponente += ls_DescripcionComponente.Substring(0, li_posicion) + "@";
                                ls_DescripcionComponente = ls_DescripcionComponente.Substring(li_posicion + separador.Length);
                                li_posicion = ls_DescripcionComponente.IndexOf(separador);

                                if (li_posicion == 0)
                                {
                                    ls_TramaDescripcionComponente += ls_DescripcionComponente;
                                }
                            }

                        }
                        else
                        {
                            ls_DescripcionComponente = "";
                        }

                        if (string.IsNullOrEmpty(ls_TramaDescripcionComponente) || ls_TramaDescripcionComponente == "")
                        {
                            ls_Descripcion = ls_DescripcionComponente;
                        }
                        else
                        {
                            ls_Descripcion = ls_TramaDescripcionComponente;
                        }

                        ls_TramaDescripcionComponente = "";

                        //co_DocumentoDetalleComentario -- no hay data

                    

                        ls_unidad2 = det.UnidadCodigo.Trim();
                        ls_ImpuestoISC = "0|0|0|0| |  | ";

                  
                        ldc_MontoTotal = ldc_MontoTotal + det.Monto;

                        if (det.PorcentajeDescuento01 > 0 && (str_ComprobanteTipoComprobante == "01" || str_ComprobanteTipoComprobante == "03"))
                        {
                            ldc_PrecioUnitarioOriginal = Math.Abs(det.PrecioUnitarioOriginal);
                            if (documentos.TipoVenta == "EXO" || documentos.TipoFacturacion == "EXP")
                            {
                                ldc_ValorUnitarioIGV = Math.Round(Math.Abs(det.PrecioUnitarioOriginal - (Math.Round(det.PrecioUnitarioOriginal * (det.PorcentajeDescuento01 / 100), 2))), 2);
                            }
                            else
                            {
                                ldc_ValorUnitarioIGV = Math.Round(Math.Round(Math.Abs(det.PrecioUnitarioOriginal - (Math.Round(det.PrecioUnitarioOriginal * (det.PorcentajeDescuento01 / 100), 2))), 2) * (1 + ldc_igv), 2);
                            }
                            ls_DescuentoCargoDetalleIndicador = "0";
                            ldc_DescuentoCargoDetalleMontoBase = Math.Round(Math.Abs(det.PrecioUnitarioOriginal) * det.CantidadPedida, 2);
                            ldc_DescuentoCargoDetalleMonto = Math.Round(ldc_DescuentoCargoDetalleMontoBase - det.Monto, 2);
                            ls_DescuentoCargoDetalleCodigoAplicado = "00";
                            ldc_DescuentoCargoDetallePorcentaje = Math.Round((ldc_DescuentoCargoDetalleMonto / ldc_DescuentoCargoDetalleMontoBase) * 100, 4);
                        }
                        else
                        {
                            ls_DescuentoCargoDetalleIndicador = " ";
                            ldc_DescuentoCargoDetalleMonto = 0;
                            ls_DescuentoCargoDetalleCodigoAplicado = " ";
                            ldc_DescuentoCargoDetalleMontoBase = 0;
                            ldc_DescuentoCargoDetallePorcentaje = 0;
                        }

                        if (det.TransferenciaGratuitaFlag == "S")
                        {
                            ldc_ValorVentaUnitario = Math.Round(det.PrecioUnitarioOriginal, 2);
                        }

                        ls_item = det.ItemCodigo;
                        if (str_pTipoCodigoProducto == "1")
                        {
                            if (det.TipoDetalle == "S")
                            {
                                str_CodigoSunat = det.CodigoSunat;
                                ls_codigodetraccion = det.CodigoDetraccion;
                            }
                            else
                            {
                                str_CodigoSunat = det.CodigoSunatItem;
                                ls_codigodetraccion = det.CodigoDetraccionItem;
                            }

                        }
                        else
                        {
                            if (det.TipoDetalle == "S")
                            {
                                str_CodigoSunat = det.CodigoSunat;
                                ls_codigodetraccion = det.CodigoDetraccion;
                            }
                            else
                            {
                                str_CodigoSunat = det.CodigoSunatItem;
                                ls_codigodetraccion = det.CodigoDetraccionItem;
                            }
                        }

                        if (str_CodigoSunat.Trim().Length != 8 || string.IsNullOrEmpty(str_CodigoSunat))
                        {
                            //Error
                            obj_return.codigo = "01";
                            obj_return.mensaje = "Configurar el catalago de productos de sunat al item" + ls_item;
                        }


                        ldc_PorcentajeIGV = str_globaligv;
                        if (documentos.CodigoFiscal == "07" && documentos.FENotaCreditoMotivo == "13")
                            ls_TipoImpuesto = "10";                       

                        switch (ls_TipoImpuesto)
                        {
                            case "10":
                                ls_ImpuestoIGV = "1000|IGV|VAT";
                                ls_Monto = Math.Round(((det.PrecioUnitario + ldc_FleteDetalle) * det.CantidadPedida), 2).ToString();
                                ldc_IgvDetalle = Math.Round((det.PrecioUnitario + ldc_FleteDetalle) * det.CantidadPedida * (ldc_PorcentajeIGV / 100), 2);
                                ls_MontoBase = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                ls_PrecioVentaItem = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida * (1 + ldc_igv)), 2).ToString("#########0.00");

                                if (det.PorcentajeDescuento01 > 0)
                                {
                                    ls_PrecioUnitarioOriginal = Math.Round(det.PrecioUnitarioOriginal + ldc_FleteDetalle, 2).ToString();
                                }
                                else
                                {
                                    ls_PrecioUnitarioOriginal = Math.Round(det.PrecioUnitario + ldc_FleteDetalle, 4).ToString();
                                }

                                ldc_ValorVentaUnitario = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * (1 + (ldc_PorcentajeIGV / 100)), 2);
                                ldc_IgvDetalleBolsa = ldc_IgvDetalle;
                                break;

                            case "30":
                                ls_ImpuestoIGV = "9998|INA|FRE";
                                ls_MontoBase = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                ldc_IgvDetalle = 0m;
                                ls_Monto = Math.Round((det.PrecioUnitario + ldc_FleteDetalle) * det.CantidadPedida, 2).ToString();

                                if (det.PorcentajeDescuento01 > 0 && (str_ComprobanteTipoComprobante == "01" || str_ComprobanteTipoComprobante == "03"))
                                {
                                    ls_PrecioUnitarioOriginal = Math.Round(det.PrecioUnitarioOriginal + ldc_FleteDetalle, 2).ToString();
                                }
                                else
                                {
                                    ls_PrecioUnitarioOriginal = Math.Round(det.PrecioUnitario + ldc_FleteDetalle, 4).ToString();
                                }

                                ldc_ValorVentaUnitario = Math.Abs(det.PrecioUnitario);
                                ldc_PorcentajeIGV = 0;
                                ls_PrecioVentaItem = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                break;

                            case "20":
                                ls_ImpuestoIGV = "9997|EXO|VAT";
                                ls_MontoBase = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                ldc_IgvDetalle = 0;
                                ls_Monto = Math.Round((det.PrecioUnitario + ldc_FleteDetalle) * det.CantidadPedida, 2).ToString();

                                if (det.PorcentajeDescuento01 > 0 && (str_ComprobanteTipoComprobante == "01" || str_ComprobanteTipoComprobante == "03"))
                                {
                                    ls_PrecioUnitarioOriginal = Math.Round(det.PrecioUnitarioOriginal + ldc_FleteDetalle, 4).ToString();
                                }
                                else
                                {
                                    ls_PrecioUnitarioOriginal = Math.Round(det.PrecioUnitario + ldc_FleteDetalle, 4).ToString();
                                }

                                ldc_ValorVentaUnitario = Math.Abs(det.PrecioUnitario);
                                ldc_PorcentajeIGV = 0;
                                ls_PrecioVentaItem = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                break;

                            case "13":
                            case "15":
                                ls_ImpuestoIGV = "9996|GRA|FRE";
                                ls_MontoBase = Math.Round(Math.Abs(det.PrecioUnitarioGratuito) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");

                                ls_Monto = ls_MontoBase;
                                ldc_IgvDetalle = Math.Round(det.PrecioUnitarioGratuito * det.CantidadPedida * (ldc_PorcentajeIGV / 100), 2);
                                ls_PrecioUnitarioOriginal = "0.00";
                                ldc_ValorVentaUnitario = Math.Abs(det.PrecioUnitarioGratuito);
                                ldc_IgvFlete2 = 0;
                                ls_PrecioVentaItem = Math.Round(Math.Abs(det.PrecioUnitario) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                dec_ComprobanteTotalImpuestoGratuito = dec_ComprobanteTotalImpuestoGratuito + ldc_IgvDetalle;
                                break;

                            case "32":
                            case "31":
                                ls_ImpuestoIGV = "9996|GRA|FRE";
                                ls_MontoBase = Math.Round(Math.Abs(det.PrecioUnitarioGratuito) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                ls_Monto = ls_MontoBase;
                                ldc_PorcentajeIGV = 0;
                                ldc_IgvDetalle = 0;
                                ls_PrecioUnitarioOriginal = "0.00";
                                ldc_ValorVentaUnitario = Math.Abs(det.PrecioUnitarioGratuito);
                                ldc_IgvFlete2 = 0;
                                ls_PrecioVentaItem = Math.Round(Math.Abs(det.PrecioUnitario) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                dec_ComprobanteTotalImpuestoGratuito = dec_ComprobanteTotalImpuestoGratuito + ldc_IgvDetalle;
                                ldc_PorcentajeIGV = 0;

                                break;

                            case "21":
                                ls_ImpuestoIGV = "9996|GRA|FRE";
                                ls_MontoBase = Math.Round(Math.Abs(det.PrecioUnitarioGratuito) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                ls_Monto = ls_MontoBase;
                                ldc_IgvDetalle = 0;
                                ls_PrecioUnitarioOriginal = "0.00";
                                ldc_ValorVentaUnitario = Math.Abs(det.PrecioUnitarioGratuito);
                                ldc_IgvFlete2 = 0;
                                ls_PrecioVentaItem = Math.Round(Math.Abs(det.PrecioUnitario) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                dec_ComprobanteTotalImpuestoGratuito = dec_ComprobanteTotalImpuestoGratuito + ldc_IgvDetalle;
                                ldc_PorcentajeIGV = 0;

                                break;

                            case "40":
                                ls_ImpuestoIGV = "9995|EXP|FRE";
                                ls_Monto = Math.Round((det.PrecioUnitario + ldc_FleteDetalle) * det.CantidadPedida, 2).ToString();
                                ls_MontoBase = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");
                                ldc_IgvDetalle = 0;
                                ls_PrecioUnitarioOriginal = (ldc_PrecioUnitarioOriginal + ldc_FleteDetalle).ToString("000000000.0000");
                                ldc_IgvFlete2 = ldc_IgvFlete2 / (1 + ldc_igv);
                                ldc_PorcentajeIGV = 0;
                                ls_PrecioVentaItem = Math.Round(Math.Abs(det.PrecioUnitario + ldc_FleteDetalle) * Math.Abs(det.CantidadPedida), 2).ToString("#########0.00");

                                break;
                            default:
                                //Error
                                obj_return.codigo = "01";
                                obj_return.mensaje = "El impuesto no se encuentra configurado, coordinar con sistemas";
                                break;
                        }

                      
                        ls_CodigoUbigeoDestino = " ";
                        ls_CodigoUbigeoOrigen = " ";
                        ls_DetalleViaje = " ";
                        ls_DireccionDestino = " ";
                        ls_DireccionOrigen = " ";

                        if (det.NumeroSerie.Length > 0)
                        {
                            ls_DescripcionComponente = ls_DescripcionComponente + "%5DCodigo IMEI:" + det.NumeroSerie;
                        }

                        ldc_TotalValorVenta += decimal.Parse(ls_MontoBase);

                        str_ComprobanteDetalleTrama = str_ComprobanteDetalleTrama +
                            det.ItemCodigo.Trim() + "|" +
                            str_CodigoSunat.Trim() + "|" +
                            ls_DescripcionComponente.Trim() + "|" +
                            det.CantidadPedida.ToString() + "|" +
                            det.UnidadMedida.Trim() + "|" +
                            ls_Monto + "|" +
                            ls_PrecioUnitarioOriginal + "|" +
                            ldc_PrecioUnitarioOriginalIGV + "|" +
                            "01" + "|" +
                            ls_Monto + "|" +
                            ldc_PorcentajeIGV + "|" +
                            ls_MontoBase + "|" +
                            ldc_IgvDetalle.ToString("F2") + "|" +
                            ls_TipoImpuesto + "|" +
                             "02" + "|" +
                             ldc_ValorUnitarioIGV.ToString() + "|" +
                             "0.00" + "|" +
                             "0.00" + "|" +
                             "0.00" + "|" +
                             "0.00" + "|" +
                             Math.Abs(ldc_BolsaItemCantidad).ToString() + "|" +
                             Math.Abs(ldc_BolsaItemValorImpuestoUnitario).ToString() + "|" +
                             Math.Abs(ldc_BolsaItemValorImpuesto + ldc_IgvDetalleBolsa).ToString("0000000000.00") + "||";
                    }
                }

                //Trama Impuesto
                if (impuestos.Count > 0)
                {
                    foreach (var impuesto in impuestos)
                    {
                        if (documentos.MontoAfecto > 0 && impuesto.Impuesto != "ICB")
                        {
                            str_ComprobanteImpuestoTrama = documentos.MontoImpuestoVentas.ToString("000000000000.00") + "|" +
                                                           documentos.MontoImpuestoVentas.ToString("000000000000.00") + "|1000|IGV|" +
                                                           str_globaligv.ToString("00.00") + "|VAT";
                        }
                        else
                        {
                            dec_ComprobanteMontoDescuento = dec_ComprobanteMontoDescuento + Math.Abs(impuesto.Monto);
                        }

                    }
                }
                else
                {
                    str_ComprobanteImpuestoTrama = "000000000000.00|000000000000.00|1000|IGV|18.00|VAT";
                }

                //Tipo de Moneda
                if (documentos.MonedaDocumento == "LO")
                {
                    ls_TipoMoneda = "PEN";
                }
                else
                {
                    ls_TipoMoneda = "USD";
                }

                //Comprobante de Referencia NC/ND               
                if (documentos.CodigoFiscal == "07" || documentos.CodigoFiscal == "08")
                {
                    w_DocRef.Add(documentos.NotaCreditoDocumento.Trim()); ;
                    ls_NotaCreditoDocumento = documentos.LetraAvalDireccion;
                    if(!string.IsNullOrEmpty(ls_NotaCreditoDocumento))
                    {

                        string[] documentosSeparados = ls_NotaCreditoDocumento.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string doc in documentosSeparados)
                        {
                            w_DocRef.Add(doc.Trim());  
                        }
                    }


                    for (ll_Con = 0; ll_Con < w_DocRef.Count; ll_Con++)
                    {
                        w_work = w_DocRef[ll_Con].Substring(0, 2);

                        //// Consulta SQL
                        //string query1 = "SELECT CO_TipoDocumento.CodigoFiscal FROM CO_TipoDocumento WHERE CO_TipoDocumento.TipoDocumento = @w_work";

                        //using (SqlConnection conn = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
                        //{
                        //    conn.Open();
                        //    using (SqlCommand cmd = new SqlCommand(query1, conn))
                        //    {
                        //        cmd.Parameters.AddWithValue("@w_work", w_work);
                        //        object result = cmd.ExecuteScalar();
                        //        w_nc_tipo = result != null ? result.ToString() : string.Empty;
                        //    }
                        //}

                        ls_DocumentoRelacionado = w_DocRef[ll_Con].Substring(3);

                        // Consulta SQL
                        string query2 = "SELECT Co_Documento.FechaDocumento FROM CO_Documento WITH(NOLOCK) WHERE CO_Documento.CompaniaSocio = @par_compania AND CO_Documento.TipoDocumento = @W_WORK AND CO_Documento.NumeroDocumento = @ls_DocumentoRelacionado";

                        using (SqlConnection conn = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand(query2, conn))
                            {
                                cmd.Parameters.AddWithValue("@par_compania", request.companiaSocio);
                                cmd.Parameters.AddWithValue("@W_WORK", w_work);
                                cmd.Parameters.AddWithValue("@ls_DocumentoRelacionado", ls_DocumentoRelacionado);

                                object result = cmd.ExecuteScalar();

                                if (result != null && result != DBNull.Value)
                                {
                                    w_FechaRelacionada = Convert.ToDateTime(result);
                                }
                                else
                                {
                                    w_FechaRelacionada = DateTime.MinValue;
                                }
                            }
                        }

                        ls_LetraAval = ls_DocumentoRelacionado.Substring(0, 4);
                        ls_CompaniaRelacionada = request.companiaSocio.Substring(0, 6);

                        if (ll_Con == 0)
                        {
                            str_ComprobanteNotaDocRefTrama = ls_DocumentoRelacionado.Substring(0, 4) + "-" + ls_DocumentoRelacionado.Substring(5) +  "|" +
                                                             documentos.TipoDocumentoReferencia + "|" +
                                                             documentos.FENotaCreditoMotivo + "|" +
                                                             //w_nc_tipo + "|" +
                                                             w_FechaRelacionada.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            str_ComprobanteNotaDocRefTrama += "||" +
                                                               ls_DocumentoRelacionado.Substring(0, 4) + "|" +
                                                               ls_DocumentoRelacionado.Substring(5) + "|" +
                                                               //w_nc_tipo + "|" +
                                                               w_FechaRelacionada.ToString("yyyy-MM-dd");
                        }
                    }

                    str_ComprobanteNotaCodigoMotivo = documentos.FENotaCreditoMotivo;
                    str_ComprobanteNotaSustento = documentos.FENotaCreditoSustento;
                    if (string.IsNullOrEmpty(str_ComprobanteNotaSustento))
                        str_ComprobanteNotaSustento = "Anulacion de la operacion";
                    
                    if (documentos.CodigoFiscal == "08" && str_ComprobanteNotaCodigoMotivo == "03" && documentos.NotaCreditoDocumento.Length >= 3 && documentos.NotaCreditoDocumento.Substring(2, 1) != "-")
                    {
                        str_ComprobanteNotaDocRefTrama = "99|0001|" + documentos.NotaCreditoDocumento.Trim() + "||";
                    }
                }
                else 
                {
                    str_ComprobanteNotaDocRefTrama = "";
                }


                if (documentos.CodigoFiscal == "08" && str_ComprobanteNotaCodigoMotivo == "03" && str_ComprobanteNotaDocRefTrama.Length < 15)
                    str_ComprobanteNotaDocRefTrama = "";

                //Detraccion          
                str_ComprobanteTipoOperacion = "0101";
                dec_DetraccionMonto = documentos.DetraccionMonto;
                dec_DetraccionPorcentaje = documentos.PorcentajeDetraccion;
                ls_detraccioncodigo = documentos.DetraccionCodigo;

                if (dec_DetraccionMonto>0 && documentos.CodigoFiscal == "01")
                {
                    str_DetraccionNumeroCuenta = compania.CuentaDetraccion;
                    str_DetraccionNumeroCuentaFormaPago = "001";
                    str_ComprobanteTipoOperacion = "1001";
                    str_DetraccionCodigoBienServicio = "3000";
                    str_DetraccionValorBienServicio = documentos.DetraccionCodigo;
                }
                
                if (documentos.CodigoFiscal == "03" || documentos.TipoDocumentoReferencia == "03")
                { 
                    if(documentos.ClienteTipoDocumento.Trim() == "1")
                    {
                        ldc_MontoTotal = documentos.MontoTotal;
                        ldc_TipoCambio = documentos.TipoDeCambio;

                        if(dec_TotalGratuito>0)
                        {
                            if (ls_TipoMoneda == "USD")
                            {
                                ldc_MontoBoleta = Math.Abs((decimal)documentos.MontoTotal) * ldc_TipoCambio;
                            }
                            else
                            {
                                ldc_MontoBoleta = Math.Abs(ldc_MontoTotal);
                            }
                        }
                       
                        if(ldc_MontoBoleta>=700)
                        {
                            if(string.IsNullOrEmpty(documentos.ClienteDocumentoIdentidad) || documentos.ClienteDocumentoIdentidad == "0")
                            {
                                str_ComprobanteRuc = documentos.ClienteDocumento.Trim();
                                str_ComprobanteCorreoElectronico = documentos.CorreoElectronicofe.Trim();
                            }
                            else
                            {
                                str_ComprobanteRuc = documentos.ClienteDocumentoIdentidad.Trim();
                                str_ComprobanteCorreoElectronico = documentos.CorreoElectronicofe.Trim();
                            }
                        }
                        else
                        {
                            str_ComprobanteCorreoElectronico = documentos.CorreoFE.Trim();
                            str_ComprobanteRuc = documentos.ClienteDocumentoIdentidad.Trim();
                        }
                    }
                    else
                    {
                        str_ComprobanteRuc = documentos.ClienteDocumentoFiscal.Trim();
                        str_ComprobanteCorreoElectronico = documentos.CorreoElectronicofe.Trim();
                    }
                }
                else
                {
                    //FC
                    str_ComprobanteRuc = documentos.ClienteDocumentoFiscal.Trim();
                    str_ComprobanteCorreoElectronico = documentos.CorreoElectronicofe.Trim();
                }

                if (string.IsNullOrEmpty(str_ComprobanteCorreoElectronico) || str_ComprobanteCorreoElectronico.Length == 0)
                    str_ComprobanteCorreoElectronico = documentos.CorreoFE;                

                str_ComprobanteRazonSocial = documentos.ClienteNombre;

                if (string.IsNullOrEmpty(str_ComprobanteNroOrdenCompra))
                {
                    str_ComprobanteNroOrdenCompra = documentos.ClienteReferencia;
                }

                str_ComprobanteTipoDocumentoIdentidad = documentos.ClienteTipoDocumento;

                if (request.numeroDocumento.Substring(0, 1) == "F")
                    str_ComprobanteTipoDocumentoIdentidad = "6";

                dec_ComprobanteImporteTotal = documentos.MontoTotal;

                /* Descuento Global */
                string ls_codigodescuento = "";

                if (dec_ComprobanteMontoDescuento > 0)
                {
                    if (documentos.MontoAfecto > 0)
                        ls_codigodescuento = "02";
                    if (documentos.MontoNoAfecto > 0)
                        ls_codigodescuento = "03";

                    if (!string.IsNullOrEmpty(str_ComprobanteDescuentoCargo))
                        str_ComprobanteDescuentoCargo = str_ComprobanteDescuentoCargo + "||";

                    str_ComprobanteDescuentoCargo += "0|DESCUENTO GLOBAL|" + ls_codigodescuento + "|" +
                        Math.Round((dec_ComprobanteMontoDescuento * 100) / (documentos.MontoAfecto + documentos.MontoNoAfecto), 2).ToString("#########0.00") + "|" +
                        Math.Round(documentos.MontoAfecto + documentos.MontoNoAfecto, 2).ToString("#########0.00") + "|" +
                        Math.Round(dec_ComprobanteMontoDescuento, 2).ToString("#########0.00") + "||";
                }

                str_MontoAdicionalObligCod = "1001";
                if (documentos.MontoAfecto > 0)
                {
                    if (dec_ComprobanteMontoDescuento > 0)
                    {
                        dec_MontoAdicionalObligMonto = documentos.MontoAfecto - dec_ComprobanteMontoDescuento;
                    }
                    else
                    {
                        dec_MontoAdicionalObligMonto = documentos.MontoAfecto;
                    }
                }

                str_MontoAdicionalObligCod2 = "1002";
                if (documentos.MontoNoAfecto > 0)
                {
                    dec_MontoAdicionalObligMonto2 = documentos.MontoNoAfecto; // + dec_ComprobanteMontoDescuento
                }
                else
                {
                    dec_MontoAdicionalObligMonto2 = documentos.MontoNoAfecto;
                }

                str_MontoAdicionalObligCod3 = "1003";

                if (documentos.MontoExonerado > 0)
                {
                    dec_MontoAdicionalObligMonto3 = documentos.MontoExonerado;                                                                               
                }
               
                str_EmpresaDepartamento = compania.Departamento;
                str_EmpresaProvincia = compania.Provincia;
                str_EmpresaDistrito = compania.Distrito;
                str_EmpresaCalle = compania.DireccionComun;
                str_EmpresaTelefono = compania.Telefono + " / " + compania.Telefono2;
                str_EmpresaRazonSocial =    compania.DescripcionLarga;
                str_EmpresaNombreComercial = compania.DescripcionLarga;
                str_EmpresaCodigoTipoDocumento = "6";
                str_EmpresaCorreo = compania.CorreoElectronico;
                str_EmpresaWeb = compania.PaginaWeb;
                str_EmpresaRuc = compania.DocumentoFiscal.Trim();             
                str_FormaPagoNotaInstruccion = documentos.FormaDePago;

                if (documentos.FormaDePago2 == "S")
                    str_FormaPagoCodigoFormaPago = "CREDITO";
                else
                    str_FormaPagoCodigoFormaPago = "CONTADO";

                str_ReceptorCalle = documentos.ClienteDireccion;

                IdComprobanteCliente = documentos.ClienteNumero;               
                ls_EstablecimientoCodigo = documentos.EstablecimientoCodigo;
                str_EmpresaCodigoEstablecimientoSunat = documentos.EmpresaCodigoEstablecimientoSunat;
                str_SucursalDireccion = documentos.SucursalDireccion;
                ls_PuntoPartidaUbigeo = documentos.PuntoPartidaUbigeo;
                str_SucursalTelefono = documentos.SucursalTelefono;

                if(str_EmpresaCodigoEstablecimientoSunat == "")
                {
                    obj_return.codigo = "01";
                    obj_return.mensaje = "El Establecimiento código:" + ls_EstablecimientoCodigo + "no cuenta con código establecimiento SUNAT";
                    return obj_return;
                }

                ls_UbigeoDescripcionLocal = documentos.UbigeoDescripcionLocal;
                if(ls_UbigeoDescripcionLocal.Length>0)
                {
                    str_DireccionEntregaDepartamento = ls_UbigeoDescripcionLocal.Substring(0, ls_UbigeoDescripcionLocal.IndexOf('-')).Trim();
                    ls_UbigeoDescripcionLocal = ls_UbigeoDescripcionLocal.Substring(ls_UbigeoDescripcionLocal.IndexOf('-') + 1);
                    str_DireccionEntregaProvincia = ls_UbigeoDescripcionLocal.Substring(0, ls_UbigeoDescripcionLocal.IndexOf('-')).Trim();
                    str_DireccionEntregaDistrito = ls_UbigeoDescripcionLocal.Substring(ls_UbigeoDescripcionLocal.IndexOf('-') + 1).Trim();
                }
                   
                str_ComprobanteMontoTotalLetras = obj_datos.f_number_to_letters(documentos.MontoTotal, documentos.MonedaDocumento, "L");
                if (dec_ComprobanteImporteTotal == 0)
                {
                    if (documentos.MonedaDocumento == "LO")
                    {
                        str_ComprobanteMontoTotalLetras = "Cero con 00/100 SOLES";
                    }
                    else
                    {
                        str_ComprobanteMontoTotalLetras = "Cero con 00/100 DOLARES AMERICANOS";
                    }
                }

                str_ComprobanteObservacion1 = "Sucursal:" + str_SucursalDireccion;//documentos.Observaciones;
                str_VendedorCodigo = documentos.VendedorCodigo;
                str_ComprobanteVendedor = documentos.ComprobanteVendedor;

                if (str_ComprobanteTipoDocumentoIdentidad == "0")
                    str_ComprobanteTipoOperacion = "0103";

                str_Url = documentos.Url;

                if (str_ComprobantePrePagoTrama.Length > 0)
                {
                    if (dec_ComprobanteMontoDescuento > 0)
                    {
                        dec_ComprobanteTotalValorVenta = Math.Round(ldc_TotalValorVenta - dec_ComprobanteMontoDescuento, 2);
                    }
                    else
                    {
                        dec_ComprobanteTotalValorVenta = Math.Round(ldc_TotalValorVenta, 2);
                    }

                    if (ls_CodigoAdelanto == "04")
                    {
                        dec_ComprobanteTotalPrecioVenta = Math.Round(dec_ComprobanteTotalValorVenta * (1 + (str_globaligv / 100)), 2);
                    }
                    else
                    {
                        dec_ComprobanteTotalPrecioVenta = dec_ComprobanteTotalValorVenta;
                    }
                }
                else
                {
                    dec_ComprobanteTotalPrecioVenta = dec_ComprobanteImporteTotal;
                    dec_ComprobanteTotalValorVenta = Math.Abs(documentos.MontoAfecto + documentos.MontoNoAfecto + documentos.MontoExonerado - dec_ComprobanteMontoDescuento);
                }

                if (dec_ComprobanteMontoDescuento > 0 && documentos.MontoNoAfecto > 0 && str_ComprobantePrePagoTrama.Length == 0)
                {
                    dec_ComprobanteTotalPrecioVenta = documentos.MontoNoAfecto; 
                    dec_ComprobanteTotalValorVenta = documentos.MontoNoAfecto; 
                }

                //Datos Forma de Pago          
                str_TipoFormaPago = documentos.TipoFormaPagoSunat;
                if(str_TipoFormaPago == "1")
                {
                    str_TipoFormaPago = "0";
                    str_GlosaFormaPago = "";
                    dec_MontoPendientePago = 0m;
                }
                else
                {
                    str_TipoFormaPago = "1";

                    str_GlosaFormaPago = documentos.TramaFormaPago;
                    dec_MontoPendientePago = documentos.MontoPendientePago;
                }
                  

                str_EmpresaCodigoPais = "PE";
                str_ClaveAutenticacion = compania.URLPassword.Trim();

                //No Domiciliado
                if (str_ComprobanteTipoDocumentoIdentidad == "0" && str_ComprobanteTipoComprobante == "03")
                {
                    str_ComprobanteTipoOperacion = "0401";
                    if (str_ComprobanteRuc.Trim().Length <= 3 || str_ComprobanteRuc.Trim().Length >= 16 || string.IsNullOrEmpty(str_ComprobanteRuc))
                        str_ComprobanteRuc = "0000";
                }


                if(dec_DetraccionMonto == 0 && documentos.MontoRetencionLocal>0)
                {
                    dec_RetencionMonto = documentos.MontoRetencionLocal;
                    dec_RetencionPorcentaje = documentos.RetencionPorcentaje;
                    dec_RetencionBase = documentos.MontoTotal;
                }

                string ls_resultado = string.Empty;

                EFACTRegistro eFACTRegistro = new EFACTRegistro();

                if (str_ComprobanteTipoComprobante == "03" || str_ComprobanteTipoComprobante == "01")
                {

                    ls_resultado = eFACTRegistro.Comprobante(request.numeroDocumento.Substring(0, 4),
                                long.Parse(request.numeroDocumento.Substring(5)).ToString(),
                                documentos.FechaDocumento,
                                documentos.FechaDocumento.ToString("hh:MM:ss"),
                                str_ComprobanteTipoComprobante,
                                str_ComprobanteTipoOperacion,
                                ls_TipoMoneda, str_ComprobanteObservacion1,
                                documentos.MontoAfecto,
                                documentos.MontoNoAfecto,
                                documentos.MontoExonerado,
                                documentos.MontoImpuestoVentas,
                                dec_ComprobanteTotalGratuito,//dec_ComprobanteTotalGratuito
                                0,//dec_ComprobanteValorVenta
                                0,//dec_ComprobanteMontoBaseDescuentoGlobal, 
                                0,//dec_ComprobantePorcentajeDescuentoGlobal,
                                0,//dec_ComprobanteTotalDescuentoGlobal, 
                                0,//dec_TotalDocumentoAnticipo,
                                0,//dec_MontoBaseDsctoGlobalAnticipo
                                0,//dec_PorcentajeDsctoGlobalAnticipo,
                                0,//dec_TotalDsctoGlobalesAnticipo, 
                                dec_ComprobanteImporteTotal,//dec_ComprobanteImporteTotal
                                str_ComprobanteMontoTotalLetras,//str_ComprobanteImporteTotalLetras
                                str_EmpresaDepartamento.Trim(),//str_EmpresaDepartamento
                                str_EmpresaProvincia.Trim(),//str_EmpresaProvincia
                                str_EmpresaDistrito.Trim(),//str_EmpresaDistrito
                                str_EmpresaCodigoDistrito.Trim(),//str_EmpresaUbigeo
                                compania.DireccionComun.Trim(),//str_EmpresaDireccion
                                "",//str_EmpresaUrbanizacion,
                                str_EmpresaRazonSocial,//str_EmpresaRazonSocial
                                str_EmpresaNombreComercial,//str_EmpresaNombreComercial
                                str_EmpresaCorreo,//str_EmpresaCorreoElectronico
                                str_EmpresaCodigoEstablecimientoSunat,//str_EmpresaEstablecimientoSunat,
                                "6",//str_EmpresaTipoDocumento, 
                                compania.DocumentoFiscal.Trim(),//str_EmpresaNumeroDocumento,
                                str_FormaPagoCodigoFormaPago,//str_FormaPagoCondicion
                                str_ComprobanteTipoDocumentoIdentidad.Trim(),//str_ReceptorTipoDocumento
                                str_ComprobanteRuc.Trim(),//str_ReceptorNumeroDocumento
                                str_ComprobanteRazonSocial.Trim(),//str_ReceptorRazonSocial
                                str_ComprobanteCorreoElectronico.Trim(),//str_ReceptorCorreoElectronico
                                str_ReceptorCalle,//str_ReceptorDireccion
                                "",//str_ReceptorUbigeo
                                documentos.ReceptorUrbanizacion,//Str_ReceptorUrbanizacion
                                documentos.ReceptorProvincia,//str_ReceptorProvincia
                                documentos.ReceptorDepartamento,//str_ReceptorDepartamento
                                documentos.ReceptorDistrito,//str_ReceptorDistrito
                                documentos.ReceptorCodigoPais,//str_ReceptorPais
                                dec_DetraccionMonto,//dec_MontoDetraccion
                                dec_DetraccionPorcentaje,//dec_DetraccionPorcentaje
                                str_DetraccionNumeroCuenta,//str_DetraccionNumeroCuenta
                                0,//dec_DetraccionValorReferencial
                                str_DetraccionValorBienServicio,//str_DetraccionBienServicio
                                "",//str_DetraccionDescripcion
                                str_ComprobanteVendedor,//str_VendedorNombre
                                "",//str_NumeroOrdenCompra,
                                "",//str_NumeroPedido
                                str_ComprobanteDetalleTrama,
                                str_ComprobantePrePagoTrama,
                                str_ComprobanteNotaDocRefTrama,
                                "",//str_ComprobanteMotivoDocumento, 
                                str_ubicacionCSV,//str_UbicacionXML,
                                str_Url,
                                str_TipoFormaPago,
                                str_GlosaFormaPago,
                                dec_MontoPendientePago,
                                0,//dec_totalTributosOpeGratuitas
                                "",//str_regimenPercepcion
                                0,//dec_baseImponiblePercepcion
                                0,//dec_porcentajePercepcion
                                0,//dec_totalPercepcion
                                0,//dec_totalVentaConPercepcion
                                "",//str_sucursal
                                0,//dec_montoBaseICBPER
                                0,//dec_totalMontoICBPER
                                "",//str_lugarDespacho
                                dec_ComprobanteTotalPrecioVenta,
                                ll_contador,
                                compania.DocumentoFiscal,
                                compania.URLPassword,
                                0,//dec_MontoRecargo,
                                0,//dec_MontoTotalExportacion,
                                0,//int_cantidadGuias, 
                                "",//str_guias,
                                documentos.FechaVencimiento.ToString("yyyy-MM-dd"),//str_fechaVencimiento,
                                0,//ldc_montoOtroExportacion,
                                0,//ldc_montoBruto,
                                0,//ldc_montoFlete, 
                                0,//ldc_montoSeguro,
                                documentos.DetraccionMonto,
                                0//ldc_montoGastosDestino
                        );
                }
                else 
                {
                    ls_resultado = eFACTRegistro.RegistrarNotas(request.numeroDocumento.Substring(0, 4),
                                long.Parse(request.numeroDocumento.Substring(5)).ToString(),
                                documentos.FechaDocumento,
                                documentos.FechaDocumento.ToString("hh:MM:ss"),
                                str_ComprobanteTipoComprobante,
                                str_ComprobanteTipoOperacion,
                                ls_TipoMoneda,
                                str_ComprobanteObservacion1,
                                documentos.MontoAfecto,
                                documentos.MontoNoAfecto,
                                documentos.MontoExonerado,
                                documentos.MontoImpuestoVentas,
                                dec_ComprobanteTotalGratuito,//dec_ComprobanteTotalGratuito
                                0,//dec_ComprobanteValorVenta
                                0,//dec_ComprobanteMontoBaseDescuentoGlobal, 
                                0,//dec_ComprobantePorcentajeDescuentoGlobal,
                                0,//dec_ComprobanteTotalDescuentoGlobal, 
                                0,//dec_TotalDocumentoAnticipo,
                                0,//dec_MontoBaseDsctoGlobalAnticipo
                                0,//dec_PorcentajeDsctoGlobalAnticipo,
                                0,//dec_TotalDsctoGlobalesAnticipo, 
                                dec_ComprobanteImporteTotal,//dec_ComprobanteImporteTotal
                                str_ComprobanteMontoTotalLetras,//str_ComprobanteImporteTotalLetras
                                str_EmpresaDepartamento.Trim(),//str_EmpresaDepartamento
                                str_EmpresaProvincia.Trim(),//str_EmpresaProvincia
                                str_EmpresaDistrito.Trim(),//str_EmpresaDistrito
                                str_EmpresaCodigoDistrito.Trim(),//str_EmpresaUbigeo
                                compania.DireccionComun.Trim(),//str_EmpresaDireccion
                                "",//str_EmpresaUrbanizacion,
                                str_EmpresaRazonSocial,//str_EmpresaRazonSocial
                                str_EmpresaNombreComercial,//str_EmpresaNombreComercial
                                str_EmpresaCorreo,//str_EmpresaCorreoElectronico
                                str_EmpresaCodigoEstablecimientoSunat,//str_EmpresaEstablecimientoSunat,
                                "6",//str_EmpresaTipoDocumento, 
                                compania.DocumentoFiscal.Trim(),//str_EmpresaNumeroDocumento,
                                str_FormaPagoCodigoFormaPago,//str_FormaPagoCondicion
                                str_ComprobanteTipoDocumentoIdentidad.Trim(),//str_ReceptorTipoDocumento
                                str_ComprobanteRuc.Trim(),//str_ReceptorNumeroDocumento
                                str_ComprobanteRazonSocial.Trim(),//str_ReceptorRazonSocial
                                str_ComprobanteCorreoElectronico.Trim(),//str_ReceptorCorreoElectronico
                                str_ReceptorCalle,//str_ReceptorDireccion
                                "",//str_ReceptorUbigeo
                                documentos.ReceptorUrbanizacion,//Str_ReceptorUrbanizacion
                                documentos.ReceptorProvincia,//str_ReceptorProvincia
                                documentos.ReceptorDepartamento,//str_ReceptorDepartamento
                                documentos.ReceptorDistrito,//str_ReceptorDistrito
                                documentos.ReceptorCodigoPais,//str_ReceptorPais
                                dec_DetraccionMonto,//dec_MontoDetraccion
                                dec_DetraccionPorcentaje,//dec_DetraccionPorcentaje
                                str_DetraccionNumeroCuenta,//str_DetraccionNumeroCuenta
                                0,//dec_DetraccionValorReferencial
                                str_DetraccionValorBienServicio,//str_DetraccionBienServicio
                                "",//str_DetraccionDescripcion
                                str_ComprobanteVendedor,//str_VendedorNombre
                                "",//str_NumeroOrdenCompra,
                                "",//str_NumeroPedido
                                str_ComprobanteDetalleTrama,
                                str_ComprobantePrePagoTrama,
                                str_ComprobanteNotaDocRefTrama,
                                str_ComprobanteNotaSustento,//str_ComprobanteMotivoDocumento, 
                                str_ubicacionCSV,//str_UbicacionXML,
                                str_Url,
                                str_TipoFormaPago,
                                str_GlosaFormaPago,
                                dec_MontoPendientePago,
                                0,//dec_totalTributosOpeGratuitas
                                "",//str_regimenPercepcion
                                0,//dec_baseImponiblePercepcion
                                0,//dec_porcentajePercepcion
                                0,//dec_totalPercepcion
                                0,//dec_totalVentaConPercepcion
                                "",//str_sucursal
                                0,//dec_montoBaseICBPER
                                0,//dec_totalMontoICBPER
                                "",//str_lugarDespacho
                                dec_ComprobanteTotalPrecioVenta,
                                ll_contador,
                                compania.DocumentoFiscal,
                                compania.URLPassword,
                                0m,//dec_MontoRecargo,
                                0m,//dec_MontoTotalExportacion,
                                0,//int_cantidadGuias, 
                                "",//str_guias,
                                documentos.FechaVencimiento.ToString("yyyy-MM-dd"),//str_fechaVencimiento,
                                0m,//ldc_montoOtroExportacion,
                                0m,//ldc_montoBruto,
                                0m,//ldc_montoFlete, 
                                0m,//ldc_montoSeguro,                               
                                0m);//ldc_montoGastosDestino
                }


                    //Fin Inversol                


                string[] str_ListaResultado = ls_resultado.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                //bool existe = str_ListaResultado[1].Contains("Descripción Detalle: El documento ya fue firmado",
                //           StringComparison.OrdinalIgnoreCase);

                obj_return.codigo = str_ListaResultado[0];
                obj_return.mensaje = str_ListaResultado[2];

                COBEc_Error obj_datosUpdate = new COBEc_Error();
                obj_datosUpdate = obj_datos.DocumentoActualizarEstado(request, obj_return.mensaje, obj_return.codigo);

                if(obj_datosUpdate.codigo == "1")
                    return obj_datosUpdate;

            }
            catch (Exception ex)
            {
                obj_return.codigo = "01";
                obj_return.mensaje = "ERROR - Procesa Comprobante - Registrar :" + ex.Message.ToString();
            }

            return obj_return;

        } 

    }
}
