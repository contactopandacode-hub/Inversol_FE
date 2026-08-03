using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{
    public class cls_ComunicadoBaja
    {
        public string rucEmisor { get; set; }
        public Int32 resumenId { get; set; }       
        public DateTime fechaEmision { get; set; }
        public string razonSocial { get; set; }
        public string tipoDocumento { get; set; }
        public string serieNumeroDocumento { get; set; }
        public string motivoBaja { get; set; }
        public string rutaXml { get; set; }
        public string URLUsuario { get; set; }
        public string URLPassword { get; set; }
        public string urlWebService { get; set; }

    }
}
