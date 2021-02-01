using HRS.Data;
using HRS.Domain;
using HRS.Services;
using HRS.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace HRS
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
            services
                .AddOptions()
                .Configure<ConnectionStringSettings>(Configuration.GetSection("ConnectionStrings"));

            services.AddDistributedMemoryCache();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(10000);
                options.Cookie.Name = "_HR.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });





            services.AddScoped<IAuthenticationSessionProvider, AuthenticationSessionProvider>();
            services.AddSingleton<IConnectionResolver<NpgsqlConnection>, SqlConnectionResolver>();
            services.AddTransient<IUserRepository, UserRepository>();
            //services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            services.AddScoped<IAuthenticationService, AuthenticationService>();


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
