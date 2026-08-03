using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicioRSNetCore.Controllers.Clases;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddDbContext<DbContext>(options => options.UseSqlServer(DecryptStringFromBytes_Aes(Configuration.GetConnectionString("dbSpringNetRrhh"))));
         //   services.AddDbContext<DbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("dbSpringNetRrhh")));
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddDbContext<DbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("dbSpringNetRrhh")));

            // seguridad
            services.AddAuthentication().AddJwtBearer(
                cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };

                });
            //Mascara
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Interface de Facturación Electrónica",
                    Version = "v1",
                    Description = "REST API  para Grupo Inversol",
                    Contact = new OpenApiContact()
                    {
                        Name = "Walter Roman Parraga",
                        Email = "walter.roman.wr@gmail.com"
                    }

                });
                c.OperationFilter<MyHeaderFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //        app.UseHsts();
        //    }

        //    app.UseMvc();
        //    app.UseSwagger();
        //    app.UseSwaggerUI(c => {
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Facturación Electronica");
        //    });
        //}
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseStaticFiles(); // Middleware de archivos estáticos

            //app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Facturación Electronica");
            });
        }

        public static string DecryptStringFromBytes_Aes(String text)
        {

            byte[] cipherText = Convert.FromBase64String(text);

            String key = Environment.GetEnvironmentVariable("NETCORE_KEY");
            if (key == null)
                key = "hyb91p4nhvcnlmlkye17uyfz63q5jtcy";
            else if (key.Trim().Length != 32)
                key = "hyb91p4nhvcnlmlkye17uyfz63q5jtcy";

            var Key = Encoding.UTF8.GetBytes(key);
            var IV = Encoding.UTF8.GetBytes(key.Substring(0, 16));

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
