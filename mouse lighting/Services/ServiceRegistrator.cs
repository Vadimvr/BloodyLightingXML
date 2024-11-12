using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mouse_lighting.Services.db;
using mouse_lighting.Services.Interfaces;
using static mouse_lighting.Services.Interfaces.IDataTransferBetweenViews;

namespace mouse_lighting.Services
{
    internal static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
           .AddSingleton<IDataTransferBetweenViews, DataTransferBetweenViews>()
           .AddTransient<IDataService, DataService>()
           .AddTransient<IUserDialog, UserDialog>();
    }
}
