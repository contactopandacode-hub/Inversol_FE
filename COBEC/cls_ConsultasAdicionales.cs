using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace COBEC
{
    public class cls_ConsultasAdicionales
    {
        public decimal FactorPorcentaje { get; set; }
        public string CodigoFiscal { get; set; }
        public string MotivoSustento { get; set; }
        public DateTime FechaDocumento { get; set; }
        public string CodigoFormaPago { get; set; }

    }
}
