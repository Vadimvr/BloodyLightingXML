using Microsoft.Extensions.DependencyInjection;

namespace mouse_lighting.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
        //public LightingViewModel LightingViewModel => App.Services.GetRequiredService<LightingViewModel>();
        //public LightingCycleViewMode LightingCycleViewModel => App.Services.GetRequiredService<LightingCycleViewMode>();
        //public MainViewModel MainViewModel => App.Services.GetRequiredService<MainViewModel>();
        //public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public TaskbarViewModel TaskbarViewModel => App.Services.GetRequiredService<TaskbarViewModel>();
        // new
        public StatusBarViewModel StatusBarViewModel => App.Services.GetRequiredService<StatusBarViewModel>();
        public MenuViewModel MenuViewModel => App.Services.GetRequiredService<MenuViewModel>();
        public PathsViewModel PathsViewModel => App.Services.GetRequiredService<PathsViewModel>();
        public LightingViewModel LightingViewModel => App.Services.GetRequiredService<LightingViewModel>();
        public CyclesViewModel CyclesViewModel => App.Services.GetRequiredService<CyclesViewModel>();
    }
    internal static class ViewModelsRegistrar
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
           .AddSingleton<MenuViewModel>()
           .AddSingleton<StatusBarViewModel>()
           .AddSingleton<PathsViewModel>()
           .AddSingleton<LightingViewModel>()
           .AddSingleton<CyclesViewModel>()

           .AddSingleton<MainWindowViewModel>()
           .AddSingleton<TaskbarViewModel>()

           //.AddSingleton<LightingViewModel>()
           //.AddSingleton<LightingCycleViewMode>()
           //.AddSingleton<MainViewModel>()
        ;
    }
}