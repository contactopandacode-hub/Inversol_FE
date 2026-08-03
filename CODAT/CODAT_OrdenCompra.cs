using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using COBE;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using COBEC;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace CODAT
{
    public class CODAT_OrdenCompra : DbContext
    {
        public DbContext context { get; set; }
        private readonly IConfiguration configuration;
        public CODAT_OrdenCompra(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }

        public string LogGrabar(cls_log logger)
        {
            string str_return = string.Empty;
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();

            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_API_OrdenCompraLog";
                _command.Connection.Open();

                SqlParameter pFecha = new SqlParameter("@pFecha", logger.fecha);
                _command.Parameters.Add(pFecha);

                SqlParameter pEstado = new SqlParameter("@pEstado", logger.estado);
                _command.Parameters.Add(pEstado);

                SqlParameter pMensaje = new SqlParameter("@pMensaje", logger.mensaje);
                _command.Parameters.Add(pMensaje);

                _command.ExecuteNonQuery();

            }
            catch (Exception ex )
            {
                str_return= ex.Message;
            }
            return str_return;
        }

        public RootCompras Lista(cls_OrdenCompras consultaDatos)
        {
            RootCompras ListaOrdenCompras = new RootCompras();          
            Dictionary<string, Compras> comprasMap = new Dictionary<string, Compras>();  // Para agrupar las compras
            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
            int int_Posicion = 0;
            string str_compraKey = string.Empty;
            try
            {
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "SNP_API_OrdenCompraLista";
                _command.Connection.Open();

                SqlParameter pRucEmisor = new SqlParameter("@pRuc", consultaDatos.RucEmisor);
                _command.Parameters.Add(pRucEmisor);

                SqlParameter pRucProveedor = new SqlParameter("@pFechaDesde", consultaDatos.fechaDesde);
                _command.Parameters.Add(pRucProveedor);

                SqlParameter pFechaDesde = new SqlParameter("@pFechaHasta", consultaDatos.fechaHasta);
                _command.Parameters.Add(pFechaDesde);

                DbDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    // Obtener los identificadores únicos para la compra
                    string compraSerie = string.Empty;  
                    string compraNumero = _reader["numeroDocumento"].ToString().Trim(); 
                    string ruc = _reader["ruc"].ToString().Trim();
                    string compraKey = $"{compraNumero}-{ruc}";  // Llave única para la compra
                    str_compraKey = compraKey;

                    if (str_compraKey == "-31045923-00000000022")
                        {
                        str_compraKey = "xxx";
                    }
                    // Verificar si ya hemos agregado esta compra
                    if (!comprasMap.ContainsKey(compraKey))
                    {
                        // Si no existe en el diccionario, crear la compra y agregarla al diccionario
                        Compras nuevaCompra = new Compras
                        {
                            Compra = CargarCabecera(_reader),
                            Productos = new List<Producto>()
                        };
                        comprasMap.Add(compraKey, nuevaCompra);
                    }

                    // Agregar el producto al detalle de la compra
                    Producto detalle = CargarDetalle(_reader);
                    comprasMap[compraKey].Productos.Add(detalle);
                }

                // Agregar todas las compras agrupadas a la lista final
                foreach (var compra in comprasMap.Values)
                {
                    ListaOrdenCompras.ListaCompras.Add(compra);
                }

                _reader.Close();
                _reader.Dispose();
                _command.Dispose();
                _command.Connection.Close();
            }
            catch (Exception ex)
            {
                _command.Dispose();
                _command.Connection.Close();
                Log.Information("codata_OrdenCompra-Lista: {Data}", ex.Message + str_compraKey);
                throw;
            }

            return ListaOrdenCompras;

        }

        static Compra CargarCabecera(DbDataReader reader)
        {
            Compra compra = new Compra();
            try
            {
                if (reader["numerodocumento"].ToString().Trim().Contains("-"))
                {
                    string[] numeroDocumento = reader["numerodocumento"].ToString().Trim().Split('-');
                    compra.Compra_Seriedoc = numeroDocumento[0];
                    compra.Compra_Nrodoc = numeroDocumento[1];
                }
                else
                {
                    compra.Compra_Nrodoc = reader["numerodocumento"].ToString().Trim();
                }

                compra.Proveedor_Ruc = reader["ruc"].ToString().Trim();
                compra.Compra_Tipodoc = reader["tipoDocumento"].ToString().Trim();
                compra.Compra_Observacion = reader["comentario"].ToString().Trim();
                compra.Compra_Total = Convert.ToDecimal(reader["MontoTotal"].ToString());
                compra.Compra_Subtotal = Convert.ToDecimal(reader["subTotal"].ToString());
                compra.Compra_Igv = Convert.ToDecimal(reader["igv"].ToString());
                compra.Compra_Fecha = Convert.ToDateTime(reader["FechaTransaccion"]).ToString("yyyy-MM-dd");
                compra.Compra_Estado = reader["estado"].ToString().Trim();
                compra.Compra_Fechatributacion = Convert.ToDateTime(reader["fechaDocumento"]).ToString("yyyy-MM-dd");
                compra.transaccion = reader["transaccion"].ToString().Trim();

                if (compra.Compra_Fechatributacion.Substring(0, 3) == "1900")
                    compra.Compra_Fechatributacion = string.Empty;
             }
            catch (Exception)
            {

                throw;
            }
            return compra;
        }

        // Método para cargar el detalle (simulación)
        static Producto CargarDetalle(DbDataReader reader)
        {
            Producto producto = new Producto();
            try
            {
                producto.Producto_Codigointerno = reader["item"].ToString().Trim();
                producto.Detallecompra_Descripcion = reader["descripcionLocal"].ToString().Trim();
                producto.Detallecompra_Costosinimpuesto = Convert.ToDecimal(reader["detalleMonto"]);
                producto.Detallecompra_Igv = Convert.ToDecimal(reader["detalleIgv"]);
                producto.Detallecompra_Cantidad = Convert.ToDecimal(reader["DetalleCantidad"]);
            }
            catch (Exception)
            {
                throw;
            }
            return producto;
        }

        // Método para validar registros (simulación)
        static bool ValidarRegistros(DbDataReader currentRow, Compra previousRow)
        {
            try
            {
                if (previousRow == null)
                    return false;

                if (currentRow["tipoDocumento"].ToString() == previousRow.Compra_Seriedoc
                    && currentRow["numeroDocumento"].ToString().EndsWith(previousRow.Compra_Nrodoc)
                    && currentRow["ruc"].ToString() == previousRow.Proveedor_Ruc)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }
    }

    //public List<DetalleItem> DatosDetalle(string p_companiaSocio, string p_numeroOrden)
    //{
    //    List<DetalleItem> obj_Listadetalle = new List<DetalleItem>();
    //    DbCommand _command = context.Database.GetDbConnection().CreateCommand();
    //    try
    //    {
    //        _command.CommandType = System.Data.CommandType.StoredProcedure;
    //        _command.CommandText = "SNP_API_ListaOrdenCompraDetalle";
    //        _command.Connection.Open();

    //        SqlParameter pRucEmisor = new SqlParameter("@pCompaniaSocio", p_companiaSocio);
    //        _command.Parameters.Add(pRucEmisor);

    //        SqlParameter pRucProveedor = new SqlParameter("@pNumeroOrden", p_numeroOrden);
    //        _command.Parameters.Add(pRucProveedor);              

    //        DbDataReader _reader = _command.ExecuteReader();

    //        while (_reader.Read())
    //        {
    //            DetalleItem detalle = new DetalleItem();
    //            detalle.Linea = Convert.ToInt32( _reader["Secuencia"].ToString());
    //            detalle.Item = _reader["item"].ToString().Trim();
    //            detalle.Commodity = _reader["Commodity"].ToString().Trim();
    //            detalle.Descripcion = _reader["Descripcion"].ToString().Trim();
    //            detalle.UnidadMedida = _reader["UnidadMedida"].ToString().Trim();
    //            detalle.CantidadPedida = Convert.ToDecimal(_reader["CantidadPedida"].ToString());
    //            detalle.PrecioUnitario = Convert.ToDecimal(_reader["PrecioUnitario"].ToString());
    //            detalle.Total = Convert.ToDecimal(_reader["Total"].ToString());

    //            if (!_reader.IsDBNull(_reader.GetOrdinal("FechaEntrega")) )
    //                detalle.FechaEntrega = Convert.ToDateTime(_reader["FechaEntrega"].ToString()).ToString("yyyy-MM-dd");

    //            detalle.CantidadRecibida = Convert.ToDecimal(_reader["CantidadRecibida"].ToString());
    //            detalle.CentroCosto = _reader["CentroCosto"].ToString().Trim();
    //            detalle.Estado = _reader["Estado"].ToString();
    //            detalle.Comentario = _reader["Comentario"].ToString();
    //            obj_Listadetalle.Add(detalle);
    //        }
    //        _reader.Close();
    //        _reader.Dispose();
    //        _command.Dispose();
    //        _command.Connection.Close();
    //    }              
    //    catch (Exception)
    //    {
    //        _command.Dispose();
    //        _command.Connection.Close();
    //        throw;
    //    }
    //    return obj_Listadetalle;

    //}





}
