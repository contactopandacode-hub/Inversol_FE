//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
//using Microsoft.Extensions.Configuration;
//using Microsoft.VisualBasic;
//using COBE;
//using CODAT;

//using COBEC;
//using System.Data.SqlClient;
//using System.Security.Policy;
//using System.IO;
//using ServicioRSNetCore.Controllers.Clases;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using Serilog;
//using System.Reflection.Metadata.Ecma335;

//namespace ServicioRSNetCore.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class ServicioController : ControllerBase
//    {
//        private readonly IConfiguration configuration;      
//        public DbContext context { get; set; }

//        public ServicioController(IConfiguration _configuration, DbContext _context)
//        {
//            configuration = _configuration;
//            this.context = _context;
//        }
              

//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        [HttpPost("[action]")]
//        public ActionResult OrdenCompra([FromBody] cls_OrdenCompras OrdenCompra)
//        {
//            var jsonData = System.Text.Json.JsonSerializer.Serialize(OrdenCompra);
//            Log.Information("OrdenCompra: {Data}", jsonData);
//            CODAT_OrdenCompra obj_datos = new CODAT_OrdenCompra(configuration, this.context);
//            RootCompras obj_Return= new RootCompras();            
//            COBEc_Error obj_error = new COBEc_Error();
            
//            try
//            {
//                obj_Return = obj_datos.Lista(OrdenCompra);
//            }
//            catch (Exception e)            {

//                obj_error.codigo = "99";
//                obj_error.mensaje = e.Message;
//                return Ok(obj_error);
//            }
//            return Ok(obj_Return);
//        }

//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        [HttpPost("[action]")]
//        public ActionResult NotaCredito([FromBody] cls_OrdenCompras OrdenCompra)
//        {
//            var jsonData = System.Text.Json.JsonSerializer.Serialize(OrdenCompra);
//            Log.Information("OrdenCompra: {Data}", jsonData);
//            RootNotasCredito obj_Return = new RootNotasCredito();

//            COBEc_Error obj_error = new COBEc_Error();

//            try
//            {
//                // obj_respuesta = obj_datosOrden.DatosCabecera(consultaDatos);
//            }
//            catch (Exception e)
//            {

//                obj_error.codigo = "99";
//                obj_error.mensaje = e.Message;
//                return Ok(obj_error);
//            }
//            return Ok(obj_Return);
//        }

//       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        [HttpPut("[action]")]
//        public ActionResult LogRegistro([FromBody] cls_log Loger)
//        {
//            var jsonData = System.Text.Json.JsonSerializer.Serialize(Loger);
//            Log.Information("OrdenCompra: {Data}", jsonData);
//            CODAT_OrdenCompra obj_datos = new CODAT_OrdenCompra(configuration, this.context);
//            COBEc_Error obj_error = new COBEc_Error();
//            string str_return = string.Empty;

//            try
//            {
//                str_return = obj_datos.LogGrabar(Loger);
//            }
//            catch (Exception e)
//            {

//                obj_error.codigo = "99";
//                obj_error.mensaje = e.Message;
//                return Ok(obj_error);
//            }
//            return Ok();//new { message = "Recurso actualizado exitosamente."});
//        }
//    }
//}

