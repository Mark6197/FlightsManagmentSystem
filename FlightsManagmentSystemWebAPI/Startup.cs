using AutoMapper;
using FlightsManagmentSystemWebAPI.Authentication;
using FlightsManagmentSystemWebAPI.Configuration;
using FlightsManagmentSystemWebAPI.CountriesManagerService;
using FlightsManagmentSystemWebAPI.Mappers;
using FlightsManagmentSystemWebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FlightsManagmentSystemWebAPI
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
            services.AddSingleton<ICountriesManager, CountriesManager>();

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FlightProfile(provider.GetService<ICountriesManager>()));
                cfg.AddProfile(new TicketProfile());
                cfg.AddProfile(new AdministratorProfile());
                cfg.AddProfile(new CustomerProfile());
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new AirlineCompanyProfile());
                cfg.AddProfile(new CountryProfile());
            }).CreateMapper());

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;//Removes the ProblemDetails body generated for ststus code of 400+
            });


            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();//read the values required to configure the jet token from the configuration file
            services.AddSingleton(jwtTokenConfig);//register the JwtTokenConfig as a singleton

            var departuresAndLandingConfig = Configuration.GetSection("departuresAndLandingConfig").Get<DeparturesAndLandingConfig>();//read the values required to configure the jet token from the configuration file
            services.AddSingleton(departuresAndLandingConfig);//register the JwtTokenConfig as a singleton

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();//Register JwtAuthManager as singleton
            services.AddHostedService<JwtRefreshTokenCache>();//Register JwtRefreshTokenCache as Hosted Service which implement background task running on a timer

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlightsManagmentSystemWebAPI", Version = "v1" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Please insert JWT with Bearer into field",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                //Read the comments from the xml file that is being auto genrated on build,
                //this file includes the comments from the contollers
                //We configure to auto genreate the file in csproj file: <GenerateDocumentationFile>true</GenerateDocumentationFile>
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //Add cors policy to expose all the endpoints in the web api to any origin (might restrict it later to specific origin)
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightsManagmentSystemWebAPI v1");
                    c.DocumentTitle = "Flights Managment System API";
                });
            }

            //Use the cors policy defined in ConfigureServices
            app.UseCors("CorsPolicy");

            //Use the custom logging middleware
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
