using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{
    public class cls_DatosPercepcion
    {
        public string TipoDocumentoReceptor { get; set; }
        public string DocumentoReceptor { get; set; }
        public string RazonSocialReceptor { get; set; }
        public string NombreComercialReceptor { get; set; }
        public string UbigeoReceptor { get; set; }
        public string DireccionReceptor { get; set; }
        public string DepartamentoReceptor { get; set; }
        public string ProvinciaReceptor { get; set; }
        public string DistritoReceptor { get; set; }

        public string CodigoPostalReceptor { get; set; }
        public string CorreoReceptor { get; set; }
        public string UnidadNegocio { get; set; }
        public string DocumentoRelacionadoPX { get; set; }
        public decimal MontoPercepcion { get; set; }
        public string TipoRelacionado { get; set; }
        public string SerieRelacionado { get; set; }
        public string NumeroRelacionado { get; set; }
        public DateTime FechaDocumento { get; set; }
        public decimal MontoTotal { get; set; }
        public string Moneda { get; set; }
        public decimal TipoCambio { get; set; }
    }
}
