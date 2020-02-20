using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CustomAuthetication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authetication
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

            //No controller actions available unless autheticated.
            services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));

            //Get the services for View production
            services.AddControllersWithViews();
            services.AddSingleton<IPostConfigureOptions<BasicAuthenticationOptions>, BasicAuthenticationPostConfigureOptions>();

            //Only APIs, no view/controller
            //services.AddControllers();
            #region Authetication
            //Cookie based authetication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, act =>
            {
                act.Cookie.HttpOnly = true; //Cookie is only available to servers
                act.LoginPath = "/Login/UserLogin";
                act.AccessDeniedPath = "/Login/UserLogin";
                act.SlidingExpiration = true;
            });

            //Example of custom authetication implementation
            //services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
            //    .AddBasic<AutheticationService>(o => o.Realm = "My App");


            //services.AddAuthentication(x =>
            //    {
            //        x.DefaultAuthenticateScheme = MyJwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = MyJwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddMyJwtBearer(x =>
            //    {
            //        x.RequireHttpsMetadata = false;
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            //When using certificate for signing token
            //            IssuerSigningKey = GetSecurityKey(),
            //            //When using symmetric security with shared key.
            //            //IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateIssuer = false,
            //            ValidateAudience = false
            //        };
            //    });
            #endregion
            #region Authorization

            //Authorization policies

            /*policyBuilder.RequireClaim(claim, values)
                            AddRequirements()
                            AddAuthenticationSchemes()
                            RequireAssertion() // Executes the provided lambda function, which returns a bool.
                            RequireAuthenticatedUser()
                            RequireRole()
                            RequireUserName()
            */
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanEnterSecurity",
                    policyBuilder => policyBuilder.RequireClaim("BoardingPassNumber"));

                //IAuthorization Requirement. These requirement needs handlers.
                options.AddPolicy("CanSeeHome", policyBuilder => policyBuilder.AddRequirements(
                    new RequirementOne(), new RequirementTwo(18)));
            });

            //Register the Authorization requirement handlers
            /* Multiple handlers for one requirement */
            services.AddSingleton<IAuthorizationHandler, RequirementOneHandler1>();
            services.AddSingleton<IAuthorizationHandler, RequirementOneHandler2>();

            services.AddSingleton<IAuthorizationHandler, Requirement2OneHandler>();

            #endregion
        }

        private SecurityKey GetSecurityKey()
        {
            X509Certificate2 cert = new X509Certificate2("C:\\temp\\vipul.pfx", "1234");
            SecurityKey key = new X509SecurityKey(cert); //SecurityAlgorithms.RsaSha256
            return key;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseAuthentication();
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

            app.UseAuthorization();


            //Here we configure routes..
            //Endpoint Routing is creating a route table across the different sets of middleware.

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=UserLogin}/{id?}");

                //Automatically adds endpoints for public actions in the controllers
                //endpoints.MapControllers();
            });

        }
    }


}
