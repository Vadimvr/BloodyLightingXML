using mouse_lighting.Models;
using mouse_lighting.Services.Observer;
using mouse_lighting.ViewModels.Base;

namespace mouse_lighting.ViewModels
{
   internal class StatusBarViewModel : ViewModelBase
    {
        private readonly StatusBarModel _StatusBarModel;
        private string? _Status;
        public string? Status { get => _Status; set => Set(ref _Status, value); }

        private string _Color = default!;
        public string Color { get => _Color; set => Set(ref _Color, value); }

        public StatusBarViewModel(IObserverStatusBar observerStatusBar)
        {
            _StatusBarModel = new StatusBarModel(UpdateStatusBar);
            ((ObserverStatusBar)observerStatusBar).UpdateStatusBarEvent(_StatusBarModel.UpdateStatusBar);
            observerStatusBar.StatusBar("Ready");
        }

        private void UpdateStatusBar()
        {
            if (_StatusBarModel != null)
            {
                Color = _StatusBarModel.Color;
                Status = _StatusBarModel.Message;
            }
        }
    }
}
