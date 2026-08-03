using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using COBE;
using System.Data.Common;
using System.Data.SqlClient;

using COBEC;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CODAT
{
    public class CODAT_DatosDocumento : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;
        public CODAT_DatosDocumento(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        //public DatosCabecera Lista(COBEC_Comprobante cOBEC_Comprobante)
        //{
        //    DatosCabecera objreturn = new DatosCabecera();
        //    DbCommand _command = context.Database.GetDbConnection().CreateCommand();
        //    try
        //    {
        //        _command.CommandType = System.Data.CommandType.StoredProcedure;
        //        _command.CommandText = "SNP_API_Datos_Documento";
        //        _command.Connection.Open();

        //        SqlParameter p_compania = new SqlParameter("@p_compania", cOBEC_Comprobante.companiaSocio);
        //        _command.Parameters.Add(p_compania);

        //        SqlParameter p_tipodocumento = new SqlParameter("@p_tipodocumento", cOBEC_Comprobante.tipoDocumento);
        //        _command.Parameters.Add(p_tipodocumento);

        //        SqlParameter p_comprobante = new SqlParameter("@p_comprobante", cOBEC_Comprobante.numeroDocumento);
        //        _command.Parameters.Add(p_comprobante);

        //        DbDataReader _reader = _command.ExecuteReader();

        //        while (_reader.Read())
        //        {
        //            // Para propiedades string
        //            objreturn.CompaniaSocio = _reader["CompaniaSocio"]?.ToString() ?? string.Empty;
        //            objreturn.CodigoFiscal = _reader["CodigoFiscal"]?.ToString() ?? string.Empty;
        //            objreturn.TipoDocumento = _reader["TipoDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.SerieDocumento = _reader["SerieDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.NumeroDocumento = _reader["NumeroDocumento"]?.ToString() ?? string.Empty;

        //            // Para propiedades DateTime
        //            objreturn.FechaDocumento = _reader["FechaDocumento"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(_reader["FechaDocumento"]);
        //            objreturn.FechaVencimiento = _reader["FechaVencimiento"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(_reader["FechaVencimiento"]);

        //            // Más propiedades string
        //            objreturn.ClienteRuc = _reader["ClienteRuc"]?.ToString() ?? string.Empty;                    
        //            objreturn.ClienteCobrarA = _reader["ClienteCobrarA"] is DBNull ? 0 : Convert.ToInt32(_reader["ClienteCobrarA"]);
        //            objreturn.ClienteDireccionSecuencia = _reader["ClienteDireccionSecuencia"]?.ToString() ?? string.Empty;                    
        //            objreturn.ConceptoFacturacion = _reader["ConceptoFacturacion"]?.ToString() ?? string.Empty;
        //            objreturn.AlmacenCodigo = _reader["AlmacenCodigo"]?.ToString() ?? string.Empty;
        //            objreturn.FormaFacturacion = _reader["FormaFacturacion"]?.ToString() ?? string.Empty;
        //            objreturn.TipoFacturacion = _reader["TipoFacturacion"]?.ToString() ?? string.Empty;

        //            // Para propiedades decimal
        //            objreturn.MontoRedondeo = _reader["MontoRedondeo"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoRedondeo"]);

        //            // Más propiedades string
        //            objreturn.DireccionEntrega = _reader["DireccionEntrega"]?.ToString() ?? string.Empty;

        //            // Para propiedades int
        //            objreturn.DireccionEntregaSec = _reader["DireccionEntregaSec"] is DBNull ? 0 : Convert.ToInt32(_reader["DireccionEntregaSec"]);
        //            //objreturn.ClienteNumero = _reader["ClienteNumero"]?.ToString() ?? string.Empty;

        //            // Continúa con el mismo patrón para todas las propiedades...
        //            objreturn.LetraAvalDireccion = _reader["LetraAvalDireccion"]?.ToString() ?? string.Empty;
        //            objreturn.EstablecimientoCodigo = _reader["EstablecimientoCodigo"]?.ToString() ?? string.Empty;
        //            objreturn.ClienteNombre = _reader["ClienteNombre"]?.ToString() ?? string.Empty;
        //            objreturn.Sucursal = _reader["Sucursal"]?.ToString() ?? string.Empty;
        //            objreturn.MontoPercepcion = _reader["MontoPercepcion"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoPercepcion"]);
        //            objreturn.NombreCompleto = _reader["NombreCompleto"]?.ToString() ?? string.Empty;
        //            objreturn.TransportistaVehiculo = _reader["TransportistaVehiculo"]?.ToString() ?? string.Empty;
        //            objreturn.TipoDeCambio = _reader["TipodeCambio"] is DBNull ? 0m : Convert.ToDecimal(_reader["TipodeCambio"]);
        //            objreturn.ClienteReferencia = _reader["ClienteReferencia"]?.ToString() ?? string.Empty;
        //            objreturn.NotaCreditoDocumento = _reader["NotaCreditoDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.FENotaCreditoMotivo = _reader["FENotaCreditoMotivo"]?.ToString() ?? string.Empty;
        //            objreturn.FENotaCreditoSustento = _reader["FENotaCreditoSustento"]?.ToString() ?? string.Empty;
        //            objreturn.TipoDocumentoReferencia = _reader["TipoDocumentoReferenia"]?.ToString() ?? string.Empty;
        //            objreturn.SerieReferencia = _reader["SerieReferencia"]?.ToString() ?? string.Empty;
        //            objreturn.NumeroReferencia = _reader["NumeroReferencia"]?.ToString() ?? string.Empty;
        //            objreturn.NumeroInterno = _reader["NumeroInterno"]?.ToString() ?? string.Empty;
        //            objreturn.TipoDocumentoInterno = _reader["TipoDocumentoInterno"]?.ToString() ?? string.Empty;
        //            objreturn.SerieInterna = _reader["SerieInterna"]?.ToString() ?? string.Empty;
        //            objreturn.NumeroInterna = _reader["NumeroInterna"]?.ToString() ?? string.Empty;
        //            objreturn.PersonaDireccion = _reader["PersonaDireccion"]?.ToString() ?? string.Empty;
        //            objreturn.ClienteDireccion = _reader["ClienteDireccion"]?.ToString() ?? string.Empty;
        //            objreturn.Ubigeo = _reader["Ubigeo"]?.ToString() ?? string.Empty;
        //            objreturn.FormaDePago = _reader["FormadePago"]?.ToString() ?? string.Empty;
        //            objreturn.FormaDePago2 = _reader["FormadePago2"]?.ToString() ?? string.Empty;
        //            objreturn.ClienteTipoDocumento = _reader["ClienteTipoDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.ClienteDocumento = _reader["ClienteDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.SunatNacionalidad = _reader["SunatNacionalidad"]?.ToString() ?? string.Empty;
        //            objreturn.ClienteDocumentoFiscal = _reader["ClienteDocumentoFiscal"]?.ToString() ?? string.Empty;
        //            objreturn.ClienteDocumentoIdentidad = _reader["ClienteDocumentoIdentidad"]?.ToString() ?? string.Empty;
        //            objreturn.CorreoElectronicofe = _reader["CorreoElectronicofe"]?.ToString() ?? string.Empty;
        //            objreturn.MonedaDocumento = _reader["MonedaDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.MontoAfecto = _reader["MontoAfecto"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoAfecto"]);
        //            objreturn.MontoNoAfecto = _reader["MontoNoAfecto"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoNoAfecto"]);
        //            objreturn.MontoIvap = _reader["montoivap"] is DBNull ? 0m : Convert.ToDecimal(_reader["montoivap"]);
        //            objreturn.MontoImpuestoVentas = _reader["MontoImpuestoVentas"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoImpuestoVentas"]);
        //            objreturn.MontoRetencionLocal = _reader["montoretencionlocal"] is DBNull ? 0m : Convert.ToDecimal(_reader["montoretencionlocal"]);
        //            objreturn.MontoImpuestos = _reader["montoimpuestos"] is DBNull ? 0m : Convert.ToDecimal(_reader["montoimpuestos"]);
        //            objreturn.MontoDescuentos = _reader["MontoDescuentos"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoDescuentos"]);
        //            objreturn.MontoTotal = _reader["MontoTotal"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoTotal"]);
        //            objreturn.DocumentoReferencia = _reader["DocumentoReferencia"]?.ToString() ?? string.Empty;
        //            objreturn.CodigoFiscalReferencia = _reader["CodigoFiscalReferencia"]?.ToString() ?? string.Empty;
        //            objreturn.Estado = _reader["Estado"]?.ToString() ?? string.Empty;
        //            objreturn.Vendedor = _reader["Vendedor"]?.ToString() ?? string.Empty;
        //            objreturn.Comentarios = _reader["Comentarios"]?.ToString() ?? string.Empty;
        //            objreturn.TipoVenta = _reader["TipoVenta"]?.ToString() ?? string.Empty;
        //            objreturn.DetraccionMontoLocal = _reader["DetraccionMontoLocal"] is DBNull ? 0m : Convert.ToDecimal(_reader["DetraccionMontoLocal"]);
        //            objreturn.DetraccionCodigo = _reader["DetraccionCodigo"]?.ToString() ?? string.Empty;
        //            objreturn.TipoCambio = _reader["TipoCambio"] is DBNull ? 0m : Convert.ToDecimal(_reader["TipoCambio"]);
        //            objreturn.ValorIgv = _reader["ValorIGV"] is DBNull ? 0m : Convert.ToDecimal(_reader["ValorIGV"]);
        //            objreturn.ClienteDescripcionDocumento = _reader["ClienteDescripcionDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.FEFlag = _reader["FEFlag"]?.ToString() ?? string.Empty;
        //            objreturn.FETipoComprobanteRef = _reader["FETipoComprobanteRef"]?.ToString() ?? string.Empty;
        //            objreturn.TipoFormaPagoSunat = _reader["TipoFormaPagoSunat"]?.ToString() ?? string.Empty;
        //            objreturn.TramaFormaPago = _reader["TramaFormaPago"]?.ToString() ?? string.Empty;
        //            objreturn.MontoPendientePago = _reader["MontoPendientePago"] is DBNull ? 0m : Convert.ToDecimal(_reader["MontoPendientePago"]);

        //        }
        //        _command.Connection.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("error" + e.Message);
        //        _command.Connection.Close();
        //    }
        //    return objreturn;
        //}
    }
}
