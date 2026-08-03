using COBE;
using COBEC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace CODAT
{
    public class CODAT_ComunicadoBaja : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;
        public CODAT_ComunicadoBaja(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public cls_ComunicadoBaja Datos(COBEC_Comprobante cOBEC_Comprobante)
        {
            cls_ComunicadoBaja cls_ComunicadoBaja = new cls_ComunicadoBaja();
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_WSFE_ComunicadoBajaDatos";
                _command.Connection.Open();

                SqlParameter p_compania = new SqlParameter("@pCompania", cOBEC_Comprobante.companiaSocio);
                _command.Parameters.Add(p_compania);

                SqlParameter pTipoDocumento = new SqlParameter("@pTipoDocumento", cOBEC_Comprobante.tipoDocumento);
                _command.Parameters.Add(pTipoDocumento);

                SqlParameter pNumeroDocumento = new SqlParameter("@pNumeroDocumento", cOBEC_Comprobante.numeroDocumento);
                _command.Parameters.Add(pNumeroDocumento);


                DbDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    cls_ComunicadoBaja = new cls_ComunicadoBaja
                    {
                        rucEmisor = (reader["rucEmisor"] as string ?? string.Empty).Trim(),
                        resumenId = reader["correlativo"] is DBNull ? 0 : Convert.ToInt32(reader["correlativo"]),
                        fechaEmision = reader["fechaEmision"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["fechaEmision"]),                 
                        razonSocial = (reader["razonSocial"] as string ?? string.Empty).Trim(),
                        tipoDocumento = (reader["tipoDocumento"] as string ?? string.Empty).Trim(),
                        serieNumeroDocumento = (reader["serieNumeroDocumento"] as string ?? string.Empty).Trim(),
                        motivoBaja = (reader["motivoBaja"] as string ?? string.Empty).Trim(),
                        rutaXml = (reader["rutaXml"] as string ?? string.Empty).Trim(),
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
            return cls_ComunicadoBaja;
        }

        public COBEc_Error Update(COBEC_Comprobante cOBEC_Comprobante, string par_tipo ,string par_estado, string par_hashCode ,
            string par_descripcion, string par_correlativo)
        {
            COBEc_Error cls_ComunicadoBaja = new COBEc_Error();
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
            cls_ComunicadoBaja.codigo = "00";
            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_WSFE_BajaDatosActualizar";
                _command.Connection.Open();

                SqlParameter p_compania = new SqlParameter("@pCompania", cOBEC_Comprobante.companiaSocio);
                _command.Parameters.Add(p_compania);

                SqlParameter pTipoDocumento = new SqlParameter("@pTipoDocumento", cOBEC_Comprobante.tipoDocumento);
                _command.Parameters.Add(pTipoDocumento);

                SqlParameter pNumeroDocumento = new SqlParameter("@pNumeroDocumento", cOBEC_Comprobante.numeroDocumento);
                _command.Parameters.Add(pNumeroDocumento);

                SqlParameter pParTipo = new SqlParameter("@pParTipo", par_tipo);
                _command.Parameters.Add(pParTipo);

                SqlParameter pFeEstado = new SqlParameter("@pFeEstado", par_estado);
                _command.Parameters.Add(pFeEstado);

                SqlParameter pHashCode = new SqlParameter("@pHashCode", par_hashCode);
                _command.Parameters.Add(pHashCode);

                SqlParameter pDescripcion = new SqlParameter("@pDescripcion", par_descripcion);
                _command.Parameters.Add(pDescripcion);

                SqlParameter pCorrelativo = new SqlParameter("@pCorrelativo", par_correlativo);
                _command.Parameters.Add(pCorrelativo);

               _command.ExecuteNonQuery();
               
                _command.Connection.Close();
            }
            catch (Exception e)
            {
                _command.Connection.Close();
                cls_ComunicadoBaja.codigo = "01";
                cls_ComunicadoBaja.mensaje = e.ToString();
                throw;
            }
            return cls_ComunicadoBaja;
        }
    }
}
