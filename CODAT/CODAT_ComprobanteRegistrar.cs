using COBE;
using COBEC;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace CODAT
{
    public class CODAT_ComprobanteRegistrar
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CODAT_ComprobanteRegistrar(IConfiguration configuration, DbContext context)
        {
            _configuration = configuration;
            _context = context;

            // Obtener la cadena de conexión del configuration o del context
            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = _context.Database.GetDbConnection()?.ConnectionString;
            }
        }

        // Método para obtener una conexión válida
        private DbConnection GetConnection()
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                return new SqlConnection(_connectionString);
            }

            var connection = _context.Database.GetDbConnection();
            if (connection != null)
            {
                return connection;
            }

            throw new InvalidOperationException("No se pudo obtener una conexión a la base de datos");
        }

        public DatosCompania DatosCompania(RegistrarComprobanteRequest request)
        {
            DatosCompania objreturn = new DatosCompania();

            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SNP_PCODE_FE_DatosCompania";

                        var par_compania = new SqlParameter("@par_compania", request.companiaSocio ?? (object)DBNull.Value);
                        command.Parameters.Add(par_compania);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                objreturn.DescripcionLarga = reader["DescripcionLarga"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.DireccionComun = reader["DireccionComun"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.DireccionAdicional = reader["DireccionAdicional"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.DocumentoFiscal = reader["DocumentoFiscal"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Distrito = reader["Distrito"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Provincia = reader["Provincia"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Departamento = reader["Departamento"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Ubigeo = reader["Ubigeo"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Fax = reader["Fax"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.CorreoElectronico = reader["CorreoElectronico"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.PaginaWeb = reader["PaginaWeb"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Documento = reader["Documento"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Telefono = reader["Telefono"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.Telefono2 = reader["Telefono2"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.CuentaDetraccion = reader["CuentaDetraccion"]?.ToString()?.Trim() ?? string.Empty;
                                objreturn.URLPassword = reader["URLPassword"]?.ToString()?.Trim() ?? string.Empty;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en DatosCompania: {ex.Message}");
                }
            }

            return objreturn;
        }

        public DatosDocumento DatosDocumento(RegistrarComprobanteRequest request)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@par_compania", request.companiaSocio ?? (object)DBNull.Value);
                    parameters.Add("@par_tipodocumento", request.tipoDocumento ?? (object)DBNull.Value);
                    parameters.Add("@par_comprobante", request.numeroDocumento ?? (object)DBNull.Value);

                    var result = connection.QueryFirstOrDefault<DatosDocumento>(
                        "SNP_PCODE_FE_DatosDocumento",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new DatosDocumento();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos del documento: {ex.Message}");
                return new DatosDocumento();
            }
        }

        public List<DocumentoImpuesto> DatosImpuesto(RegistrarComprobanteRequest request)
        {
            List<DocumentoImpuesto> listaImpuestos = new List<DocumentoImpuesto>();

            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SNP_PCODE_FE_DocumentoImpuesto";

                        command.Parameters.Add(new SqlParameter("@par_compania", request.companiaSocio ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@par_tipodocumento", request.tipoDocumento ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@par_comprobante", request.numeroDocumento ?? (object)DBNull.Value));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DocumentoImpuesto impuesto = new DocumentoImpuesto
                                {
                                    CompaniaSocio = reader["CompaniaSocio"]?.ToString() ?? string.Empty,
                                    TipoDocumento = reader["TipoDocumento"]?.ToString() ?? string.Empty,
                                    NumeroDocumento = reader["NumeroDocumento"]?.ToString() ?? string.Empty,
                                    TipoRegistro = reader["TipoRegistro"]?.ToString() ?? string.Empty,
                                    Impuesto = reader["Impuesto"]?.ToString() ?? string.Empty,
                                    Porcentaje = reader["Porcentaje"] is DBNull ? 0m : Convert.ToDecimal(reader["Porcentaje"]),
                                    Monto = reader["Monto"] is DBNull ? 0m : Convert.ToDecimal(reader["Monto"])
                                };
                                listaImpuestos.Add(impuesto);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error en DatosImpuesto: {e.Message}");
                }
            }

            return listaImpuestos;
        }

        public List<DatosDocumentoDetalle> DocumentoDetalle(RegistrarComprobanteRequest request)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@par_compania", request.companiaSocio ?? (object)DBNull.Value);
                    parameters.Add("@par_tipodocumento", request.tipoDocumento ?? (object)DBNull.Value);
                    parameters.Add("@par_comprobante", request.numeroDocumento ?? (object)DBNull.Value);

                    var result = connection.Query<DatosDocumentoDetalle>(
                        "SNP_PCODE_FE_DatosDocumentoDetalle",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DocumentoDetalle: {ex.Message}");
                return new List<DatosDocumentoDetalle>();
            }
        }

       

        public cls_ConsultasAdicionales ConsultasAdicionales(int al_tipo, string as_valor1, string as_valor2, string as_valor3, string as_valor4)
        {
            cls_ConsultasAdicionales objreturn = new cls_ConsultasAdicionales();

            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_API_Consultas_Adicionales";

                        command.Parameters.Add(new SqlParameter("@p_tipo", al_tipo));
                        command.Parameters.Add(new SqlParameter("@ps_valor1", as_valor1 ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@ps_valor2", as_valor2 ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@ps_valor3", as_valor3 ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@ps_valor4", as_valor4 ?? (object)DBNull.Value));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                objreturn.FactorPorcentaje = reader["FactorPorcentaje"] is DBNull ? 0m : Convert.ToDecimal(reader["FactorPorcentaje"]);
                                objreturn.CodigoFiscal = reader["CodigoFiscal"]?.ToString() ?? string.Empty;
                                objreturn.FechaDocumento = reader["FechaDocumento"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["FechaDocumento"]);
                                objreturn.MotivoSustento = reader["MotivoSustento"]?.ToString() ?? string.Empty;
                                objreturn.CodigoFormaPago = reader["CodigoFormaPago"]?.ToString() ?? string.Empty;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error en ConsultasAdicionales: {e.Message}");
                }
            }

            return objreturn;
        }

        public COBEc_Error DocumentoActualizarEstado(RegistrarComprobanteRequest request, string par_fehascode, string par_tipo)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@par_compania", request.companiaSocio ?? (object)DBNull.Value);
                    parameters.Add("@par_tipodocumento", request.tipoDocumento ?? (object)DBNull.Value);
                    parameters.Add("@par_comprobante", request.numeroDocumento ?? (object)DBNull.Value);
                    parameters.Add("@par_fehashcode", par_fehascode ?? (object)DBNull.Value);
                    parameters.Add("@par_tipo", par_tipo);

                    var result = connection.QueryFirstOrDefault<COBEc_Error>(
                        "SNP_PCODE_FE_ActualizarEstadoDocumento",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null && result.codigo == "0")
                    {
                        return new COBEc_Error
                        {
                            codigo = "0",
                            mensaje = result.mensaje ?? "Documento actualizado correctamente"
                        };
                    }
                    else
                    {
                        return new COBEc_Error
                        {
                            codigo = "1",
                            mensaje = result?.mensaje ?? "Error: No se recibió respuesta del procedimiento almacenado"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new COBEc_Error
                {
                    codigo = "1",
                    mensaje = $"Error DocumentoActualizarEstado: {e.Message}"
                };
            }
        }

        public cls_GuiaDatos GuiaDatos(COBEC_Guia request)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@pCompania", request.companiaSocio ?? (object)DBNull.Value, DbType.String, size: 8);
                    parameters.Add("@pSerieGuia", request.serieNumero ?? (object)DBNull.Value, DbType.String, size: 4);
                    parameters.Add("@pGuiaNumero", request.guiaNumero ?? (object)DBNull.Value, DbType.String, size: 10);

                    var result = connection.QueryFirstOrDefault<cls_GuiaDatos>(
                        "SNP_PCODE_GuiaDatosCabecera",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result; // null si no encontró la guía (igual que "dt_Guia Is Nothing" en VB)
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos de la guía: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene el detalle (ítems) de la guía
        /// </summary>
        public List<cls_GuiaDetalle> GuiaDatosDetalle(COBEC_Guia request)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@pCompania", request.companiaSocio ?? (object)DBNull.Value);
                    parameters.Add("@pSerieGuia", request.serieNumero ?? (object)DBNull.Value);
                    parameters.Add("@pGuiaNumero", request.guiaNumero ?? (object)DBNull.Value);

                    var result = connection.Query<cls_GuiaDetalle>(
                        "SNP_PCODE_DatosGuiaDetalle", // ajustar al nombre real del store
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList(); // lista vacía si no hay ítems (Dapper nunca devuelve null aquí)
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el detalle de la guía: {ex.Message}");
                return new List<cls_GuiaDetalle>();
            }
        }

        /// <summary>
        /// Obtiene la dirección del punto de llegada (equivalente a obj_Datos.GuiaPuntoLlegada en VB)
        /// </summary>
        public cls_GuiaPuntaLlegada GuiaPuntoLlegada(int destinatario, int destinatarioDireccionSecuencia)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@par_destinatario", destinatario);
                    parameters.Add("@par_direccionsecuencia", destinatarioDireccionSecuencia);

                    var result = connection.QueryFirstOrDefault<cls_GuiaPuntaLlegada>(
                        "SNP_PCODE_GuiaPuntoLlegada", // ajustar al nombre real del store
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new cls_GuiaPuntaLlegada();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el punto de llegada: {ex.Message}");
                return new cls_GuiaPuntaLlegada();
            }
        }


        /// <summary>
        /// Actualiza el estado de la guía tras el envío a EFACT/SUNAT (SNP_WSFE_GuiaDatosActualizar)
        /// </summary>
        /// <param name="request">Datos originales de la guía (para obtener Compania, SerieGuia, NumeroGuia)</param>
        /// <param name="parTipo">"1" = registrado OK, "2" = error</param>
        /// <param name="feEstado">Código de estado FE</param>
        /// <param name="hashCode">Código hash devuelto por SUNAT (vacío si fue error)</param>
        /// <param name="observaciones">Descripción/mensaje de respuesta</param>
        /// <param name="identificador">Token/identificador de la respuesta (ej. str_Token)</param>
        public bool GuiaActualizarDatos(COBEC_Guia request, string parTipo, string feEstado, string hashCode, string observaciones, string identificador)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@pCompania", request.companiaSocio ?? (object)DBNull.Value, DbType.String, size: 8);
                    parameters.Add("@pSerieGuia", request.serieNumero ?? (object)DBNull.Value, DbType.String, size: 4);
                    parameters.Add("@pNumeroGuia", request.guiaNumero ?? (object)DBNull.Value, DbType.String, size: 15);
                    parameters.Add("@pParTipo", parTipo ?? (object)DBNull.Value, DbType.StringFixedLength, size: 1);
                    parameters.Add("@pFeEstado", feEstado ?? (object)DBNull.Value, DbType.StringFixedLength, size: 2);
                    parameters.Add("@pHashCode", hashCode ?? (object)DBNull.Value, DbType.String, size: 255);
                    parameters.Add("@pObservaciones", observaciones ?? (object)DBNull.Value, DbType.String, size: -1);
                    parameters.Add("@pIdentificador", identificador ?? (object)DBNull.Value, DbType.String, size: 100);

                    connection.Execute(
                        "SNP_WSFE_GuiaDatosActualizar",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar datos de la guía: {ex.Message}");
                return false;
            }
        }

        public List<cls_CuentaBancaria> CuentaBancaria(COBEC_Comprobante cOBEC_Comprobante)
        {
            List<cls_CuentaBancaria> listabancaria = new List<cls_CuentaBancaria>();

            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SNP_API_CuentaBancaria";
                        command.Parameters.Add(new SqlParameter("@p_compania", cOBEC_Comprobante.companiaSocio ?? (object)DBNull.Value));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cls_CuentaBancaria cuentaBancaria = new cls_CuentaBancaria
                                {
                                    CuentaBancoOriginal = reader["CuentaBancoOriginal"]?.ToString() ?? string.Empty,
                                    DescripcionBanco = reader["DescripcionBanco"]?.ToString() ?? string.Empty,
                                    Moneda = reader["Moneda"]?.ToString() ?? string.Empty,
                                    CuentaInterbancaria = reader["CuentaInterbancaria"]?.ToString() ?? string.Empty,
                                };
                                listabancaria.Add(cuentaBancaria);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error en CuentaBancaria: {e.Message}");
                }
            }

            return listabancaria;
        }

        public string f_number_to_letters(decimal par_number, string par_currency, string par_language)
        {
            if ((int)(par_number / 1000000000) > 999)
            {
                return "ERROR: EL MONTO A PAGAR ES DEMASIADO GRANDE";
            }

            if (par_number < 0.01m)
            {
                return "EL MONTO A PAGAR ES CERO";
            }

            string[] L = new string[57];

            L[1] = "UN"; L[2] = "DOS"; L[3] = "TRES"; L[4] = "CUATRO"; L[5] = "CINCO";
            L[6] = "SEIS"; L[7] = "SIETE"; L[8] = "OCHO"; L[9] = "NUEVE";
            L[11] = "CIENTO "; L[12] = "DOSCIENTOS "; L[13] = "TRESCIENTOS ";
            L[14] = "CUATROCIENTOS "; L[15] = "QUINIENTOS "; L[16] = "SEISCIENTOS ";
            L[17] = "SETECIENTOS "; L[18] = "OCHOCIENTOS "; L[19] = "NOVECIENTOS ";
            L[21] = "DIECI"; L[22] = "VEINTI"; L[23] = "TREINTI"; L[24] = "CUARENTI";
            L[25] = "CINCUENTI"; L[26] = "SESENTI"; L[27] = "SETENTI"; L[28] = "OCHENTI"; L[29] = "NOVENTI";
            L[31] = "UNO"; L[32] = "DOS"; L[33] = "TRES"; L[34] = "CUATRO"; L[35] = "CINCO";
            L[36] = "SEIS"; L[37] = "SIETE"; L[38] = "OCHO"; L[39] = "NUEVE";
            L[41] = "DIEZ"; L[42] = "VEINTE"; L[43] = "TREINTA"; L[44] = "CUARENTA";
            L[45] = "CINCUENTA"; L[46] = "SESENTA"; L[47] = "SETENTA"; L[48] = "OCHENTA"; L[49] = "NOVENTA";
            L[51] = "ONCE"; L[52] = "DOCE"; L[53] = "TRECE"; L[54] = "CATORCE"; L[55] = "QUINCE";
            L[56] = "CIEN ";

            string w_decimals;
            long indice, indicew, pivot, resta, workv;
            decimal w_number, numeroi, numeroy, numeroip;
            string letras = "";

            w_number = Math.Round(par_number, 2);
            numeroy = (long)Math.Truncate(w_number);
            w_decimals = (100 * (w_number - numeroy)).ToString("00");

            for (int j = 1; j <= 4; j++)
            {
                pivot = 100;
                numeroi = 0;

                switch (j)
                {
                    case 1:
                        if (numeroy > 999999999) numeroi = (long)(numeroy / 1000000000);
                        break;
                    case 2:
                        if (numeroy > 999999) numeroi = (long)(numeroy / 1000000);
                        break;
                    case 3:
                        if (numeroy > 999) numeroi = (long)(numeroy / 1000);
                        break;
                    case 4:
                        if (numeroy > 0) numeroi = numeroy;
                        break;
                }

                numeroip = numeroi;

                if (numeroi > 0)
                {
                    for (indice = 10; indice <= 30; indice += 10)
                    {
                        if (numeroi >= pivot)
                        {
                            workv = (long)(numeroi / 10);

                            if (workv * 10 == numeroi && pivot == 10)
                            {
                                indicew = 40 + (int)(numeroi / pivot);
                                workv = (long)(numeroi / pivot);
                                resta = workv * pivot;
                            }
                            else
                            {
                                if (numeroi < 16 && numeroi > 10)
                                {
                                    indicew = 40 + (int)numeroi;
                                    resta = (long)numeroi;
                                }
                                else
                                {
                                    indicew = indice + (int)(numeroi / pivot);

                                    if ((j < 3 && indice == 30) || (j == 3 && indicew == 31))
                                        indicew -= 30;

                                    workv = (long)(numeroi / pivot);
                                    resta = workv * pivot;
                                }
                            }

                            if (numeroi == 100) indicew = 56;

                            letras += L[indicew];
                            numeroi -= resta;
                        }
                        pivot /= 10;
                    }

                    switch (j)
                    {
                        case 1:
                            numeroy -= (numeroip * 1000000000);
                            letras += " MIL ";
                            break;
                        case 2:
                            numeroy -= (numeroip * 1000000);
                            letras += " MILLON";
                            if (numeroip > 1) letras += "ES";
                            letras += " ";
                            break;
                        case 3:
                            numeroy -= (numeroip * 1000);
                            letras += " MIL ";
                            break;
                    }
                }
            }

            string ls_pais = "PER";
            if (ls_pais == "VEN")
            {
                letras = letras + " CON " + w_decimals + "/100 ";
            }
            else
            {
                if (string.IsNullOrEmpty(letras))
                {
                    letras = "CERO" + " Y " + w_decimals + "/100 ";
                }
                else
                {
                    letras = letras + " Y " + w_decimals + "/100 ";
                }
            }

            return letras.Trim();
        }

        public string f_fe_espacios(string as_texto)
        {
            as_texto = as_texto ?? "";
            return as_texto.PadRight(30);
        }
    }
}