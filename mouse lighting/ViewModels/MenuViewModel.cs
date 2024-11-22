using mouse_lighting.Commands.Base;
using mouse_lighting.Services.UserDialog;
using mouse_lighting.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace mouse_lighting.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        IUserDialog _UserDialog;

        public MenuViewModel(IUserDialog userDialog)
        {
            _UserDialog = userDialog;
        }


        #region ExitFromAppCommand - описание команды 
        private LambdaCommand? _ExitFromAppCommand;
        public ICommand ExitFromAppCommand => _ExitFromAppCommand ??=
            new LambdaCommand(OnExitFromAppCommandExecuted, CanExitFromAppCommandExecute);
        private bool CanExitFromAppCommandExecute(object? p) => p is Window;
        private void OnExitFromAppCommandExecuted(object? p) => (p as Window)?.Close();
        #endregion
    }
}
