using System;
using System.Collections.Generic;
using System.Text;

namespace COBE
{
    public class ComprobanteElectronico
    {
        // Información básica del comprobante
        public string VersionUBL { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public DateTime FechaEmision { get; set; }
        public string TipoComprobante { get; set; }
        public string TipoOperacion { get; set; }
        public string TipoCodigoProducto { get; set; }

        public decimal MontoImpuestoVentas { get; set; }

        // Información del cliente/receptor
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string CorreoElectronico { get; set; }
        public string TipoDocumentoIdentidad { get; set; }
        public string NumeroOrdenCompra { get; set; }
        public string CodigoCliente { get; set; }

        // Totales y montos
        public decimal ImporteTotal { get; set; }
        public string ImporteTotalLetras { get; set; }
        public decimal TotalGratuito { get; set; }
        public decimal TotalImpuestoGratuito { get; set; }
        public decimal DescuentoGlobal { get; set; }
        public decimal TotalExportacion { get; set; }
        public decimal MontoBaseIGV { get; set; }
        public decimal MontoBaseISC { get; set; }
        public decimal MontoBaseIVAP { get; set; }
        public decimal TotalPrecioVenta { get; set; }
        public decimal TotalValorVenta { get; set; }
        public decimal TotalCargo { get; set; }
        public decimal TotalISC { get; set; }

        // Percepción
        public string PercepcionCodigoRegimen { get; set; }
        public decimal MontoPercepcion { get; set; }
        public decimal BasePercepcion { get; set; }
        public decimal PercepcionTasa { get; set; }
        public decimal MontoTotalPercepcion { get; set; }

        // Montos adicionales
        public string MontoAdicionalObligCod1 { get; set; }
        public decimal MontoAdicionalObligMonto1 { get; set; }
        public string MontoAdicionalObligCod2 { get; set; }
        public decimal MontoAdicionalObligMonto2 { get; set; }
        public string MontoAdicionalObligCod3 { get; set; }
        public decimal MontoAdicionalObligMonto3 { get; set; }

        // Información de la empresa
        public string EmpresaCodigoPais { get; set; }
        public string EmpresaDepartamento { get; set; }
        public string EmpresaProvincia { get; set; }
        public string EmpresaCodDistrito { get; set; }
        public string EmpresaDistrito { get; set; }
        public string EmpresaCalle { get; set; }
        public string EmpresaUrbanizacion { get; set; }
        public string EmpresaTelefono { get; set; }
        public string EmpresaRazonSocial { get; set; }
        public string EmpresaNombreComercial { get; set; }
        public string EmpresaCorreo { get; set; }
        public string EmpresaWeb { get; set; }
        public string EmpresaFax { get; set; }
        public string EmpresaCodigoTipoDocumento { get; set; }
        public string EmpresaRuc { get; set; }
        public string EmpresaCodigoEstablecimientoSunat { get; set; }
        public string URLUsuario { get; set; }
        public string URLPassword { get; set; }
        public string urlWebService { get; set; }

        // Forma de pago
        public string FormaPagoNotaInstruccion { get; set; }
        public string FormaPagoCodigoFormaPago { get; set; }
        public DateTime? FormaPagoFechaVencimiento { get; set; }

        // Información del receptor
        public string ReceptorCalle { get; set; }
        public string ReceptorUrbanizacion { get; set; }
        public string ReceptorCodigoPais { get; set; }
        public string ReceptorDepartamento { get; set; }
        public string ReceptorProvincia { get; set; }
        public string ReceptorDistrito { get; set; }

        // Tramas
        public string ImpuestoTrama { get; set; }
        public string DetalleTrama { get; set; }
        public string PrePagoTrama { get; set; }
        public string GrillaCuentaTrama { get; set; }
        public string DescuentoCargoCabeceraTrama { get; set; }
        public string NotaDocRefTrama { get; set; }

        // Motivos y referencias
        public string MotivoDocumento { get; set; }
        public string MotivoSustento { get; set; }
        public int IdComprobanteCliente { get; set; }

        // Detracción
        public string DetraccionNumeroCuenta { get; set; }
        public string DetraccionNumeroCuentaCodigoFormaPago { get; set; }
        public decimal DetraccionPorcentaje { get; set; }
        public string DetraccionBienesServicios { get; set; }
        public decimal Detraccion { get; set; }

        // Sucursal
        public string SucursalDepartamento { get; set; }
        public string SucursalProvincia { get; set; }
        public string SucursalDistrito { get; set; }
        public string SucursalDireccion { get; set; }
        public string SucursalTelefono { get; set; }
        public string SucursalNombre { get; set; }

        // Transporte/Guía
        public string VehiculoPlaca { get; set; }
        public string GuiaCodigoMotivoTraslado { get; set; }
        public string GuiaModalidadTransporte { get; set; }
        public string GuiaFechaInicioTraslado { get; set; }
        public string GuiaCodigoTipoDocumento { get; set; }
        public string GuiaNumeroDocumento { get; set; }
        public string GuiaVehiculoPlaca { get; set; }
        public string GuiaVehiculoConstanciaInscripcion { get; set; }
        public string GuiaVehiculoMarca { get; set; }
        public string GuiaNroLicencia { get; set; }
        public string GuiaTransportista { get; set; }
        public string GuiaTransportistaRuc { get; set; }
        public decimal GuiaTotalPesoBruto { get; set; }
        public string GuiaUnidadPesoBruto { get; set; }

        // Direcciones
        public string DireccionEntregaCodPais { get; set; }
        public string DireccionEntregaDepartamento { get; set; }
        public string DireccionEntregaProvincia { get; set; }
        public string DireccionEntregaDistrito { get; set; }
        public string DireccionEntregaCalle { get; set; }
        public string DireccionEntregaUrbanizacion { get; set; }

        public string ClienteDireccion { get; set; }

        // Puntos de partida/llegada
        public string PuntoPartidaCodigoPais { get; set; }
        public string PuntoPartidaDepartamento { get; set; }
        public string PuntoPartidaProvincia { get; set; }
        public string PuntoPartidaUbigeo { get; set; }
        public string PuntoPartidaDistrito { get; set; }
        public string PuntoPartidaDireccion { get; set; }
        public string PuntoPartidaUrbanizacion { get; set; }
        public string PuntoLlegadaCodigoPais { get; set; }
        public string PuntoLlegadaDepartamento { get; set; }
        public string PuntoLlegadaProvincia { get; set; }
        public string PuntoLlegadaUbigeo { get; set; }
        public string PuntoLlegadaDistrito { get; set; }
        public string PuntoLlegadaDireccion { get; set; }
        public string PuntoLlegadaUrbanizacion { get; set; }

        // Vendedor
        public string VendedorCodigo { get; set; }
        public string VendedorNombre { get; set; }

        // Pagos
        public string TipoFormaPago { get; set; }
        public string GlosaFormaPago { get; set; }
        public decimal MontoPendientePago { get; set; }

        // Retención
        public decimal RetencionMonto { get; set; }
        public decimal RetencionPorcentaje { get; set; }
        public decimal RetencionBase { get; set; }

        // Multi-glosa
        public string MultiGlosa { get; set; }

        // Moneda
        public string TipoMoneda { get; set; }

        // Guía relacionada
        public string GuiaNumero { get; set; }
    }
}
