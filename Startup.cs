using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions;
using URL_shortener.Router;
using URL_shortener.Models;

namespace URL_shortener
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            //services.AddNHibernate();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            
            var routebuilder = new RouteBuilder(app);
            routebuilder.Routes.Add(new ShortUrlRoute());
            app.UseRouter(routebuilder.Build());
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Urls}/{action=Index}");
            });
        }
    }
}
