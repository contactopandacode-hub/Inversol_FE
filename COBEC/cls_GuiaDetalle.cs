using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{
    public class cls_GuiaDetalle
    {
        public string Descripcion { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public decimal cantidad { get; set; }
        public string unidadFE { get; set; } = string.Empty;
        public string ItemCodigo { get; set; } = string.Empty;
        public decimal fe_pesobrutoitem { get; set; }
        public string UnidadDescripcion { get; set; } = string.Empty;

    }
}
