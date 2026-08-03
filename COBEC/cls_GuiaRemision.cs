using System;
using System.Collections.Generic;
using System.Text;

namespace COBEC
{
    public class cls_GuiaRemision
    {
        // Punto de Partida

        public string ComprobanteGuiaTrama { get; set; }
        public string PuntoPartidaCodigoPais { get; set; } = "PE";
        public string PuntoPartidaDistrito { get; set; }
        public string PuntoPartidaProvincia { get; set; }
        public string PuntoPartidaDepartamento { get; set; }
        public string PuntoPartidaDireccion { get; set; }
        public string PuntoPartidaUbigeo { get; set; }
        public string PuntoPartidaUrbanizacion { get; set; } = "-";

        // Punto de Llegada
        public string PuntoLlegadaCodigoPais { get; set; } = "PE";
        public string PuntoLlegadaDistrito { get; set; }
        public string PuntoLlegadaProvincia { get; set; }
        public string PuntoLlegadaDepartamento { get; set; }
        public string PuntoLlegadaDireccion { get; set; }
        public string PuntoLlegadaUbigeo { get; set; }
        public string PuntoLlegadaUrbanizacion { get; set; } = "-";

        // Datos de Transporte
        public string GuiaModalidadTransporte { get; set; } = "02";
        public string GuiaVehiculoPlaca { get; set; }
        public string GuiaVehiculoConstanciaInscripcion { get; set; }
        public string GuiaVehiculoMarca { get; set; }
        public string GuiaNroLicencia { get; set; }
        public string GuiaTransportista { get; set; }
        public string GuiaTransportistaRuc { get; set; }
        public string GuiaNumeroDocumento { get; set; }
        public string GuiaCodigoTipoDocumento { get; set; } = "1";
        public string GuiaCodigoMotivoTraslado { get; set; } = "04";
        public string GuiaFechaInicioTraslado { get; set; }
    }
}
