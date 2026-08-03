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
    public class CODAT_DatosImpuestos : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;
        public CODAT_DatosImpuestos(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public DocumentoImpuesto Lista(COBEC_Comprobante cOBEC_Comprobante)
        {
            DocumentoImpuesto objreturn = new DocumentoImpuesto();
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_API_Datos_Impuestos";
                _command.Connection.Open();

                SqlParameter p_compania = new SqlParameter("@p_compania", cOBEC_Comprobante.companiaSocio);
                _command.Parameters.Add(p_compania);

                SqlParameter p_tipodocumento = new SqlParameter("@p_tipodocumento", cOBEC_Comprobante.tipoDocumento);
                _command.Parameters.Add(p_tipodocumento);

                SqlParameter p_comprobante = new SqlParameter("@p_comprobante", cOBEC_Comprobante.numeroDocumento);
                _command.Parameters.Add(p_comprobante);

                DbDataReader _reader = _command.ExecuteReader();
                while (_reader.Read()) {
                    objreturn.CompaniaSocio = _reader["CompaniaSocio"]?.ToString() ?? string.Empty;
                    objreturn.TipoDocumento = _reader["TipoDocumento"]?.ToString() ?? string.Empty;
                    objreturn.NumeroDocumento = _reader["NumeroDocumento"]?.ToString() ?? string.Empty;
                    objreturn.TipoRegistro = _reader["TipoRegistro"]?.ToString() ?? string.Empty;
                    objreturn.Impuesto = _reader["Impuesto"]?.ToString() ?? string.Empty;
                    objreturn.Porcentaje = _reader["Porcentaje"] is DBNull ? 0m : Convert.ToDecimal(_reader["Porcentaje"]);
                    objreturn.Monto = _reader["Monto"] is DBNull ? 0m : Convert.ToDecimal(_reader["Monto"]);
                }
                _command.Connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("error" + e.Message);
                _command.Connection.Close();
            }
            return objreturn;
        }
    }
}
