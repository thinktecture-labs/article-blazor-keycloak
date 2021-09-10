using Blazor.Keycloak.Api.Services;
using Blazor.Keycloak.Api.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Blazor.Keycloak.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ConferencesService>();
            services.AddScoped<ContributionsService>();
            services.AddScoped<SpeakerService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blazor.Keycloak.Api", Version = "v1" });
            });

            var authority = Configuration.GetValue<string>("IdentityProvider:Authority");
            var audience = Configuration.GetValue<string>("IdentityProvider:Audience");

            services
                .AddTransient<IClaimsTransformation>(_ => new KeycloakRolesClaimsTransformation("roles", audience))
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    if (Environment.IsDevelopment())
                    {
                        options.BackchannelHttpHandler = new HttpClientHandler()
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                        };
                    }

                    options.Authority = authority;
                    options.Audience = audience;
                    options.RequireHttpsMetadata = Environment.IsProduction();
                    options.TokenValidationParameters.ValidateIssuer = !authority.StartsWith("http://localhost:8080");
                    options.SaveToken = true;
                    options.TokenValidationParameters.RoleClaimType = "roles";
                    options.TokenValidationParameters.NameClaimType = "preferred_username";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor.Keycloak.Api v1"));

                // DO NOT DO THIS IN PRODUCTION!
                app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(o => true));
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}