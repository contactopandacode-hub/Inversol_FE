using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{

    public class Compra
    {
        public string Compra_Tipodoc { get; set; }
        public string Compra_Observacion { get; set; }
        public decimal Compra_Montoefectivo { get; set; }
        public decimal Compra_Montotarjeta { get; set; }
        public decimal Compra_Montocheque { get; set; }
        public decimal Compra_Descuento { get; set; }
        public decimal Compra_Total { get; set; }
        public decimal Compra_Subtotal { get; set; }
        public decimal Compra_Igv { get; set; }
        public decimal Compra_Isc { get; set; }
        public decimal Compra_Ivap { get; set; }
        public string Compra_Fecha { get; set; }
        public string Compra_Estado { get; set; }
        public decimal Compra_Descuentototal { get; set; }
        public string Compra_Tipocambio { get; set; }
        public string Compra_Seriedoc { get; set; }
        public string Compra_Nrodoc { get; set; }
        public string Compra_Fechatributacion { get; set; }
        public string Proveedor_Ruc { get; set; }
        public string Moneda_Id { get; set; }
        public string Local_Id { get; set; }
        public string Almacen_Codigo { get; set; }
        public string transaccion { get; set; }

    }

    public class Producto
    {
        public string Detallecompra_Descripcion { get; set; }
        public decimal Detallecompra_Costosinimpuesto { get; set; }
        public decimal Detallecompra_Igv { get; set; }
        public decimal Detallecompra_Isc { get; set; }
        public decimal Detallecompra_Ivap { get; set; }
        public string Producto_Codigointerno { get; set; }
        public int Presentacioninsumo_Codigointerno { get; set; }
        public decimal Detallecompra_Descuento { get; set; }
        public decimal Detallecompra_Cantidad { get; set; }
    }

    public class Compras
    {
        public Compra Compra { get; set; }= new Compra();   
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }

    public class RootCompras
    {
        public List<Compras> ListaCompras { get; set; } = new List<Compras>();
    }
}
