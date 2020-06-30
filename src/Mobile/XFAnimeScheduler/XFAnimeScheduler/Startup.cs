using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFAnimeScheduler.Services;
using XFAnimeScheduler.ViewModels;
using XFAnimeScheduler.Views;

namespace XFAnimeScheduler
{
    public class Startup
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resName = assembly.GetManifestResourceNames()?.FirstOrDefault(r => r.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase));

            using var stream = assembly.GetManifestResourceStream(resName);

            var host = new HostBuilder().ConfigureHostConfiguration(c =>
                {
                    c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                    c.AddJsonStream(stream);
                })
                .ConfigureServices((c, x) =>
                {
                    nativeConfigureServices(c, x);
                    ConfigureServices(c, x);
                })
                .ConfigureLogging(l => l.AddConsole(o =>
                {
                    o.DisableColors = true;
                }))
                .Build();

            ServiceProvider = host.Services;

            return ServiceProvider.GetService<App>();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            //Add Services here
            //ex:View, ViewModel, DataService, Notification Service etc.
            //services.AddTransient<MainPage>();

            services.AddSingleton<IDataService, DataService>();
            services.AddTransient<AnimeInfosPageViewModel>();
            services.AddTransient<AnimeInfoDetailsPageViewModel>();
            services.AddTransient<Page, AppShell>();
            services.AddSingleton<App>();
        }
    }
}
