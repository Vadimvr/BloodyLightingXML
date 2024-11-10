using Microsoft.Extensions.DependencyInjection;

namespace mouse_lighting.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
