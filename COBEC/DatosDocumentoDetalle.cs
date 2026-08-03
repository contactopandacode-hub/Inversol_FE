using System;

namespace COBEC
{
    public class DatosDocumentoDetalle
    {
        public string CompaniaSocio { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public int Linea { get; set; }
        public string TipoDetalle { get; set; }
        public string Lote { get; set; }
        public string ItemCodigo { get; set; }
        public string Descripcion { get; set; }
        public string UnidadCodigo { get; set; }
        public decimal CantidadPedida { get; set; }
        public string NumeroSerie { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioUnitarioFinal { get; set; }
        public decimal PorcentajeDescuento01 { get; set; }
        public decimal PorcentajeDescuento02 { get; set; }
        public decimal PorcentajeDescuento03 { get; set; }
        public string Estado { get; set; }
        public DateTime UltimaFechaModif { get; set; }
        public string UltimoUsuario { get; set; }
        public string IgvExoneradoFlag { get; set; }
        public string TransferenciaGratuitaFlag { get; set; }
        public decimal Monto { get; set; }
        public decimal PrecioUnitarioGratuito { get; set; }
        public decimal PrecioUnitarioOriginal { get; set; }

        public DateTime FechaAnticipo { get; set; }
        public string UnidadMedida { get; set; }
        public string CodigoSunat { get; set; }
        public string CodigoDetraccion { get; set; }
        public string CodigoSunatItem { get; set; }
        public string CodigoDetraccionItem { get; set; }
    }
}