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
    public class CODAT_DocumentoDetalle : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;
        public CODAT_DocumentoDetalle(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        //public DatosDocumentoDetalle Lista(COBEC_Comprobante cOBEC_Comprobante)
        //{
        //    DatosDocumentoDetalle objreturn = new DatosDocumentoDetalle();
        //    DbCommand _command = context.Database.GetDbConnection().CreateCommand();
        //    try
        //    {
        //        _command.CommandType = System.Data.CommandType.StoredProcedure;
        //        _command.CommandText = "SNP_API_Datos_Documento_Detalle";
        //        _command.Connection.Open();

        //        SqlParameter p_compania = new SqlParameter("@p_compania", cOBEC_Comprobante.companiaSocio);
        //        _command.Parameters.Add(p_compania);

        //        SqlParameter p_tipodocumento = new SqlParameter("@p_tipodocumento", cOBEC_Comprobante.tipoDocumento);
        //        _command.Parameters.Add(p_tipodocumento);

        //        SqlParameter p_comprobante = new SqlParameter("@p_comprobante", cOBEC_Comprobante.numeroDocumento);
        //        _command.Parameters.Add(p_comprobante);

        //        DbDataReader _reader = _command.ExecuteReader();

        //        while (_reader.Read())
        //        {
        //            objreturn.CompaniaSocio = _reader["CompaniaSocio"]?.ToString() ?? string.Empty;
        //            objreturn.TipoDocumento = _reader["TipoDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.NumeroDocumento = _reader["NumeroDocumento"]?.ToString() ?? string.Empty;
        //            objreturn.TipoDetalle = _reader["TipoDetalle"]?.ToString() ?? string.Empty;
        //            objreturn.Lote = _reader["Lote"]?.ToString() ?? string.Empty;
        //            objreturn.ItemCodigo = _reader["ItemCodigo"]?.ToString() ?? string.Empty;
        //            objreturn.Descripcion = _reader["Descripcion"]?.ToString() ?? string.Empty;
        //            objreturn.Iscafectoflag = _reader["iscafectoflag"]?.ToString() ?? string.Empty;
        //            objreturn.UnidadCodigo = _reader["UnidadCodigo"]?.ToString() ?? "UND";
        //            objreturn.Estado = _reader["Estado"]?.ToString() ?? string.Empty;
        //            objreturn.UltimoUsuario = _reader["UltimoUsuario"]?.ToString() ?? string.Empty;
        //            objreturn.IgvExoneradoFlag = _reader["IgvExoneradoFlag"]?.ToString() ?? "N";
        //            objreturn.TransferenciaGratuitaFlag = _reader["TransferenciaGratuitaFlag"]?.ToString() ?? "N";                    
        //            objreturn.Linea = _reader["Linea"] is DBNull ? 0 : Convert.ToInt32(_reader["Linea"]);
        //            //objreturn.ImpuestoISC = _reader["ImpuestoISC"] is DBNull ? 0m : Convert.ToDecimal(_reader["ImpuestoISC"]);
        //            objreturn.CantidadPedida = _reader["CantidadPedida"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["CantidadPedida"]));
        //            objreturn.PrecioUnitario = _reader["PrecioUnitario"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["PrecioUnitario"]));
        //            objreturn.PrecioUnitarioFinal = _reader["PrecioUnitarioFinal"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["PrecioUnitarioFinal"]));
        //            objreturn.PrecioUnitarioGratuito = _reader["PrecioUnitarioGratuito"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["PrecioUnitarioGratuito"]));
        //            objreturn.PrecioUnitarioOriginal = _reader["PrecioUnitarioOriginal"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["PrecioUnitarioOriginal"]));
        //            objreturn.PorcentajeDescuento01 = _reader["PorcentajeDescuento01"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["PorcentajeDescuento01"]));
        //            objreturn.PorcentajeDescuento02 = _reader["PorcentajeDescuento02"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["PorcentajeDescuento02"]));
        //            objreturn.PorcentajeDescuento03 = _reader["PorcentajeDescuento03"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["PorcentajeDescuento03"]));
        //            objreturn.MontoIvap = _reader["montoivap"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["montoivap"]));
        //            objreturn.Monto = _reader["Monto"] is DBNull ? 0m : Math.Abs(Convert.ToDecimal(_reader["Monto"]));                    
        //            objreturn.UltimaFechaModif = _reader["UltimaFechaModif"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(_reader["UltimaFechaModif"]);

        //        }
        //        _command.Connection.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("error" + e.Message);
        //        _command.Connection.Close();
        //    }
        //    return objreturn;
        //}
    }
}
