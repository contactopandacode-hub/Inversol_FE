using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace ServicioRSNetCore.GuiaRemision
{
    public class EFACTRegistroGuia
    {
        public string RegistrarGuia(string str_serieNumeroGuia, DateTime dtm_fechaEmisionGuia, string str_tipoDocumentoGuia, string str_correoAdquiriente,
         string str_numeroDocumentoRemitente, string str_tipoDocumentoRemitente, string str_razonSocialRemitente, string str_numeroDocumentoDestinatario,
         string str_tipoDocumentoDestinatario, string str_razonSocialDestinatario, string str_numeroDocumentoEstablecimiento, string str_tipoDocumentoEstablecimiento,
         string str_razonSocialEstablecimiento, string str_observaciones,
         string str_numeroDocumentoRelacionado, string str_motivoTraslado, string str_descripcionMotivoTraslado,
         decimal dec_pesoBrutoTotalBienes, string str_unidadMedidaPesoBruto, string str_modalidadTraslado, string str_fechaInicioTraslado, string str_numeroPlacaVehiculo,
         decimal dec_numeroBulltos, string str_numeroRucTransportista, string str_tipoDocumentoTransportista, string str_razonSocialTransportista,
         string str_numeroDocumentoConductor, string str_tipoDocumentoConductor, string str_codigoPuerto, string str_ubigeoPtoLLegada, string str_direccionPtoLLegada,
         string str_numeroContenedor, string str_ubigeoPtoPartida, string str_direccionPtoPartida, string str_detalleGuiaItem, string str_UbicacionXML,
         string str_URL, string str_ReceptorCorreoElectronico,
         string str_codigoAutorizadoRem, string str_numeroAutorizacionRem, string str_numeroRegistroMTC, string str_codigoAutorizadoTrans,
         string str_numeroAutorizacionTrans, string str_nombreConductor, string str_apellidoConductor, string str_LicenciaConductor,
         string str_numeroDocumentoPtoLlegada, string str_codigoPtollegada, string str_numeroDocumentoPtoPartida, string str_codigoPtoPartida,
         string str_indicador, long ll_cantidadlinea, string str_EmpresaDepartamento, string str_EmpresaProvincia, string str_EmpresaDistrito, string str_EmpresaUrbanizacion,
         string str_DestinatarioDepartamento, string str_DestinatarioProvincia, string str_DestinatarioDistrito, string str_DestinatarioUrbanizacion,
         string str_AlmacenPartidaDistrito, string str_AlmacenPartidaProvincia, string str_AlmacenPartidaDepartamento, string str_AlmacenLlegadaDistrito, string str_AlmacenLlegadaProvincia,
         string str_AlmacenLlegadaDepartamento, string str_ServicioUsuario, string str_ServicioClave, string EmisorUbigeo)
        {
            string str_Resultado = string.Empty;
            StringBuilder stb_Resultado = new StringBuilder();
            StreamWriter sw_Documento = null;
            HttpWebRequest obj_Request = default;
            byte[] obj_FileByte = null;
            XmlDocument obj_XML = new XmlDocument();
            XmlNodeList obj_Respuesta = null;
            string str_status = string.Empty;
            string str_Documents = string.Empty;
            string str_separador = ",";
            StringBuilder stb_ResultadoDat = new StringBuilder();
            string str_CodigoTributario = string.Empty;
            string str_PesoSustento = string.Empty;
            string str_PesoAdicional = string.Empty;
            string str_NumeroAdicional = string.Empty;

            try
            {
                //Cabecera
                stb_Resultado.AppendFormat("UBLVersionID{0}", str_separador); //A1
                stb_ResultadoDat.AppendFormat("2.1{0}", str_separador);//A2

                stb_Resultado.AppendFormat("CustomizationID{0}", str_separador); //B1
                stb_ResultadoDat.AppendFormat("2.0{0}", str_separador);//B2

                stb_Resultado.AppendFormat("ID{0}", str_separador); //C1
                stb_ResultadoDat.AppendFormat("{0}{1}", str_serieNumeroGuia, str_separador); //C2

                stb_Resultado.AppendFormat("IssueDate{0}", str_separador); //D1
                stb_ResultadoDat.AppendFormat("{0}{1}", dtm_fechaEmisionGuia.ToString("yyyy-MM-dd"), str_separador); //D2

                stb_Resultado.AppendFormat("IssueTime{0}", str_separador); //E1
                stb_ResultadoDat.AppendFormat("{0}{1}", dtm_fechaEmisionGuia.ToString("HH:mm:ss"), str_separador); //D2

                stb_Resultado.AppendFormat("DespatchAdviceTypeCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_tipoDocumentoGuia, str_separador);

                stb_Resultado.AppendFormat("Note{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_observaciones.Trim()), str_separador);

                stb_Resultado.AppendFormat("LineCountNumeric{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", ll_cantidadlinea.ToString(), str_separador);


                if (string.IsNullOrEmpty(str_numeroDocumentoRelacionado) == false)
                {

                    //stb_Resultado.AppendFormat("<documentoRelacionado>");
                    string[] str_ListaDatos = str_numeroDocumentoRelacionado.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    stb_Resultado.AppendFormat("AdditionalDocumentReference/ID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_ListaDatos[2], str_separador);
                    str_NumeroAdicional = str_ListaDatos[2];

                    stb_Resultado.AppendFormat("AdditionalDocumentReference/DocumentTypeCode{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_ListaDatos[1], str_separador);

                    stb_Resultado.AppendFormat("AdditionalDocumentReference/DocumentType{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_ListaDatos[0]), str_separador);

                    //stb_Resultado.AppendFormat("AdditionalDocumentReference/IssuerParty/PartyIdentification/ID{0}", str_separador);
                    //stb_ResultadoDat.AppendFormat("{0}{1}", str_ListaDatos[3], str_separador);

                    //stb_Resultado.AppendFormat("AdditionalDocumentReference/IssuerParty/PartyIdentification/ID/@schemeID{0}", str_separador);
                    //stb_ResultadoDat.AppendFormat("{0}{1}", str_ListaDatos[4], str_separador);

                    str_PesoSustento = str_ListaDatos[5];
                    str_PesoAdicional = str_ListaDatos[6];
                    str_CodigoTributario = str_ListaDatos[7];
                }

                stb_Resultado.AppendFormat("Signature/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "IDSignKG", str_separador);

                stb_Resultado.AppendFormat("Signature/SignatoryParty/PartyIdentification/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroDocumentoRemitente, str_separador);

                stb_Resultado.AppendFormat("Signature/SignatoryParty/PartyName/Name{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_razonSocialRemitente), str_separador);

                stb_Resultado.AppendFormat("Signature/DigitalSignatureAttachment/ExternalReference/URI{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "#SignST", str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PartyIdentification/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroDocumentoRemitente, str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PartyIdentification/ID/@schemeID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_tipoDocumentoRemitente, str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PostalAddress/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", EmisorUbigeo, str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PostalAddress/StreetName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_direccionPtoPartida), str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PostalAddress/CitySubdivisionName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_EmpresaUrbanizacion, str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PostalAddress/CityName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_EmpresaDepartamento, str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PostalAddress/CountrySubentity{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_EmpresaProvincia, str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PostalAddress/District{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_EmpresaDistrito, str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PostalAddress/Country/IdentificationCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "PE", str_separador);

                stb_Resultado.AppendFormat("DespatchSupplierParty/Party/PartyLegalEntity/RegistrationName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_razonSocialRemitente), str_separador);


                //Datos Destinatario
                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PartyIdentification/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroDocumentoDestinatario.Trim(), str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PartyIdentification/ID/@schemeID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_tipoDocumentoDestinatario.Trim(), str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PostalAddress/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_ubigeoPtoLLegada, str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PostalAddress/StreetName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_direccionPtoLLegada), str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PostalAddress/CitySubdivisionName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_DestinatarioUrbanizacion, str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PostalAddress/CityName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_DestinatarioDepartamento, str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PostalAddress/CountrySubentity{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_DestinatarioDepartamento, str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PostalAddress/District{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_DestinatarioDistrito, str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PostalAddress/Country/IdentificationCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "PE", str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/PartyLegalEntity/RegistrationName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_razonSocialDestinatario.Trim()), str_separador);

                stb_Resultado.AppendFormat("DeliveryCustomerParty/Party/Contact/ElectronicMail{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_ReceptorCorreoElectronico, str_separador);


                stb_Resultado.AppendFormat("Shipment/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "SUNAT_Envio", str_separador);

                stb_Resultado.AppendFormat("Shipment/HandlingCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_motivoTraslado, str_separador);

                stb_Resultado.AppendFormat("Shipment/HandlingInstructions{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_descripcionMotivoTraslado), str_separador);

                if (string.IsNullOrEmpty(str_numeroDocumentoRelacionado) == false)
                {
                    stb_Resultado.AppendFormat("Shipment/Information{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_PesoSustento, str_separador);
                }

                stb_Resultado.AppendFormat("Shipment/GrossWeightMeasure{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", dec_pesoBrutoTotalBienes.ToString(), str_separador);

                stb_Resultado.AppendFormat("Shipment/GrossWeightMeasure/@unitCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_unidadMedidaPesoBruto, str_separador);

                if (string.IsNullOrEmpty(str_numeroDocumentoRelacionado) == false)
                {
                    stb_Resultado.AppendFormat("Shipment/NetWeightMeasure{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_PesoSustento, str_separador);

                    stb_Resultado.AppendFormat("Shipment/NetWeightMeasure/@unitCode{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_unidadMedidaPesoBruto, str_separador);

                    stb_Resultado.AppendFormat("Shipment/TotalTransportHandlingUnitQuantity{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", dec_numeroBulltos.ToString("F2"), str_separador);
                }

                stb_Resultado.AppendFormat("Shipment/ShipmentStage/TransportModeCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_modalidadTraslado, str_separador);

                stb_Resultado.AppendFormat("Shipment/ShipmentStage/TransitPeriod/StartDate{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_fechaInicioTraslado, str_separador);

                if (str_modalidadTraslado == "01")
                {
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/CarrierParty/PartyIdentification/ID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroRucTransportista, str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/CarrierParty/PartyIdentification/ID/@schemeID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_tipoDocumentoTransportista, str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/CarrierParty/PartyLegalEntity/RegistrationName{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_razonSocialTransportista), str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/CarrierParty/PartyLegalEntity/CompanyID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_numeroRegistroMTC), str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/CarrierParty/AgentParty/PartyLegalEntity/CompanyID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_numeroAutorizacionTrans), str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/CarrierParty/AgentParty/PartyLegalEntity/CompanyID/@schemeID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_codigoAutorizadoTrans), str_separador);
                }
                else
                {
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/DriverPerson/ID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroDocumentoConductor.Trim(), str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/DriverPerson/ID/@schemeID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_tipoDocumentoConductor, str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/DriverPerson/FirstName{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_nombreConductor.Trim()), str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/DriverPerson/FamilyName{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_apellidoConductor.Trim()), str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/DriverPerson/JobTitle{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", "Principal", str_separador);
                    stb_Resultado.AppendFormat("Shipment/ShipmentStage/DriverPerson/IdentityDocumentReference/ID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_LicenciaConductor.Trim(), str_separador);
                }


                stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_ubigeoPtoLLegada, str_separador);

                if (str_motivoTraslado == "04")
                {
                    stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/AddressTypeCode{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_codigoPtollegada.Trim(), str_separador);

                    stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/AddressTypeCode/@listID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroDocumentoPtoLlegada.Trim(), str_separador);

                    //stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/LocationCoordinate/LatitudeDegreesMeasure{0}", str_separador);
                    //stb_ResultadoDat.AppendFormat("{0}{1}", "12.44", str_separador);

                    //stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/LocationCoordinate/LongitudeDegreesMeasure{0}", str_separador);
                    //stb_ResultadoDat.AppendFormat("{0}{1}", "134.00", str_separador);

                }

                stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/CitySubdivisionName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "", str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/CityName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_AlmacenLlegadaProvincia, str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/CountrySubentity{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_AlmacenLlegadaDepartamento, str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/District{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_AlmacenLlegadaDistrito, str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/AddressLine/Line{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_direccionPtoLLegada), str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/DeliveryAddress/Country/IdentificationCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "PE", str_separador);



                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/ID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_ubigeoPtoPartida, str_separador);

                if (str_motivoTraslado == "04")
                {
                    stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/AddressTypeCode{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_codigoPtoPartida.Trim(), str_separador);

                    stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/AddressTypeCode/@listID{0}", str_separador);
                    stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroDocumentoPtoPartida.Trim(), str_separador);

                    //stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/LocationCoordinate/LatitudeDegreesMeasure{0}", str_separador);
                    //stb_ResultadoDat.AppendFormat("{0}{1}", "12.44", str_separador);

                    //stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/LocationCoordinate/LongitudeDegreesMeasure{0}", str_separador);
                    //stb_ResultadoDat.AppendFormat("{0}{1}", "134.00", str_separador);

                }

                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/CitySubdivisionName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "", str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/CityName{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_AlmacenPartidaProvincia, str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/CountrySubentity{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_AlmacenPartidaDepartamento, str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/District{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_AlmacenPartidaDistrito, str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/AddressLine/Line{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_direccionPtoPartida), str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchAddress/Country/IdentificationCode{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", "PE", str_separador);
                
                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchParty/AgentParty/PartyLegalEntity/CompanyID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroAutorizacionTrans.Trim(), str_separador);

                stb_Resultado.AppendFormat("Shipment/Delivery/Despatch/DespatchParty/AgentParty/PartyLegalEntity/CompanyID/@schemeID{0}", str_separador);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_codigoAutorizadoTrans.Trim(), str_separador);

                stb_Resultado.AppendFormat("Shipment/TransportHandlingUnit/TransportEquipment/ID{0}{1}", str_separador, Environment.NewLine);
                stb_ResultadoDat.AppendFormat("{0}{1}", str_numeroPlacaVehiculo.Trim(), str_separador);


                stb_Resultado.AppendFormat("{0}{1}", stb_ResultadoDat, Environment.NewLine);

                stb_Resultado.AppendFormat("DespatchLine/ID{0}", str_separador);
                stb_Resultado.AppendFormat("DespatchLine/Note{0}", str_separador);
                stb_Resultado.AppendFormat("DespatchLine/DeliveredQuantity{0}", str_separador);
                stb_Resultado.AppendFormat("DespatchLine/DeliveredQuantity/@unitCode{0}", str_separador);
                stb_Resultado.AppendFormat("DespatchLine/OrderLineReference/LineID{0}", str_separador);
                stb_Resultado.AppendFormat("DespatchLine/Item/Description{0}", str_separador);
                stb_Resultado.AppendFormat("DespatchLine/Item/SellersItemIdentification/ID{0}{1}", str_separador, Environment.NewLine);


                string[] str_ListaGuiaItem = str_detalleGuiaItem.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_ListaGuiaItem.Length > 0)
                {
                    for (int int_Fila = 0; int_Fila < str_ListaGuiaItem.Length; int_Fila++)
                    {
                        string[] str_RegistroGuiaItem = str_ListaGuiaItem[int_Fila].Split(Convert.ToChar("|"));

                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroGuiaItem[0], str_separador);
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroGuiaItem[5], str_separador);
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroGuiaItem[1], str_separador);
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroGuiaItem[2], str_separador);
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroGuiaItem[0], str_separador);
                        stb_Resultado.AppendFormat("{0}{1}", CambiarCaracterEspecial(str_RegistroGuiaItem[3]), str_separador);
                        stb_Resultado.AppendFormat("{0}{1}", str_RegistroGuiaItem[4], str_separador);
                        if (string.IsNullOrEmpty(str_numeroDocumentoRelacionado) == false)
                        {
                            stb_Resultado.AppendFormat("{0}{1}", "Numero de declaracion aduanera (DAM)", str_separador);
                            stb_Resultado.AppendFormat("{0}{1}", str_CodigoTributario, str_separador);
                            stb_Resultado.AppendFormat("{0}{1}", str_NumeroAdicional, str_separador);
                        }
                        stb_Resultado.AppendFormat("{0}", Environment.NewLine);
                    }
                }

                //Solo para ambiente de pruebas                
                if (str_UbicacionXML.Trim() != string.Empty)
                {
                    sw_Documento = new StreamWriter(str_UbicacionXML + str_numeroDocumentoRemitente + "-" + str_tipoDocumentoGuia + "-" + str_serieNumeroGuia + ".csv");
                    sw_Documento.Write(stb_Resultado.ToString());
                    sw_Documento.Flush();
                    sw_Documento.Close();
                }

                using (var obj_Cliente = new HttpClient())
                {
                    string str_ServicioUsuarioClave = "client" + ":" + "secret";
                    byte[] byt_UsuarioClave = Encoding.UTF8.GetBytes(str_ServicioUsuarioClave.ToCharArray());

                    obj_Cliente.DefaultRequestHeaders.Accept.Clear();
                    obj_Cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    obj_Cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byt_UsuarioClave));

                    var obj_Parametros = new Dictionary<string, string>();
                    obj_Parametros.Add("grant_type", "password");
                    obj_Parametros.Add("username", str_ServicioUsuario);
                    obj_Parametros.Add("password", str_ServicioClave);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var obj_Response = obj_Cliente.PostAsync(str_URL + "/oauth/token", new FormUrlEncodedContent(obj_Parametros)).Result;

                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;
                    var obj_Resultado = JObject.Parse(str_Resultado);

                    if (obj_Resultado["error"] != null)
                    {
                        str_Resultado = "01|" + obj_Resultado["error"].ToString() + "-" + obj_Resultado["error_description"].ToString();
                        return str_Resultado;
                    }

                    string str_Token = obj_Resultado["access_token"].ToString();

                    obj_Cliente.DefaultRequestHeaders.Clear();
                    var frm_DatosEnviar = new MultipartFormDataContent();

                    Stream Stream = File.OpenRead(str_UbicacionXML + str_numeroDocumentoRemitente + "-" + str_tipoDocumentoGuia + "-" + str_serieNumeroGuia + ".csv");

                    var obj_ArchivoCSV = new StreamContent(Stream);
                    obj_ArchivoCSV.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
                    obj_ArchivoCSV.Headers.ContentDisposition.Name = "file";
                    obj_ArchivoCSV.Headers.ContentDisposition.FileName = str_numeroDocumentoRemitente + "-" + str_tipoDocumentoGuia + "-" + str_serieNumeroGuia + ".csv";

                    frm_DatosEnviar.Add(obj_ArchivoCSV);

                    obj_Cliente.DefaultRequestHeaders.Add("Authorization", "bearer " + str_Token);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    obj_Response = obj_Cliente.PostAsync(str_URL + "/v1/document", frm_DatosEnviar).Result;
                    str_Resultado = obj_Response.Content.ReadAsStringAsync().Result;

                    obj_Resultado = JObject.Parse(str_Resultado);

                    if (obj_Resultado["code"] != null && obj_Resultado["code"].ToString() == "0")
                    {
                        str_Token = obj_Resultado["description"].ToString();
                        str_Resultado = "00|EN|" + str_Token ;
                    }
                    else if (obj_Resultado["description"] != null)
                    {
                        str_Resultado = "01|PE|Error en Registro:" + obj_Resultado["description"].ToString();
                    }
                    else
                    {
                        str_Resultado = "01|PE|Error en Registro:" + obj_Resultado["error"].ToString();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                str_Resultado = "01|" + ex.Message.ToString();
            }
            catch (WebException ex)
            {
                str_Resultado = "01|" + ex.Message.ToString();
            }
            catch (Exception ex)
            {
                str_Resultado = "01|" + ex.Message.ToString();
            }
            finally
            {
                if (sw_Documento != null)
                    sw_Documento.Dispose();
                stb_Resultado = null;
                sw_Documento = null;
                obj_XML = null;
            }
            return str_Resultado;
        }

        private static string CambiarCaracterEspecial(string str_Texto)
        {
            //.Replace("&", "&#38;")
            return str_Texto.Replace("&", "Y").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&#39;").Replace("\"", "&quot;").Replace("�", "&#225;").Replace("�", "&#233;").Replace("�", "&#237;").Replace("�", "&#243;").Replace("�", "&#250;").Replace("�", "&#193;").Replace("�", "&#201;").Replace("�", "&#205;").Replace("�", "&#211;").Replace("�", "&#218;").Replace("�", "&#186;").Replace("�", "&#176;").Replace("�", "&#241;").Replace("�", "&#209;").Replace("\n", string.Empty).Replace("\u00A0", " ").Replace(",", string.Empty).Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U");
        }
    }
}
