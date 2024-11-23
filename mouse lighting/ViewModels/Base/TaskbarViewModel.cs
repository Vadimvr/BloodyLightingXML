using mouse_lighting.ViewModels.Base;

namespace mouse_lighting.ViewModels
{
    internal class TaskbarViewModel : ViewModelBase
    {
        public TaskbarViewModel()
        {
            ToolTipText = App.AppName;
        }

        private string _ToolTipText = string.Empty;
        public string ToolTipText { get => _ToolTipText; set => Set(ref _ToolTipText, value); }
    }
}
