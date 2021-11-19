using Core.Models;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;
using WebAPI.Auth;
using WebAPI.Utility;
using Core.Utility;
using AutoWrapper;
using JNogueira.Logger.Discord;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using RazorHtmlEmails.RazorClassLib.Services;
using static RazorHtmlEmails.Common.Services.EmailSenderService;
using RazorHtmlEmails.Common.Models.EmailSender;
using WebAPI.Services;
using System.Linq;
using Microsoft.AspNetCore.ResponseCompression;
using WebAPI.Hubs;
using Microsoft.AspNetCore.HttpOverrides;

namespace WebAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddScoped<ICustomTokenManager, JwtTokenManager>();
            services.AddScoped<ICustomUserManager, CustomUserManager>();


            services.AddSingleton<EmailSenderHostedService>();
            services.AddSingleton<IHostedService>(serviceProvider => serviceProvider.GetService<EmailSenderHostedService>());
            services.AddSingleton<IEmailSender>(serviceProvider => serviceProvider.GetService<EmailSenderHostedService>());
            services.Configure<SmtpOptions>(Configuration.GetSection("Smtp"));


            if (_env.IsDevelopment())
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("MyDb");
                });
            }
            else if(_env.IsStaging() || _env.IsProduction())
            {
                services.AddMemoryCache();

                /*
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
                */
                services.AddDbContext<AppDbContext>(options => {
                    options.UseMySql(Configuration.GetConnectionString("DefaultConnectionMySql"),
                        ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnectionMySql")));
                });
            }

            

            services.AddAutoMapper(Assembly.Load("Core"));

            services.AddControllers();
            services.AddApiVersioning(options=> {
                options.ReportApiVersions = true; // response header shows the supported api versionS* if a user tries to reach an unsupported version
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });

            services.AddVersionedApiExplorer(options=> options.GroupNameFormat = "'v'VVV");
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title ="Web API v1", Version = "version 1"});
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Web API v2", Version = "version 2" });
            });

            

            services.AddCors(options => {
                options.AddPolicy(name: MyAllowSpecificOrigins, 
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

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
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtSecretKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

              services.AddAuthorization(options => {
                  List<string> _claims = new List<string>();
                  _claims.AddRange(Permission.Dashboards._metrics);
                  _claims.AddRange(Permission.Users._metrics);
                  _claims.AddRange(Permission.Tags._metrics);
                  _claims.AddRange(Permission.Roles._metrics);
                  _claims.AddRange(Permission.Products._metrics);
                  _claims.AddRange(Permission.SupplierCompanies._metrics);
                  _claims.AddRange(Permission.SCResponsiblePeople._metrics);
                  _claims.AddRange(Permission.SCRPEmails._metrics);
                  _claims.AddRange(Permission.SCRPPhones._metrics);
                  _claims.AddRange(Permission.BusinessPartners._metrics);
                  _claims.AddRange(Permission.BPResponsiblePeople._metrics);
                  _claims.AddRange(Permission.BPRPEmails._metrics);
                  _claims.AddRange(Permission.BPRPPhones._metrics);
                  _claims.AddRange(Permission.RequestedProducts._metrics);
                  _claims.AddRange(Permission.Offers._metrics);
                  _claims.AddRange(Permission.Requests._metrics);
                  _claims.AddRange(Permission.Chats._metrics);

                  foreach (var item in _claims)
                  {
                      options.AddPolicy(item, builder =>
                      {
                          builder.AddRequirements(new PermissionRequirement(item, CustomClaimTypes.APIPermission));
                      });
                  }
              });

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 7;
            }).AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            
            services.AddSingleton<IEmailSenderUseCases, EmailSenderUseCases>();
            services.AddSingleton<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IWebHostEnvironment env, AppDbContext context, IMemoryCache cache)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Create in-memory db for development
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //Configure OpenAPI
                app.UseSwagger();
                app.UseSwaggerUI(
                    options=> {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1");
                        options.SwaggerEndpoint("/swagger/v2/swagger.json", "WebAPI v2");
                    });
                //error handler for development
                app.UseExceptionHandler("/error-local-development");
            }
            else if(env.IsStaging() || env.IsProduction())
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var entryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.High);
                    var services = serviceScope.ServiceProvider;
                    var myDbContext = services.GetService<AppDbContext>();
                    var nowTicks = DateTime.Now.Ticks;
                    cache.Set("LastPermissionUpdate", new DateTime(nowTicks));
                }
                //error handler
                app.UseExceptionHandler("/error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { 
                UseApiProblemDetailsException = true,
                ExcludePaths = new AutoWrapperExcludePath[] {
                    new AutoWrapperExcludePath("/chathub/.*|/chathub", ExcludeMode.Regex),
                    new AutoWrapperExcludePath("/images.*", ExcludeMode.Regex)
                }
            });

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
