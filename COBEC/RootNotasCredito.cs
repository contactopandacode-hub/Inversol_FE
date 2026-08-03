using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{
    using System;
    using System.Collections.Generic;

    public class NotaCredito
    {
        public string Nc_Tipodoc { get; set; }
        public string Nc_Seriedoc { get; set; }
        public string Nc_Nrodoc { get; set; }
        public string Nc_Motivo { get; set; }
        public decimal Nc_Total { get; set; }
        public decimal Nc_Subtotal { get; set; }
        public decimal Nc_Igv { get; set; }
        public decimal Nc_Isc { get; set; }
        public decimal Nc_Ivap { get; set; }
        public string Nc_Fechatributacion { get; set; }
        public string Nc_Estado { get; set; }
        public string Compra_Tipodoc { get; set; }
        public string Compra_Seriedoc { get; set; }
        public string Compra_Nrodoc { get; set; }
        public string Proveedor_Ruc { get; set; }
    }

    public class ProductoNC
    {
        public string Detallenc_Descripcion { get; set; }
        public decimal Detallenc_Costosinimpuesto { get; set; }
        public decimal Detallenc_Igv { get; set; }
        public decimal Detallenc_Isc { get; set; }
        public decimal Detallenc_Ivap { get; set; }
        public string Producto_Codigointerno { get; set; }
        public int Presentacioninsumo_Codigointerno { get; set; }
        public decimal Detallenc_Cantidad { get; set; }
    }

    public class NotaCreditoData
    {
        public NotaCredito Nota_Credito { get; set; }
        public List<ProductoNC> Productos { get; set; }
    }

    public class RootNotasCredito
    {
        public List<NotaCreditoData> ListaNotasCredito { get; set; }
    }

}
