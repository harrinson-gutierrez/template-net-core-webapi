using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Linq;

namespace Infrastructure.Resources
{
    public static class ServiceExtension
    {
        public static void AddCustomLocalization(this IServiceCollection services)
        {
            services.AddLocalization(
            opts =>
            {
                opts.ResourcesPath = "Resources";
            }
        );

            services.AddMvcCore()
                    .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
              opts =>
              {
                  var supportedCultures = new List<CultureInfo>
                  {
                     new CultureInfo("es")
                  };
                  opts.DefaultRequestCulture = new RequestCulture("es");
                  opts.SupportedCultures = supportedCultures;
                  opts.SupportedUICultures = supportedCultures;

                  opts.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                  {
                      var userLangs = context.Request.Headers["Accept-Language"].ToString();
                      var firstLang = userLangs.Split(',').FirstOrDefault();
                      var defaultLang = string.IsNullOrEmpty(firstLang) ? "es" : firstLang;
                      return Task.FromResult(new ProviderCultureResult(defaultLang, defaultLang));
                  }));
              });
        }

        public static void ConfigureCustomLocalization(this IApplicationBuilder app)
        {
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
        }
    }
}
