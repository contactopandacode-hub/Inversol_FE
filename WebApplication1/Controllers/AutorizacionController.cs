//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//namespace ServicioNetCore.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class AutorizacionController : ControllerBase
//    {
//        private readonly IConfiguration configuration;
//        public DbContext context { get; set; }

//        public AutorizacionController(IConfiguration _configuration, DbContext _context)
//        {
//            configuration = _configuration;
//            this.context = _context;
//        }

//        [HttpPost("[action]")]
//        public SpringToken login([FromBody] Usuario usuario)
//        {

//            SpringToken springToken = new SpringToken();
//            String respuesta = String.Empty;
//            String Mensaje = string.Empty;

//            DbCommand _command = context.Database.GetDbConnection().CreateCommand();
//            try
//            {
//                _command.CommandType = System.Data.CommandType.StoredProcedure;
//                _command.CommandText = "SNP_API_Login";
//                _command.Connection.Open();

//                SqlParameter pUsuario = new SqlParameter("@USUARIO", usuario.usuario);
//                _command.Parameters.Add(pUsuario);

//                SqlParameter pClave = new SqlParameter("@CLAVE", usuario.clave);
//                _command.Parameters.Add(pClave);


//                DbDataReader _reader = _command.ExecuteReader();

//                while (_reader.Read())
//                {
//                    if (!_reader.IsDBNull(0))
//                        respuesta = _reader.GetString(0);
//                    if (!_reader.IsDBNull(1))
//                        Mensaje = _reader.GetString(1);

//                }
//                _reader.Close();
//                _reader.Dispose();

//                _command.Dispose();

//            }
//            catch (Exception ex)
//            {
//                springToken.Codigo = "01";
//                springToken.Mensaje = ex.ToString();
//                return springToken;
//            }
//            finally
//            {
//                _command.Connection.Close();
//            }

//            if (!respuesta.Equals("S"))
//            {
//                springToken.Codigo = "01";
//                springToken.Mensaje = Mensaje;
//                return springToken;
//            }

//            // generando el nuevo token
//            var claims = new[]
//                     {
//                        new Claim(JwtRegisteredClaimNames.Azp , "AZP"),
//                        new Claim(JwtRegisteredClaimNames.Aud, "AUD"),
//                        new Claim(JwtRegisteredClaimNames.Sid, "SID"),
//                        new Claim(JwtRegisteredClaimNames.Typ , "TYP"),
//                        new Claim(JwtRegisteredClaimNames.Jti , usuario.usuario),
//                    };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

//            // Int32 expire = Convert.ToInt32(configuration["Tokens:Expire_IN"]);

//            var token = new JwtSecurityToken(configuration["Tokens:Issuer"],
//            configuration["Tokens:Audience"],
//            claims,
//            // expires: DateTime.Now.AddHours(expire),
//            expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
//            signingCredentials: creds);

//            // asignar la clave a devolver
//            springToken.Codigo = "00";
//            springToken.Mensaje = "Exitoso";
//            springToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
//            springToken.Expires_In = new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime.ToString();
//            springToken.refresh_token = refresh_token.ToString();
//            // guardar en cache el usuario actual
//            return springToken;
//        }

//        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        //[HttpGet("[action]")]
//        //public Test testPrivado()
//        //{
//        //    return new Test() { test = "con token" };
//        //}
//    }

//    public class Usuario
//    {
//        public String usuario { get; set; }
//        public String clave { get; set; }
//    }

//    public class SpringToken
//    {
//        public string Codigo { get; set; }
//        public string Mensaje { get; set; }
//        public string Expires_In { get; set; }
//        public string Token { get; set; }
//         public string refresh_token { get; set; }
//    }

//    public class Test
//    {
//        public String test { get; set; }
//    }
//}
