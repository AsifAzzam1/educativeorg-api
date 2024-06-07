using educativeorg_data.Data;
using educativeorg_models;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_services.Services.AccountServices;
using educativeorg_services.Services.PermissionServices;
using educativeorg_services.Services.RoleServices;
using educativeorg_services.Services.SeederServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Security;
using System.Text;

namespace educativeorg_api.Helper
{
    public static class ConfigurationExtension
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Educative Org",
                    Version = "V1",
                    Description = "Educative Org APIs",
                    Contact = new OpenApiContact
                    {
                        Name = "Educative org team",
                    }
                });
                opts.CustomSchemaIds(x => x.ToString());
                opts.ResolveConflictingActions(x => x.FirstOrDefault());
                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                opts.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    new string[]{ }
                    }
                });

            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(ad =>
            {
                ad.Password.RequiredLength = 8;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<EducativeOrgDbContext>().AddDefaultTokenProviders();
        }
        
        public static void ConfigureJWT(this IServiceCollection services)
        {
            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jb =>
            {
                jb.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EducativeOrg_Constants.JWtkey)),
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:7236",
                    ValidAudience = "",
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });
        }

        public static void ConfigureDbContext(this IServiceCollection services, string cs) => 
            services.AddDbContext<EducativeOrgDbContext>(options => options.UseSqlServer(cs));

        public static void ConfigureCors(this IServiceCollection service, AppConfigViewModel config)
        {
            service.AddCors(opts =>
            {
                opts.AddDefaultPolicy(polic =>
                {
                    polic.WithOrigins(config.AllowedOrigins).AllowCredentials().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }
        
        public static void ConfigureControllers(this IServiceCollection service)
        {
             service.AddControllers()
                    .AddNewtonsoftJson(opts =>
                    {
                        opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    }).AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, PermissionAuthHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ISeedService, SeedService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IRoleService, RoleService>();
        }
    }
}
