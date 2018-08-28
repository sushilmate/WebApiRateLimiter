using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebApiRateLimiter.Data.Interface;
using WebApiRateLimiter.Data.Repository;
using WebApiRateLimiter.Helpers.Factory;
using WebApiRateLimiter.Helpers.Interface;
using WebApiRateLimiter.Helpers.Providers;

namespace WebApiRateLimiter
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Injecting dependancy in services so it can be accessible in classes.
            services.AddTransient<ICacheSettingProvider, CacheSettingProvider>();
            services.AddTransient<IOrderByFactory, CollectionOrderByFactory>();
            services.AddTransient<IHotelRepository, HotelRepository>();

            // Mapper used for mapping model with viewmodels (POCO)
            services.AddAutoMapper();

            // Using memory cache objects to keep apis hit count in them
            services.AddMemoryCache();

            // Maintaining APIs configuration in the config, configuring them with the class.
            services.Configure<ApiRateLimitPolicies>(Configuration.GetSection("ApiRateLimitPolicies"));

            // Adding swagger as services UI, it will displays the services we provide & also UI to invoke APIs.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Web API Rate Limiter", Version = "v1" });
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

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API Rate Limiter V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}