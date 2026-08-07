using Azure;
using COBE;
using COBEC;
using CODAT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Serilog;
using ServicioRSNetCore.Controllers.Funciones;
using ServicioRSNetCore.GuiaRemision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


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
            COBEC_EstadoReturn obj_error = new COBEC_EstadoReturn();
            COBEc_Error obj_token = new COBEc_Error();
            EFACTGeneral obj_procesar = new EFACTGeneral(configuration, this.context);
            try
            {
                obj_token = obj_procesar.GeneraToken(request);
                if(obj_token.codigo == "00")
                {
                    obj_error = obj_procesar.EfactConsultaEstado(request.identificador, obj_token.mensaje);
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

        [HttpPost("ObtenerAdjuntosByte")]
        public IActionResult ObtenerAdjuntosByte([FromBody] COBEC_DatosAdjunto request)
        {
            COBEc_Error obj_error = new COBEc_Error();
            byte[] pdfBytes = new byte[0];
            EFACTGeneral obj_procesar = new EFACTGeneral(configuration, this.context);
            try
            {
                pdfBytes = obj_procesar.ObtenerAdjuntosByte(request);                
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception e)
            {
                return BadRequest("ERROR|" + e.Message);
            }

        }
    }
}
