using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service.Contract;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;

namespace CompanyEmployees.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCORS(this IServiceCollection Services)
        {
            Services.AddCors(Options =>
            {
                Options.AddPolicy("CorsPolicy", builder =>
                {
                     builder.AllowAnyOrigin();
                     builder.AllowAnyMethod();
                     builder.AllowAnyHeader();
                     builder.WithExposedHeaders("X-Pagination");
                 });
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection Services)
        {
            Services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
 services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureSqlContext(this IServiceCollection services,IConfiguration configuration) =>
             services.AddDbContext<RepositoryContext>(opts =>
                      opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureGlobalExceptionHandler(this IApplicationBuilder app,ILoggerManager logger)
        {

            app.UseExceptionHandler(appError =>
            appError.Run(async context =>
            {
               context.Response.ContentType = "application/json";

            var ContextFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (ContextFeature != null)
            {
                    context.Response.StatusCode = ContextFeature.Error switch
                    {
                        BadRequestException => StatusCodes.Status400BadRequest,
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    };

                logger.LogError($"Something Went Wrong at =>{ ContextFeature.Error}");

                    await context.Response.WriteAsync(new ErrorModel()
                    {
                        StatusCode = context.Response.StatusCode,
                        StatusMessage = ContextFeature.Error.Message
                    }.ToString());

                    await context.Response.CompleteAsync();

                }
            }
            )
            );
        }



        public static void AddCustomMediaTypes(this IServiceCollection services) 
        { 
            services.Configure<MvcOptions>(config => 
            { 
                var systemTextJsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault(); 
                if (systemTextJsonOutputFormatter != null) 
                {                  
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+json"); 
                } 
                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault(); 
                if (xmlOutputFormatter != null) 
                {                   
                   xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+xml");
                }
            }
         ); 
        }
    }



}
