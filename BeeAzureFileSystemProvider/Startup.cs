using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BeeFreeAzureFileSystemProvider.Models.BasicAuthentication;
using BeeFreeAzureFileSystemProvider.Models.BasicAuthentication.Events;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using BeeFreeAzureFileSystemProvider.AppCode;

namespace BeeFreeAzureFileSystemProvider
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
                .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasicAuthentication(
                    options =>
                    {
                        options.Realm = "BasicAuthentication";
                        options.Events = new BasicAuthenticationEvents
                        {
                            OnValidatePrincipal = context =>
                            {
                                if (Common.IsAuthorized(context.UserName, context.Password, Configuration.GetConnectionString("DataConnection")))
                                {
                                    var claims = new List<Claim>
                                    {
                                        new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer)
                                    };

                                    var ticket = new AuthenticationTicket(
                                        new ClaimsPrincipal(new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme)),
                                        new AuthenticationProperties(),
                                        BasicAuthenticationDefaults.AuthenticationScheme);

                                    return Task.FromResult(AuthenticateResult.Success(ticket));
                                }

                                return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
                            }
                        };
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

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
