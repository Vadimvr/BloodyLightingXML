using Microsoft.Extensions.DependencyInjection;
using mouse_lighting.Services.DataService;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.Services.Observer;
using mouse_lighting.Services.UserDialog;

namespace mouse_lighting.Services
{
    internal static class ServicesRegistrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IDataService, DataService.DataService>();
            services.AddTransient<IUserDialog, UserDialogService>();
            //TODO 
            services.AddSingleton<IObserverStatusBar, ObserverStatusBar>();
            services.AddSingleton<ILocalData, LocalData>();
            services.AddSingleton<IDataTransferBetweenViews, DataTransferBetweenViews>();


            return services;
        }
    }
}
