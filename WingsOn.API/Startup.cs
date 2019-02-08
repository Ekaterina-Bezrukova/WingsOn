using System;
using Autofac;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WingsOn.API.BusinessLogic.Contracts;
using WingsOn.API.BusinessLogic.Services;
using WingsOn.API.ExceptionHandling;
using WingsOn.Dal;
using WingsOn.Domain;

namespace WingsOn.API
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
            services.AddOptions();

            services.AddRouting(
                options =>
                {
                    options.LowercaseUrls = true;
                }
            );
            services
               .AddMvcCore(
                   options =>
                   {
                       options.Filters.Add<ApiExceptionFilterAttribute>();
                   }
               );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WingsOn API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint(
                        $"/swagger/v1/swagger.json",
                        "v1");
                }
            );
            app.UseMvc();
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(c => LogManager.GetLogger(type: typeof(Object))).As<ILog>();
            builder.RegisterType<PersonService>().As<IPersonService>();
            builder.RegisterType<BookingService>().As<IBookingService>();
            builder.RegisterType<PersonRepository>().As<IRepository<Person>>().SingleInstance();
            builder.RegisterType<BookingRepository>().As<IRepository<Booking>>().SingleInstance();
            builder.RegisterType<FlightRepository>().As<IRepository<Flight>>().SingleInstance();
        }
    }
}
