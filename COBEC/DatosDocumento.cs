using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace COBEC
{
    public class DatosDocumento
    {
        // Strings (ya son nullable por defecto)
        public string CompaniaSocio { get; set; }
        public string CodigoFiscal { get; set; }
        public string TipoDocumento { get; set; }
        public string SerieDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        // DateTimes nullable
        public DateTime FechaDocumento { get; set; }
        public DateTime FechaVencimiento { get; set; }

        public string ClienteRuc { get; set; }

        // Numericos nullable
        public long ClienteCobrarA { get; set; }
        public string ClienteDireccionSecuencia { get; set; }
        public int ClienteNumero { get; set; }
        public string ConceptoFacturacion { get; set; }
        public string AlmacenCodigo { get; set; }
        public string FormaFacturacion { get; set; }
        public string TipoFacturacion { get; set; }
        public decimal MontoRedondeo { get; set; }
        public string DireccionEntrega { get; set; }
        public int DireccionEntregaSec { get; set; }
        public string LetraAvalDireccion { get; set; }
        public string EstablecimientoCodigo { get; set; }
        public string ClienteNombre { get; set; }
        public string Sucursal { get; set; }
        public decimal MontoPercepcion { get; set; }
        public string NombreCompleto { get; set; }
        public string TransportistaVehiculo { get; set; }
        public decimal TipoDeCambio { get; set; }
        public string ClienteReferencia { get; set; }
        public string NotaCreditoDocumento { get; set; }
        public string FENotaCreditoMotivo { get; set; }
        public string FENotaCreditoSustento { get; set; }
        public string TipoDocumentoReferencia { get; set; }
        public string SerieReferencia { get; set; }
        public string NumeroReferencia { get; set; }
        public string NumeroInterno { get; set; }
        public string TipoDocumentoInterno { get; set; }
        public string SerieInterna { get; set; }
        public string NumeroInterna { get; set; }
        public string PersonaDireccion { get; set; }
        public string ClienteDireccion { get; set; }
        public string Ubigeo { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string FormaDePago { get; set; }
        public string FormaDePago2 { get; set; }
        public string ClienteTipoDocumento { get; set; }
        public string ClienteDocumento { get; set; }
        public string Telefono { get; set; }
        public string SunatNacionalidad { get; set; }
        public string ClienteDocumentoFiscal { get; set; }
        public string ClienteDocumentoIdentidad { get; set; }
        public string CorreoElectronicofe { get; set; }
        public string MonedaDocumento { get; set; }
        public decimal MontoAfecto { get; set; }
        public decimal MontoNoAfecto { get; set; }
        public decimal MontoIvap { get; set; }
        public decimal MontoImpuestoVentas { get; set; }
        public decimal MontoRetencionLocal { get; set; }
        public decimal MontoExonerado { get; set; }
        public decimal MontoImpuestos { get; set; }
        public decimal MontoDescuentos { get; set; }
        public decimal MontoTotal { get; set; }
        public string DocumentoReferencia { get; set; }
        public string CodigoFiscalReferencia { get; set; }
        public string Estado { get; set; }
        public string Vendedor { get; set; }
        public string Observaciones { get; set; }
        public string Comentarios { get; set; }
        public string TipoVenta { get; set; }
        public decimal DetraccionMontoLocal { get; set; }
        public string DetraccionCodigo { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal ValorIGV { get; set; }
        public string ClienteDescripcionDocumento { get; set; }
        public string FEFlag { get; set; }
        public string FETipoComprobanteRef { get; set; }
        public string TipoFormaPagoSunat { get; set; }
        public string TramaFormaPago { get; set; }
        public decimal MontoPendientePago { get; set; }
        public string VersionUbl { get; set; }
        public string TipoCodigoProducto { get; set; }
        public decimal PorcentajeDetraccion { get; set; }
        public string DetraccionBienesServicios { get; set; }
        public decimal DetraccionMonto { get; set; }
        public string Advaservpg { get; set; }
        public string CorreoFE { get; set; }
        public string SucursalDireccion { get; set; }
        public string SucursalDepartamento { get; set; }
        public string SucursalProvincia { get; set; }
        public string SucursalDistrito { get; set; }
        public string SucursalNombre { get; set; }
        public string SucursalTelefono { get; set; }
        public string EmpresaCodigoEstablecimientoSunat { get; set; }
        public string VendedorNombre { get; set; }
        public string VendedorCodigo { get; set; }
        public string ReceptorDistrito { get; set; }
        public string ReceptorProvincia { get; set; }
        public string ReceptorDepartamento { get; set; }
        public string ReceptorCalle { get; set; }
        public string ReceptorUrbanizacion { get; set; }
        public string DireccionEntregaCodPais { get; set; }
        public string ReceptorCodigoPais { get; set; }
        public string MonedaDescripcion { get; set; }
        public decimal FactorPorcentaje { get; set; }
        public decimal RetencionPorcentaje { get; set; }
        public string cuentaBancaria { get; set; }
        public decimal porcentajeIvap { get; set; }
        public decimal IGVGlobal { get; set; }
        public string PuntoPartidaUbigeo { get; set; }
        public string UbigeoDescripcionLocal { get; set; }
        public string ComprobanteVendedor { get; set; }
        public string Url { get; set; }
    }
}