using mouse_lighting.Services.DataService;
using mouse_lighting.Services.Observer;
using mouse_lighting.Services.UserDialog;
using mouse_lighting.ViewModels.Base;

namespace mouse_lighting.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly IUserDialog _UserDialog;
        private readonly IDataService _DataService;
        private readonly IObserverStatusBar _ObserverStatusBar;
        
        private string _Title = "Lighting for the bloody mouse";
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #region Status bar
        private string status = "Готово";
        public string Status { get => status; set => Set(ref status, value); }

        private string _StatusBarForegroundColor = "#000000";
        public string StatusBarForegroundColor { get => _StatusBarForegroundColor; set => Set(ref _StatusBarForegroundColor, value); }

        private void UpdateStatusBar(string message, System.Drawing.Color color)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Status = message;
                StatusBarForegroundColor = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }
        #endregion

        public MainWindowViewModel(IDataService dataService, IUserDialog userDialog, IObserverStatusBar ObserverStatusBar)
        {
            _UserDialog = userDialog;
            _DataService = dataService;
            _ObserverStatusBar = ObserverStatusBar;
        }
    }
}
