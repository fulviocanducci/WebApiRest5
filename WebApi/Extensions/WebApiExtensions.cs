using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using WebApi.Services;
using WebApi.Settings;

namespace WebApi.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSigningAndTokenConfigurations
        (
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            SigningConfigurations signingConfigurations = new SigningConfigurations();
            TokenConfigurations tokenConfigurations = configuration
                .GetSection("TokenConfigurations")
                .Get<TokenConfigurations>();

            services.AddSingleton(tokenConfigurations);
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = signingConfigurations.Key,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuers = tokenConfigurations.Issuer,
                    ValidateAudience = true,
                    ValidAudiences = tokenConfigurations.Audience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                };
            });

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerGenConfigurations(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Web Api",
                        Version = "v1",
                        Description = "Web Api",
                        Contact = new OpenApiContact()
                        {
                            Email = "fulviocanducci@hotmail.com",
                            Name = "Fúlvio Cezar Canducci Dias",
                        }
                    });
                c.IncludeXmlComments(
                    Path.Combine(
                        PlatformServices.Default.Application.ApplicationBasePath,
                        $"{PlatformServices.Default.Application.ApplicationName}.xml"
                    )
                );
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Bearer Token Authentication",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGlobalConfigurations(this IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<DataService>();
            services.AddScoped<TokenService>();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            return services;
        }
    }
}
