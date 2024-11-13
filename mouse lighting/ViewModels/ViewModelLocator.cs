using Microsoft.Extensions.DependencyInjection;

namespace mouse_lighting.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public LightingViewModel LightingViewModel => App.Services.GetRequiredService<LightingViewModel>(); 
        public LightingCycleViewMode LightingCycleViewModel => App.Services.GetRequiredService<LightingCycleViewMode>();
    }
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViews(this IServiceCollection services) => services
           .AddSingleton<MainWindowViewModel>()
           .AddSingleton<LightingViewModel>()
           .AddSingleton<LightingCycleViewMode>()
        ;
    }
}
