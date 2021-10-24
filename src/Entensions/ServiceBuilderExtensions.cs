using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace poc_asp_net_core_rabbitmq_producer.Extensions
{
    using Microsoft.Extensions.Hosting;
    using System.IO;
    using System.Net.Http;

    public static class ServiceBuilderExtensions
    {
        public static IServiceCollection AddApiInfra(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, string xmlCommentsFilePath)
        {
            services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            services
                .AddMvc()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddHttpContextAccessor();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddDocumentation("Poc Asp.net core rabbitMQ Producer API", "Poc Asp.net core rabbitMQ Producer API", xmlCommentsFilePath);

            services.AddScoped<IConnectionFactory, ConnectionFactoryCreator>();

            return services;
        }
        public static IServiceCollection AddDocumentation(this IServiceCollection services, string title, string descricao, string xmlDocumentationFilePath, string version = "v1")
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Version = version,
                    Title = title,
                    Description = descricao,
                });

                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
                c.IncludeXmlComments(xmlDocumentationFilePath);
                c.CustomSchemaIds(x => x.FullName);
            });
            return services;
        }
    }
}
