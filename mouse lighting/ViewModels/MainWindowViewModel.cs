using mouse_lighting.ViewModels.Base;

namespace mouse_lighting.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _Title = "Lighting for the bloody mouse";
        public string Title { get => _Title; set => Set(ref _Title, value); }

        public MainWindowViewModel() { }
    }
}
