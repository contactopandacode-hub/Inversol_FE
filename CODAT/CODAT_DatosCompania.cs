using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using COBE;
using System.Data.Common;
using System.Data.SqlClient;

using COBEC;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CODAT
{
    public class CODAT_DatosCompania : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;
        public CODAT_DatosCompania(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public DatosCompania Lista (COBEC_Comprobante cOBEC_Comprobante) {
            DatosCompania objreturn = new DatosCompania();
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_API_Datos_Compania";
                _command.Connection.Open();

                SqlParameter p_compania = new SqlParameter("@p_compania", cOBEC_Comprobante.companiaSocio);
                _command.Parameters.Add(p_compania);

                DbDataReader _reader = _command.ExecuteReader();

                while (_reader.Read()){                    
                    objreturn.DescripcionLarga = _reader["DescripcionLarga"].ToString().Trim();
                    objreturn.DescripcionCorta = _reader["DescripcionCorta"].ToString().Trim();
                    objreturn.DireccionComun = _reader["DireccionComun"].ToString().Trim();
                    objreturn.DireccionAdicional = _reader["DireccionAdicional"].ToString().Trim();
                    objreturn.DocumentoFiscal = _reader["DocumentoFiscal"].ToString().Trim();
                    objreturn.Distrito = _reader["Distrito"].ToString().Trim();
                    objreturn.Provincia = _reader["Provincia"].ToString().Trim();
                    objreturn.Departamento = _reader["Departamento"].ToString().Trim();
                    objreturn.Telefono = _reader["Telefono"].ToString().Trim();
                    objreturn.Telefono2 = _reader["Telefono2"].ToString().Trim();
                    objreturn.Fax = _reader["Fax"].ToString().Trim();
                    objreturn.CorreoElectronico = _reader["CorreoElectronico"].ToString().Trim();
                    objreturn.PaginaWeb = _reader["PaginaWeb"].ToString().Trim();
                    //objreturn.DetraccionCuentaBancaria = _reader["DetraccionCuentaBancaria"].ToString().Trim();
                    objreturn.Documento = _reader["Documento"].ToString().Trim();

                }
                _command.Connection.Close();
            }
            catch (Exception)
            {
                _command.Connection.Close();
            }
            return objreturn;
        }
    }
}
