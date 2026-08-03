using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using COBE;
using CODAT;

using COBEC;
using Serilog;
using ServicioRSNetCore.Controllers.Funciones;
using ServicioRSNetCore.GuiaRemision;


namespace ServicioRSNetCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FEController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private DbContext context;

        public FEController(IConfiguration _configuration, DbContext _context)
        {
            configuration = _configuration;
            this.context = _context;
        }


        [HttpPost("RegistrarComprobante")]
        public ActionResult RegistrarComprobante([FromBody] RegistrarComprobanteRequest request)
        {
            COBEc_Error obj_error = new COBEc_Error();
            ProcesarComprobantes obj_procesar = new ProcesarComprobantes(configuration, this.context);
            try
            {
                obj_error = obj_procesar.Registrar(request);
            }
            catch (Exception e)
            {
                obj_error.codigo = "99";
                obj_error.mensaje = e.Message;
                return Ok(obj_error);
            }

            return Ok(obj_error);
        }

        [HttpPost("RegistrarGuia")]
        public ActionResult RegistrarGuia([FromBody] COBEC_Guia request)
        {
            COBEc_Error obj_error = new COBEc_Error();
            ProcesaGuiaRemision obj_procesar = new ProcesaGuiaRemision(configuration, this.context);
            try
            {
                obj_error = obj_procesar.Registrar(request);
            }
            catch (Exception e)
            {
                obj_error.codigo = "99";
                obj_error.mensaje = e.Message;
                return Ok(obj_error);
            }

            return Ok(obj_error);
        }

        [HttpPost("ObtenerEstado")]
        public ActionResult ObtenerEstado([FromBody] COBEC_Generico request)
        {
            COBEc_Error obj_error = new COBEc_Error();
            EFACTGeneral obj_procesar = new EFACTGeneral(configuration, this.context);
            try
            {
                obj_error = obj_procesar.GeneraToken(request);
                if(obj_error.codigo == "00")
                {
                    obj_error = obj_procesar.EfactConsultaEstado(request.identificador, obj_error.mensaje);
                }
            }
            catch (Exception e)
            {
                obj_error.codigo = "99";
                obj_error.mensaje = e.Message;
                return Ok(obj_error);
            }

            return Ok(obj_error);
        }
    }
}
