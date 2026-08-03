using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{
    public class DocumentoImpuesto
    {
        public string CompaniaSocio { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoRegistro { get; set; }
        public string Impuesto { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Monto { get; set; }        
    }
}
