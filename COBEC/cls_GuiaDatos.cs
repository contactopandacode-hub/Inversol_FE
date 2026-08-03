using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{
    public class cls_GuiaDatos
    {
        public string SerieNumero { get; set; } = string.Empty;
        public string GuiaNumero { get; set; } = string.Empty;
        public string tipoDocumentoRelacionado { get; set; } = string.Empty;
        public string DescripcionDocumentoRelacionado { get; set; } = string.Empty;
        public string ChoferNombre { get; set; } = string.Empty;
        public string ChoferApellido { get; set; } = string.Empty;
        public string RutaXml { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string FE_TrasladoCodigo { get; set; } = string.Empty;
        public string FE_TrasladoMotivo { get; set; } = string.Empty;
        public string fe_modalidadguia { get; set; } = string.Empty;
        public DateTime FechaDocumento { get; set; }
        public DateTime fechainiciotraslado { get; set; }
        public string DestinatarioRuC { get; set; } = string.Empty;
        public string DestinatarioDireccionFiscal { get; set; } = string.Empty;
        public int Destinatario { get; set; }
        public string transportistaplaca { get; set; } = string.Empty;
        public string DestinatarioNombre { get; set; } = string.Empty;
        public string DestinatarioDireccion { get; set; } = string.Empty;
        public string transportistachofer { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string TransportistaTipoDocumento { get; set; } = string.Empty;
        public string DestinatarioTipoDocumento { get; set; } = string.Empty;

      
        public string ProveedorTipoDocumento { get; set; } = string.Empty;

        public string Comentarios { get; set; } = string.Empty;
        public int DestinatarioDireccionSecuencia { get; set; }
        public string AlmacenUbigeo { get; set; } = string.Empty;
        public string AlmacenDireccion { get; set; } = string.Empty;
        public string CodigoEstablecimientoPartida { get; set; } = string.Empty;
        public int proveedorcodigo { get; set; }
        public string ProveedorNumeroDocumento { get; set; } = string.Empty;
        public string ProveedorRazonSocial { get; set; } = string.Empty;
        public string TipoDocumentoAdicional { get; set; } = string.Empty;
        public string NumeroDocumentoAdicional { get; set; } = string.Empty;
        public string referencianumeropedido { get; set; } = string.Empty;
        public string motivotraslado { get; set; } = string.Empty;
        public decimal nrobultos { get; set; }
        public string AlmacenUbigeoLlegada { get; set; } = string.Empty;
        public string AlmacenDireccionLlegada { get; set; } = string.Empty;
        public string CodigoEstablecimientoLlegada { get; set; } = string.Empty;
        public string indicadorservicio { get; set; } = string.Empty;
        public string DescripcionDocumentoAdicional { get; set; } = string.Empty;
        public string FacturaNumero { get; set; } = string.Empty;
        public string NumeroMTC { get; set; } = string.Empty;
        public string NumeroAutorizacion { get; set; } = string.Empty;
        public string CodigoAutorizacion { get; set; } = string.Empty;
        public string transportistaruc { get; set; } = string.Empty;
        public string transportistanombre { get; set; } = string.Empty;
        public string ProveedorDireccion { get; set; } = string.Empty;
        public string TransportistaBrevete { get; set; } = string.Empty;
        public string TransportistaDocumento { get; set; } = string.Empty;
        public string ProveedorCodigoEstablecimiento { get; set; } = string.Empty;
        public string ProveedorUbigeo { get; set; } = string.Empty;
    }
}
