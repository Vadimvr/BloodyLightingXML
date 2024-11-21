using Microsoft.Extensions.DependencyInjection;

namespace mouse_lighting.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public LightingViewModel LightingViewModel => App.Services.GetRequiredService<LightingViewModel>();
        public LightingCycleViewMode LightingCycleViewModel => App.Services.GetRequiredService<LightingCycleViewMode>();
        public MainViewModel MainViewModel => App.Services.GetRequiredService<MainViewModel>();
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public TaskbarViewModel TaskbarViewModel => App.Services.GetRequiredService<TaskbarViewModel>();
    }
    internal static class ViewModelsRegistrar
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
           .AddSingleton<MainWindowViewModel>()
           .AddSingleton<TaskbarViewModel>()
           .AddSingleton<LightingViewModel>()
           .AddSingleton<LightingCycleViewMode>()
           .AddSingleton<MainViewModel>()
        ;
    }
}