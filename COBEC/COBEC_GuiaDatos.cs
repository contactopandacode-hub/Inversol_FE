using System;
using System.Collections.Generic;
using System.Text;

namespace COBE
{
      
    public class COBEC_GuiaDatos
    {
        public string SerieNumeroGuia { get; set; } = string.Empty;
        public DateTime FechaEmisionGuia { get; set; }
        public string TipoDocumentoGuia { get; set; } = string.Empty;
        public string CorreoAdquiriente { get; set; } = string.Empty;
        public string NumeroDocumentoRemitente { get; set; } = string.Empty;
        public string TipoDocumentoRemitente { get; set; } = string.Empty;
        public string RazonSocialRemitente { get; set; } = string.Empty;
        public string NumeroDocumentoDestinatario { get; set; } = string.Empty;
        public string TipoDocumentoDestinatario { get; set; } = string.Empty;
        public string RazonSocialDestinatario { get; set; } = string.Empty;
        public string NumeroDocumentoEstablecimiento { get; set; } = string.Empty;
        public string TipoDocumentoEstablecimiento { get; set; } = string.Empty;
        public string RazonSocialEstablecimiento { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public string NumeroDocumentoRelacionado { get; set; } = string.Empty;
        public string MotivoTraslado { get; set; } = string.Empty;
        public string DescripcionMotivoTraslado { get; set; } = string.Empty;
        public decimal PesoBrutoTotalBienes { get; set; }
        public string UnidadMedidaPesoBruto { get; set; } = string.Empty;
        public string ModalidadTraslado { get; set; } = string.Empty;
        public string FechaInicioTraslado { get; set; } = string.Empty;
        public string NumeroPlacaVehiculo { get; set; } = string.Empty;
        public decimal NumeroBulltos { get; set; }
        public string NumeroRucTransportista { get; set; } = string.Empty;
        public string TipoDocumentoTransportista { get; set; } = string.Empty;
        public string RazonSocialTransportista { get; set; } = string.Empty;
        public string NumeroDocumentoConductor { get; set; } = string.Empty;
        public string TipoDocumentoConductor { get; set; } = string.Empty;
        public string CodigoPuerto { get; set; } = string.Empty;
        public string UbigeoPtoLLegada { get; set; } = string.Empty;
        public string DireccionPtoLLegada { get; set; } = string.Empty;
        public string AlmacenLlegadaDistrito { get; set; } = string.Empty;
        public string AlmacenLlegadaProvincia { get; set; } = string.Empty;
        public string AlmacenLlegadaDepartamento { get; set; } = string.Empty;
        public string NumeroContenedor { get; set; } = string.Empty;
        public string UbigeoPtoPartida { get; set; } = string.Empty;
        public string DireccionPtoPartida { get; set; } = string.Empty;
        public string AlmacenPartidaDistrito { get; set; } = string.Empty;
        public string AlmacenPartidaProvincia { get; set; } = string.Empty;
        public string AlmacenPartidaDepartamento { get; set; } = string.Empty;
        public string DetalleGuiaItem { get; set; } = string.Empty;
        public string UbicacionXML { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string ReceptorCorreoElectronico { get; set; } = string.Empty;
        public string CodigoAutorizadoRem { get; set; } = string.Empty;
        public string NumeroAutorizacionRem { get; set; } = string.Empty;
        public string NumeroRegistroMTC { get; set; } = string.Empty;
        public string CodigoAutorizadoTrans { get; set; } = string.Empty;
        public string NumeroAutorizacionTrans { get; set; } = string.Empty;
        public string NombreConductor { get; set; } = string.Empty;
        public string ApellidoConductor { get; set; } = string.Empty;
        public string LicenciaConductor { get; set; } = string.Empty;
        public string NumeroDocumentoPtoLlegada { get; set; } = string.Empty;
        public string CodigoPtollegada { get; set; } = string.Empty;
        public string NumeroDocumentoPtoPartida { get; set; } = string.Empty;
        public string CodigoPtoPartida { get; set; } = string.Empty;
        public string Indicador { get; set; } = string.Empty;
        public long CantidadLineas { get; set; } = 0;
        public string EmpresaDepartamento { get; set; } = string.Empty;
        public string EmpresaProvincia { get; set; } = string.Empty;
        public string EmpresaDistrito { get; set; } = string.Empty;
        public string EmpresaUrbanizacion { get; set; } = string.Empty;
        public string DestinatarioDepartamento { get; set; } = string.Empty;
        public string DestinatarioProvincia { get; set; } = string.Empty;
        public string DestinatarioDistrito { get; set; } = string.Empty;
        public string DestinatarioUrbanizacion { get; set; } = string.Empty;
        public string ServicioUsuario { get; set; } = string.Empty;
        public string ServicioClave { get; set; } = string.Empty;

        public string EmisorUbigeo { get; set; } = string.Empty;
        public Int32 cantidadLineas { get; set; } = 0;  

    }
}

