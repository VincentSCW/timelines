using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Timelines.OAuth2Provider;
using TimelinesAPI.Authentication;
using TimelinesAPI.DataVaults;
using TimelinesAPI.Settings;

namespace TimelinesAPI
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
            services.AddSingleton(sp => new StorageAccountSettings
            {
				Key = Environment.GetEnvironmentVariable("STORAGEACCOUNT_KEY"),
				ConnectionString = Environment.GetEnvironmentVariable("STORAGEACCOUNT_CONNECTIONSTRING")
            });
	        var jwtSettings = new JwtSettings
	        {
		        Secret = Environment.GetEnvironmentVariable("JWT_SECRET")
	        };

			services.AddSingleton(sp => jwtSettings);
	        services.AddSingleton(sp => new OpenIdAuthorization
	        {
		        LinkedInConfig = new OAuth2Config
		        {
					ClientId = Environment.GetEnvironmentVariable("LINKEDIN_CLIENTID"),
					ClientSecret = Environment.GetEnvironmentVariable("LINKEDIN_CLIENTSECRET"),
					RedirectUrl = Environment.GetEnvironmentVariable("LINKEDIN_REDIRECT")
				}
	        });
	        services.AddSingleton<OAuth2ProviderFactory>();

	        services.AddSingleton<SimpleCacheService<List<MomentEntity>>>();
	        services.AddSingleton<SimpleCacheService<List<TimelineEntity>>>();

			services.AddSingleton<MomentTableStorageVaults>();
	        services.AddSingleton<TimelineTableStorageVaults>();
	        services.AddSingleton<BlobStorageVaults>();

	        services.AddAuthenticationOAuth(jwtSettings);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Timelines API Client",
                    Version = "v1",
                    TermsOfService = "None"
                });

                c.DescribeAllEnumsAsStrings();
            });

	        services.Configure<FormOptions>(x =>
	        {
		        x.ValueLengthLimit = int.MaxValue;
		        x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
	        });
			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
	        app.UseAuthentication();
			app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                    !System.IO.Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.Contains("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timelines API Client");
            });
        }
    }
}
