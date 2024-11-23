using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using mouse_lighting.Models;
using mouse_lighting.Services;
using mouse_lighting.ViewModels;
using System.Data;
using System.Windows;

namespace mouse_lighting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? "Assembly.GetExecutingAssembly().GetName().Name is Null ): ";
        public static Window? ActiveWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

        public static Window? FocusedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);
        private static IHost? __Host;

        public static IHost Host => __Host ??= Microsoft.Extensions.Hosting.Host
          .CreateDefaultBuilder(Environment.GetCommandLineArgs())
          .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json", true, true))
          .ConfigureServices((host, services) => services
              .AddViewModels()
              .AddServices(host.Configuration)
          )
          .Build();

        public static IServiceProvider Services => Host.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (Host != null)
            {
                using (Host) { await Host.StopAsync(); };
            }
        }
    }

}
