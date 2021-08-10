using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Tastee.Application.Interfaces;
using Tastee.Application.Services;
using Tastee.Infrastructure.IoC;
using Tastee.Infrastucture.Data.Context;
using Tastee.WebApi.HealthChecks;

namespace TasteeWebApi
{
    public class Startup
    {
        private readonly bool _enableSwagger = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var appSettings = Configuration.GetSection("Appsettings");
            _enableSwagger = bool.Parse(appSettings.GetValue<string>("EnableSwagger") ?? "false");
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            var awsOption = Configuration.GetAWSOptions();
            awsOption.Credentials = new BasicAWSCredentials(Configuration["AWS:AccessKey"], Configuration["AWS:SecretKey"]);
            services.AddDefaultAWSOptions(awsOption);
            services.AddAWSService<IAmazonS3>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<TransferUtility>();
            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
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
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            if (_enableSwagger)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Tastee Api",
                        Version = "v1",
                        Contact = new OpenApiContact { Email = "minhthu2511@gmail.com", Name = "Nguyễn Minh Thư" },
                        Description = "Api for integrate on front end and mobile app",
                    });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type= ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name="Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
                    c.IncludeXmlComments(string.Format(@"{0}\TasteeWebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                });
            }

            services.AddHealthChecks()
                .AddAsyncCheck("google_ping_check", () => HealthCheckHelpers.GenerateHealthCheckResultFromPIngRequest("google.com"))
                .AddAsyncCheck("microsoft_ping_check", () => HealthCheckHelpers.GenerateHealthCheckResultFromPIngRequest("microsoft.com"))
                .AddAsyncCheck("forecast_time_check", () => HealthCheckHelpers.RouteTimingHealthCheck("/weatherforecast"))
                .AddAsyncCheck("forecast_time_check_slow", () => HealthCheckHelpers.RouteTimingHealthCheck("/weatherforecast/slow"))
                ;
            
            //services.AddDbContext<tasteeContext>(options =>
            //{
            //    options.UseSqlServer(@"Connection_string_here");
            //});



            RegisterServices(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use( async(context,next) =>
            {
                HealthCheckHelpers.BaseUrl = $"{ context.Request.Scheme }://{context.Request.Host}";
                await next();
            });
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckHelpers.WriteResponses
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (_enableSwagger)
            {
                #region Swagger

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tastee Api");
                    //To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string:
                    c.RoutePrefix = "docs";
                });

                #endregion Swagger
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            #region Cors policy

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            #endregion Cors policy

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/database", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("database"),
                    ResponseWriter = HealthCheckHelpers.WriteResponses
                });

                endpoints.MapHealthChecks("/health/websites", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("websites"),
                    ResponseWriter = HealthCheckHelpers.WriteResponses
                });

                endpoints.MapControllers();
            });

            // Browse images (static file)
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), Configuration["Path:UploadImagePath"])),
            //    RequestPath = Configuration["Path:BrowserImagePath"]
            //});

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //            Path.Combine(Directory.GetCurrentDirectory(), Configuration["Path:UploadImagePath"])),
            //    RequestPath = Configuration["Path:BrowserImagePath"]
            //});
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            DependencyContainer.RegisterServices(services, configuration);
        }
    }
}