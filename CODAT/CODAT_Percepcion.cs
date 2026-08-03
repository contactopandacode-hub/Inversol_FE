using COBE;
using COBEC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Text;

namespace CODAT
{
    public class CODAT_Percepcion : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;
        public CODAT_Percepcion(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public CompaniaInfo DatosCompania(COBEC_Percepcion cOBEC_Percepcion)
        {
            CompaniaInfo companiaInfo = new CompaniaInfo();
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_WSFE_ConsultaCompania";
                _command.Connection.Open();

                SqlParameter p_compania = new SqlParameter("@pCompania", cOBEC_Percepcion.companiaSocio);
                _command.Parameters.Add(p_compania);

                DbDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    companiaInfo = new CompaniaInfo
                    {
                        DescripcionLarga = (reader["DescripcionLarga"] as string ?? string.Empty).Trim(),
                        DescripcionCorta = (reader["DescripcionCorta"] as string ?? string.Empty).Trim(),
                        DireccionComun = (reader["DireccionComun"] as string ?? string.Empty).Trim(),
                        DireccionAdicional = (reader["DireccionAdicional"] as string ?? string.Empty).Trim(),
                        DocumentoFiscal = (reader["DocumentoFiscal"] as string ?? string.Empty).Trim(),
                        Distrito = (reader["Distrito"] as string ?? string.Empty).Trim(),
                        Provincia = (reader["Provincia"] as string ?? string.Empty).Trim(),
                        Departamento = (reader["Departamento"] as string ?? string.Empty).Trim(),
                        Fax = (reader["Fax"] as string ?? string.Empty).Trim(),
                        CorreoElectronico = (reader["CorreoElectronico"] as string ?? string.Empty).Trim(),
                        PaginaWeb = (reader["PaginaWeb"] as string ?? string.Empty).Trim(),
                        Documento = (reader["Documento"] as string ?? string.Empty).Trim(),
                        Telefono = (reader["Telefono"] as string ?? string.Empty).Trim(),
                        Telefono2 = (reader["Telefono2"] as string ?? string.Empty).Trim(),
                        CuentaDetraccion = (reader["CuentaDetraccion"] as string ?? string.Empty).Trim(),
                        Telefono3 = (reader["Telefono3"] as string ?? string.Empty).Trim(),
                        Ubigeo = (reader["Ubigeo"] as string ?? string.Empty).Trim(),
                        URLUsuario = (reader["URLUsuario"] as string ?? string.Empty).Trim(),
                        URLPassword = (reader["URLPassword"] as string ?? string.Empty).Trim(),
                        urlWebService = (reader["urlWebService"] as string ?? string.Empty).Trim()
                    };
                }
                _command.Connection.Close();
            }
            catch (Exception)
            {
                _command.Connection.Close();
                throw;
            }
            return companiaInfo;

        }

        public cls_DatosPercepcion DatosPercepcion(COBEC_Percepcion cOBEC_Percepcion)
        {
            cls_DatosPercepcion percepcion = new cls_DatosPercepcion();
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();

            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_API_Datos_Percepcion";
                _command.Connection.Open();

                SqlParameter p_compania = new SqlParameter("@p_compania", cOBEC_Percepcion.companiaSocio);
                _command.Parameters.Add(p_compania);
                SqlParameter p_documento = new SqlParameter("@p_documento", cOBEC_Percepcion.numeroDocumento);
                _command.Parameters.Add(p_documento);
                SqlParameter p_persona = new SqlParameter("@p_persona", cOBEC_Percepcion.proveedor);
                _command.Parameters.Add(p_persona);

                DbDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    percepcion = new cls_DatosPercepcion
                    {
                        TipoDocumentoReceptor = (reader["TipoDocumentoReceptor"] as string ?? string.Empty).Trim(),
                        DocumentoReceptor = (reader["DocumentoReceptor"] as string ?? string.Empty).Trim(),
                        RazonSocialReceptor = (reader["RazonSocialReceptor"] as string ?? string.Empty).Trim(),
                        NombreComercialReceptor = (reader["NombreComercialReceptor"] as string ?? string.Empty).Trim(),
                        UbigeoReceptor = (reader["UbigeoReceptor"] as string ?? string.Empty).Trim(),
                        DireccionReceptor = (reader["DireccionReceptor"] as string ?? string.Empty).Trim(),
                        DepartamentoReceptor = (reader["DepartamentoReceptor"] as string ?? string.Empty).Trim(),
                        ProvinciaReceptor = (reader["ProvinciaReceptor"] as string ?? string.Empty).Trim(),
                        DistritoReceptor = (reader["ProvinciaReceptor"] as string ?? string.Empty).Trim(),
                        CodigoPostalReceptor = (reader["CodigoPostalReceptor"] as string ?? string.Empty).Trim(),
                        CorreoReceptor = (reader["CorreoReceptor"] as string ?? string.Empty).Trim(),
                        UnidadNegocio = (reader["UnidadNegocio"] as string ?? string.Empty).Trim(),
                        DocumentoRelacionadoPX = (reader["DocumentoRelacionadoPX"] as string ?? string.Empty).Trim(),
                        MontoPercepcion = reader["MontoPercepcion"] as decimal? ?? 0m,
                        TipoRelacionado = (reader["TipoRelacionado"] as string ?? string.Empty).Trim(),
                        SerieRelacionado = (reader["SerieRelacionado"] as string ?? string.Empty).Trim(),
                        NumeroRelacionado = (reader["NumeroRelacionado"] as string ?? string.Empty).Trim(),
                        FechaDocumento = reader["FechaDocumento"] as DateTime? ?? DateTime.MinValue,
                        MontoTotal = reader["MontoTotal"] as decimal? ?? 0m,
                        Moneda = (reader["Moneda"] as string ?? string.Empty).Trim(),
                        TipoCambio = reader["TipoCambio"] as decimal? ?? 0
                    };
                }
                _command.Connection.Close();


            }
            catch (Exception)
            {
                _command.Connection.Close();
                throw;
            }

            return percepcion;
        }

    }

}