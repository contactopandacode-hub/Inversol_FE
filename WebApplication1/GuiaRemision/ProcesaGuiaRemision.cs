using COBE;
using COBEC;
using CODAT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace ServicioRSNetCore.GuiaRemision
{
    public class ProcesaGuiaRemision : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;

        public ProcesaGuiaRemision(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }
        public COBEc_Error Registrar(COBEC_Guia obj_InfoFe)
        {
            var obj_respuesta = new COBEc_Error();
            string str_Resultado = string.Empty;                      
            CODAT_ComprobanteRegistrar obj_datos = new CODAT_ComprobanteRegistrar(configuration, this.context);
            RegistrarComprobanteRequest obj_compania = new RegistrarComprobanteRequest();
            COBEC_GuiaDatos obj_guia = new COBEC_GuiaDatos();
            try
            {
                obj_compania.companiaSocio = obj_InfoFe.companiaSocio;
                DatosCompania compania = obj_datos.DatosCompania(obj_compania);
                cls_GuiaDatos cls_GuiaDatos = obj_datos.GuiaDatos(obj_InfoFe);
                List<cls_GuiaDetalle> cls_GuiaDetalle = obj_datos.GuiaDatosDetalle(obj_InfoFe);

                if (cls_GuiaDatos != null)
                {
                    obj_guia.SerieNumeroGuia = obj_InfoFe.serieNumero + "-" + Convert.ToInt32(obj_InfoFe.guiaNumero).ToString("00000000");
                    obj_guia.FechaEmisionGuia = Convert.ToDateTime(cls_GuiaDatos.FechaDocumento);
                    obj_guia.TipoDocumentoGuia = "09";
                    obj_guia.CorreoAdquiriente = compania.CorreoElectronico;
                    obj_guia.NumeroDocumentoRemitente = compania.DocumentoFiscal;
                    obj_guia.EmisorUbigeo = compania.Ubigeo;
                    obj_guia.EmpresaDepartamento = compania.Departamento;
                    obj_guia.EmpresaProvincia = compania.Provincia;
                    obj_guia.EmpresaDistrito= compania.Distrito;
                    obj_guia.EmpresaUrbanizacion = "-";
                    obj_guia.TipoDocumentoRemitente = "6";
                    obj_guia.RazonSocialRemitente = compania.DescripcionLarga;
                    obj_guia.NumeroDocumentoDestinatario = cls_GuiaDatos.DestinatarioRuC;
                    obj_guia.TipoDocumentoDestinatario = cls_GuiaDatos.DestinatarioTipoDocumento;
                    obj_guia.RazonSocialDestinatario = cls_GuiaDatos.DestinatarioNombre;
                    obj_guia.Observaciones = cls_GuiaDatos.Comentarios;
                    obj_guia.MotivoTraslado = cls_GuiaDatos.FE_TrasladoCodigo;
                    obj_guia.DescripcionMotivoTraslado = cls_GuiaDatos.FE_TrasladoMotivo;
                    obj_guia.FechaInicioTraslado = Convert.ToDateTime(cls_GuiaDatos.fechainiciotraslado).ToString("yyyy-MM-dd");
                    obj_guia.NumeroPlacaVehiculo =  cls_GuiaDatos.transportistaplaca.Trim();
                    obj_guia.NumeroBulltos = 0;//cls_GuiaDatos.bul Convert.ToDecimal(dt_Guia.Rows[0]["nrobultos"]);
                    obj_guia.UbigeoPtoPartida = cls_GuiaDatos.AlmacenUbigeo;
                    obj_guia.DireccionPtoPartida = cls_GuiaDatos.AlmacenDireccion       ;
                    obj_guia.UnidadMedidaPesoBruto = "KGM";
                    obj_guia.Indicador = cls_GuiaDatos.indicadorservicio;
                    obj_guia.URL = cls_GuiaDatos.Url;
                    obj_guia.UbicacionXML = configuration["RutaAdjuntos"]; ;
                    obj_guia.ReceptorCorreoElectronico =  cls_GuiaDatos.CorreoElectronico;
                    obj_guia.ModalidadTraslado = cls_GuiaDatos.fe_modalidadguia;

                    obj_guia.ServicioUsuario = compania.DocumentoFiscal;
                    obj_guia.ServicioClave = compania.URLPassword;
                    // Traslado entre establecimiento
                    if (cls_GuiaDatos.FE_TrasladoCodigo.Trim() == "04")
                    {
                        obj_guia.NumeroDocumentoPtoPartida = cls_GuiaDatos.DestinatarioRuC;
                        obj_guia.CodigoPtoPartida = cls_GuiaDatos.CodigoEstablecimientoPartida.Trim();

                        obj_guia.DireccionPtoLLegada = cls_GuiaDatos.AlmacenDireccionLlegada.Trim();
                        obj_guia.UbigeoPtoLLegada = cls_GuiaDatos.AlmacenUbigeoLlegada.Trim();
                        obj_guia.CodigoPtollegada = cls_GuiaDatos.CodigoEstablecimientoLlegada.Trim();
                        obj_guia.NumeroDocumentoPtoLlegada = cls_GuiaDatos.DestinatarioRuC.Trim();
                    }
                    else
                    {
                       cls_GuiaPuntaLlegada cls_GuiaPuntaLlegada  = obj_datos.GuiaPuntoLlegada(
                            Convert.ToInt32(cls_GuiaDatos.Destinatario  ),
                            Convert.ToInt32(cls_GuiaDatos.DestinatarioDireccionSecuencia));

                        obj_guia.DireccionPtoLLegada = cls_GuiaPuntaLlegada.direccion;
                        obj_guia.UbigeoPtoLLegada = cls_GuiaPuntaLlegada.Ubigeo;
                    }

                    if (cls_GuiaDatos.fe_modalidadguia.Trim() == "01")
                    {
                        obj_guia.NumeroRucTransportista = cls_GuiaDatos.transportistaruc;

                        if (obj_guia.NumeroRucTransportista.Length == 11)
                        {
                            obj_guia.TipoDocumentoTransportista = "6";
                        }
                        else
                        {
                            obj_guia.TipoDocumentoTransportista = "1";
                        }

                        obj_guia.RazonSocialTransportista = cls_GuiaDatos.transportistanombre;

                        if (obj_guia.Indicador != "06")
                        {
                            obj_guia.NumeroPlacaVehiculo = string.Empty;
                        }
                    }

                    if (cls_GuiaDatos .fe_modalidadguia == "02" || cls_GuiaDatos.indicadorservicio == "06")
                    {
                        obj_guia.LicenciaConductor = cls_GuiaDatos.TransportistaBrevete.Trim();
                        obj_guia.NumeroDocumentoConductor = cls_GuiaDatos.TransportistaDocumento.Trim();
                        obj_guia.NombreConductor = cls_GuiaDatos.ChoferNombre.Trim();
                        obj_guia.ApellidoConductor = cls_GuiaDatos.ChoferApellido.Trim();

                        if (obj_guia.NumeroDocumentoConductor.Length == 8)
                        {
                            obj_guia.TipoDocumentoConductor = "1";
                        }
                        else
                        {
                            obj_guia.TipoDocumentoConductor = "6";
                        }
                    }

                    if (cls_GuiaDatos.FE_TrasladoCodigo == "02"
                        && Convert.ToInt32(cls_GuiaDatos.FE_TrasladoCodigo) > 0)
                    {
                        obj_guia.NumeroDocumentoEstablecimiento = cls_GuiaDatos.ProveedorNumeroDocumento;
                        obj_guia.TipoDocumentoEstablecimiento = cls_GuiaDatos.ProveedorTipoDocumento;
                        obj_guia.RazonSocialEstablecimiento = cls_GuiaDatos.ProveedorRazonSocial;
                        obj_guia.UbigeoPtoPartida = cls_GuiaDatos.ProveedorUbigeo;
                        obj_guia.DireccionPtoPartida = cls_GuiaDatos.ProveedorDireccion;
                    }

                    // Documento Relacionado
                    string facturaNumero = cls_GuiaDatos.FacturaNumero;
                    if (facturaNumero.Length > 0 && (facturaNumero.Substring(0, 1) == "F" || facturaNumero.Substring(0, 1) == "B"))
                    {
                        // VB "Mid(s, 3)" es 1-based (desde el 3er caracter) -> C# Substring(2)
                        string str_faturaNumero = facturaNumero.Substring(2);

                        if (str_faturaNumero.Length > 0 && str_faturaNumero.Substring(0, 1) == "-")
                        {
                            // VB "Mid(s, 2)" -> C# Substring(1)
                            str_faturaNumero = str_faturaNumero.Substring(1);
                        }

                        long ll_posicion = str_faturaNumero.IndexOf("-");

                        str_faturaNumero = str_faturaNumero.Substring(0, 5)
                            + Convert.ToUInt32(str_faturaNumero.Substring((int)ll_posicion + 1)).ToString("00000000");

                        obj_guia.NumeroDocumentoRelacionado =
                            cls_GuiaDatos.DescripcionDocumentoRelacionado + "|" +
                            cls_GuiaDatos.tipoDocumentoRelacionado + "|" +
                            str_faturaNumero + "|" +
                            obj_guia.NumeroDocumentoRemitente + "|" +
                            "6";
                    }

                    if (cls_GuiaDetalle != null)
                    {
                        for (int ll_InicioDetalle = 0; ll_InicioDetalle <= cls_GuiaDetalle.Count - 1; ll_InicioDetalle++)
                        {
                            obj_guia.cantidadLineas= cls_GuiaDetalle.Count;

                            string str_descripcion = cls_GuiaDetalle[ll_InicioDetalle].Descripcion.Trim();   
                            if (cls_GuiaDetalle[ll_InicioDetalle].Marca.Trim() != "-")
                            {
                                str_descripcion += " - Marca:" + cls_GuiaDetalle[ll_InicioDetalle].Marca.Trim();
                            }

                            obj_guia.DetalleGuiaItem +=
                                (ll_InicioDetalle + 1).ToString("000") + "|" +
                                Convert.ToDecimal(cls_GuiaDetalle[ll_InicioDetalle].cantidad).ToString() + "|" +
                                cls_GuiaDetalle[ll_InicioDetalle].unidadFE.Trim() + "|" +
                                str_descripcion + "|" +
                                cls_GuiaDetalle[ll_InicioDetalle].ItemCodigo.Trim() + "|"+
                                cls_GuiaDetalle[ll_InicioDetalle].UnidadDescripcion.Trim() + "||";

                            obj_guia.PesoBrutoTotalBienes += Math.Round(
                                Convert.ToDecimal(cls_GuiaDetalle[ll_InicioDetalle].fe_pesobrutoitem) *
                                Convert.ToDecimal(cls_GuiaDetalle[ll_InicioDetalle].cantidad), 3);
                        }
                    }
                }

                // Valida Placa
                bool lb_placa = false;
                if (cls_GuiaDatos.indicadorservicio != "06")
                {
                    // sin acción (igual que el VB original)
                }
                else
                {
                    if (string.IsNullOrEmpty(cls_GuiaDatos.transportistaplaca) == false)
                    {
                        if (ValidarPlaca(cls_GuiaDatos.transportistaplaca) == false)
                        {
                            obj_respuesta.codigo = "1";
                            obj_respuesta.mensaje = "El formato de la placa es incorrecto. Validar el dato ingresado.";
                            return obj_respuesta;
                        }
                    }
                }

                if (cls_GuiaDatos.indicadorservicio != "06")
                {
                    // sin acción (igual que el VB original)
                }
                else
                {
                    // Bloque comentado también en el original VB:
                    // if (string.IsNullOrEmpty(obj_guia.NumeroPlacaVehiculo) == true && obj_guia.ModalidadTraslado == "01")
                    // {
                    //     obj_respuesta.Codigo = "1";
                    //     obj_respuesta.Mensaje = "Debe ingresar la placa para continuar con el registro.";
                    //     return obj_respuesta;
                    // }
                }

                if (compania.DocumentoFiscal == cls_GuiaDatos.DestinatarioRuC)
                {
                    if (cls_GuiaDatos.FE_TrasladoCodigo != "04")
                    {
                        obj_respuesta.codigo = "1";
                        obj_respuesta.mensaje = "Los datos del destinatario y remitente similares tan solo se permiten para el motivo de traslado entre establecimientos. Verificar.";
                        return obj_respuesta;
                    }
                }
                              

                if (cls_GuiaDatos.indicadorservicio == "02" && cls_GuiaDatos.FE_TrasladoMotivo == "02")
                {
                    obj_guia.LicenciaConductor =string.Empty;
                    obj_guia.NumeroDocumentoConductor = string.Empty;
                    obj_guia.NombreConductor = string.Empty;
                    obj_guia.ApellidoConductor = string.Empty;
                    obj_guia.TipoDocumentoConductor = string.Empty;
                    obj_guia.NumeroPlacaVehiculo = string.Empty;
                }

                var efactRegistro = new EFACTRegistroGuia();
                str_Resultado = efactRegistro.RegistrarGuia(obj_guia.SerieNumeroGuia,
                                                           obj_guia.FechaEmisionGuia,
                                                           obj_guia.TipoDocumentoGuia,
                                                           obj_guia.CorreoAdquiriente,
                                                           obj_guia.NumeroDocumentoRemitente,
                                                           obj_guia.TipoDocumentoRemitente,
                                                           obj_guia.RazonSocialRemitente,
                                                           obj_guia.NumeroDocumentoDestinatario,
                                                           obj_guia.TipoDocumentoDestinatario,
                                                           obj_guia.RazonSocialDestinatario,
                                                           obj_guia.NumeroDocumentoEstablecimiento,
                                                           obj_guia.TipoDocumentoEstablecimiento,
                                                           obj_guia.RazonSocialEstablecimiento,
                                                           obj_guia.Observaciones,
                                                           obj_guia.NumeroDocumentoRelacionado,
                                                           obj_guia.MotivoTraslado,
                                                           obj_guia.DescripcionMotivoTraslado,
                                                           obj_guia.PesoBrutoTotalBienes,
                                                           obj_guia.UnidadMedidaPesoBruto,
                                                           obj_guia.ModalidadTraslado,
                                                           obj_guia.FechaInicioTraslado,
                                                           obj_guia.NumeroPlacaVehiculo,
                                                           obj_guia.NumeroBulltos,
                                                           obj_guia.NumeroRucTransportista,
                                                           obj_guia.TipoDocumentoTransportista,
                                                           obj_guia.RazonSocialTransportista,
                                                           obj_guia.NumeroDocumentoConductor,
                                                           obj_guia.TipoDocumentoConductor,
                                                           obj_guia.CodigoPuerto,
                                                           obj_guia.UbigeoPtoLLegada,
                                                           obj_guia.DireccionPtoLLegada,
                                                           obj_guia.NumeroContenedor,
                                                           obj_guia.UbigeoPtoPartida,
                                                           obj_guia.DireccionPtoPartida,
                                                           obj_guia.DetalleGuiaItem,
                                                           obj_guia.UbicacionXML,
                                                           obj_guia.URL,
                                                           obj_guia.ReceptorCorreoElectronico,
                                                           obj_guia.CodigoAutorizadoRem,
                                                           obj_guia.NumeroAutorizacionRem,
                                                           obj_guia.NumeroRegistroMTC,
                                                           obj_guia.CodigoAutorizadoTrans,
                                                           obj_guia.NumeroAutorizacionTrans,
                                                           obj_guia.NombreConductor,
                                                           obj_guia.ApellidoConductor,
                                                           obj_guia.LicenciaConductor,
                                                           obj_guia.NumeroDocumentoPtoLlegada,
                                                           obj_guia.CodigoPtollegada,
                                                           obj_guia.NumeroDocumentoPtoPartida,
                                                           obj_guia.CodigoPtoPartida,
                                                           obj_guia.Indicador,
                                                           obj_guia.cantidadLineas,
                                                           obj_guia.EmpresaDepartamento,
                                                           obj_guia.EmpresaProvincia,
                                                           obj_guia.EmpresaDistrito,
                                                           obj_guia.EmpresaUrbanizacion,
                                                           obj_guia.DestinatarioDepartamento,
                                                           obj_guia.DestinatarioProvincia,
                                                           obj_guia.DestinatarioDistrito,
                                                           obj_guia.DestinatarioUrbanizacion,
                                                           obj_guia.AlmacenPartidaDistrito,
                                                           obj_guia.AlmacenPartidaProvincia,
                                                           obj_guia.AlmacenPartidaDepartamento,
                                                           obj_guia.AlmacenLlegadaDistrito,
                                                           obj_guia.AlmacenLlegadaProvincia,
                                                           obj_guia.AlmacenLlegadaDepartamento,
                                                           obj_guia.ServicioUsuario,
                                                           obj_guia.ServicioClave, obj_guia.EmisorUbigeo);

                string[] str_ListaResultado = str_Resultado.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                if (str_ListaResultado[0] == "00")
                {
                    //obj_Datos.GuiaActualizarDatos(obj_InfoFe, str_Resultado);
                    obj_datos.GuiaActualizarDatos(obj_InfoFe, "1", str_ListaResultado[1],"Procesado", string.Empty, str_ListaResultado[2]);
                    obj_respuesta.codigo = "00";
                    obj_respuesta.mensaje = "Registrado con codigo Hash:" + str_ListaResultado[2];
                }
                else
                {
                    obj_datos.GuiaActualizarDatos(obj_InfoFe, "2", str_ListaResultado[1], string.Empty, str_ListaResultado[2], string.Empty);
                    obj_respuesta.codigo = "01";
                    obj_respuesta.mensaje = str_ListaResultado[2];
                }
            }
            catch (Exception ex)
            {
                obj_respuesta.codigo = "01";
                obj_respuesta.mensaje = "Error: " + ex.Message;
                return obj_respuesta;
            }

            return obj_respuesta;
        }

        private bool ValidarPlaca(string placa)
        {
            // Patrón que permite solo letras (mayúsculas y minúsculas) y números
            string formatoPlaca = "^[A-Za-z0-9]+$";

            // Verificar que la placa contenga solo letras y números
            bool esValido = System.Text.RegularExpressions.Regex.IsMatch(placa, formatoPlaca);

            // Verificar si contiene al menos una letra y un número
            bool tieneLetra = System.Text.RegularExpressions.Regex.IsMatch(placa, "[A-Za-z]");
            bool tieneNumero = System.Text.RegularExpressions.Regex.IsMatch(placa, "\\d");

            // Retorna verdadero solo si cumple ambas condiciones
            return esValido && tieneLetra && tieneNumero;
        }
    }
}
