using Microsoft.Extensions.DependencyInjection;
using mouse_lighting.Services.Interfaces;

namespace mouse_lighting.Services
{
    internal static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
           .AddSingleton<ILocalData, LocalData>()
           .AddSingleton<IDataTransferBetweenViews, DataTransferBetweenViews>()
           .AddSingleton<IDataService, DataService>()
           .AddTransient<IUserDialog, UserDialog>();
    }
}
