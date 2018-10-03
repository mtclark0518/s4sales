using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using S4Sales.Identity;
using S4Sales.Log;
using S4Sales.Models;
using S4Sales.Services;
namespace S4Sales
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            //  policies will depend on how hsmv wants to handle money transfers
            // add authorization policy
            // services.AddAuthorization(opts =>
            // {
            //     opts.AddPolicy("admin", policy => policy.RequireRole("admin"));
            //     opts.AddPolicy("user", policy => policy.RequireRole("user"));
            // });       

            // session services
            services.AddDistributedMemoryCache();
            services.AddSession( opts =>
            {
                opts.IdleTimeout = TimeSpan.FromMinutes(60);
                opts.Cookie.Name = "S4Sales.Session";
            });
            services.AddTransient<SessionUtility>();

            // add identity services
            services.AddIdentity<S4Identity, S4IdentityRole>( opts =>
            {
                opts.Lockout.AllowedForNewUsers = false;
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 1;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequiredUniqueChars = 0;
                opts.SignIn.RequireConfirmedEmail = false;  
                opts.User.RequireUniqueEmail = true;
            });

            // add custom user && roles stores
            services.AddTransient<IUserStore<S4Identity>, S4IdentityStore>();
            services.AddTransient<IRoleStore<S4IdentityRole>, S4RoleStore>();

            // adds authentication cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.ConfigureApplicationCookie(opts=>
            {
                opts.Cookie.Name = "S4Sales.App";
                opts.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                opts.LoginPath = new PathString("/api/identity/login");
            });

            // add identity services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILookupNormalizer, S4LookupNormalizer>();
            services.AddScoped<IUserValidator<S4Identity>, UserValidator<S4Identity>>();
            services.AddScoped<IPasswordValidator<S4Identity>, PasswordValidator<S4Identity>>();
            services.AddScoped<IPasswordHasher<S4Identity>, S4PasswordHasher<S4Identity>>();
            services.AddScoped<IRoleValidator<S4IdentityRole>, RoleValidator<S4IdentityRole>>();
            services.AddScoped<IdentityErrorDescriber>();
            services.AddScoped<IUserClaimsPrincipalFactory<S4Identity>, S4UserClaimsPrincipalFactory<S4Identity, S4IdentityRole>>();
            
            // configures asp.net Identity managers to work with custom stores
            services.AddScoped<UserManager<S4Identity>>();
            services.AddScoped<SignInManager<S4Identity>>();
            services.AddScoped<RoleManager<S4IdentityRole>>();
            
            // add data access layers
            services.AddSingleton<AgencyManager>();
            services.AddSingleton<CartStore>();
            services.AddSingleton<CommerceRepository>();
            services.AddSingleton<DataRepository>();
            services.AddSingleton<SearchRepository>();

            // additonal services
            services.AddSingleton<Logg>();
            services.AddSingleton<DownloadToken>();
            services.AddSingleton<S4Emailer>();
            services.AddSingleton<StripeService>();

            services.AddMvc();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(options =>
            {
                options.RootPath = "Client/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddConsole(Configuration.GetSection("Logging"));
            log.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSession();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Client";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}