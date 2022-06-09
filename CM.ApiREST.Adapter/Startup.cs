using CM.ApiREST.Adapter.DTOs;
using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models;
using CM.DominioApi.Port.Models.Addreses;
using CM.DominioApi.Port.Ports;
using CM.Persistence.Adapter.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ApiREST.Adapter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();

            //Entities
            services.AddScoped<IBaseRepository<Enterprise, int>, EnterpriseRepository>();
            services.AddScoped<IClientRepository<Client,int>, ClientRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            //DbContext
            services.AddDbContext<ApplicationDbContext>();

            //Mappers
            services.AddAutoMapper(config =>
            {
                config.CreateMap<EnterpriseDTO, Enterprise>();
                config.CreateMap<ClientDTO, Client>();
                config.CreateMap<AddressDTO, Address>();
                config.CreateMap<CountryDTO, Country>();
                config.CreateMap<CityDTO, City>();

            }, typeof(Startup));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CM.ApiREST.Adapter", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CM.ApiREST.Adapter v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
