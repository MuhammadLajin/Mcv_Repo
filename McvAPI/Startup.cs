using McvAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McvAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        string cor = "";
        public void ConfigureServices(IServiceCollection services)
        {
            //Register Dependency using sql and use UseLazyLoadingProxies
            services.AddDbContext<MCVDataBaseContext>(option => option.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("McvDB")));

            //NewtonsoftJson to prevent ReferenceLoopHandling
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //add swagger document and use for help page
            services.AddSwaggerDocument();

            //enable the cores(cross origin resource sharings) to be able to be consumed in the ajax calls
            services.AddCors(options =>
            {
                options.AddPolicy(cor,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        
                        //builder.SetIsOriginAllowed(o => o. ).AllowAnyHeader()/.AllowAnyMethod();
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "McvAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseHttpsRedirection();

                //app.UseSwagger();
                app.UseOpenApi();
                app.UseSwaggerUi3();
                
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "McvAPI v1"));
            }

            //app cores to origins
            app.UseCors(cor);

            //to allow use authorization
            app.UseRouting();

            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
