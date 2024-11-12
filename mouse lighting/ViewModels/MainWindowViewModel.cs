using mouse_lighting.Services.Interfaces;
using mouse_lighting.ViewModels.Base;
namespace mouse_lighting.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private IDataTransferBetweenViews _DataTransfer;
        private readonly IUserDialog _UserDialog;
        private readonly IDataService _DataService;

        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _Title = "Главное окно";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion

        #region Status : string - Статус

        /// <summary>Статус</summary>
        private string _Status = "Готов!";

        /// <summary>Статус</summary>
        public string Status { get => _Status; set => Set(ref _Status, value); }

        #endregion

        public MainWindowViewModel(IUserDialog UserDialog, IDataService DataService, IDataTransferBetweenViews DataTransfer)
        {
            _DataTransfer = DataTransfer;
            _UserDialog = UserDialog;
            _DataService = DataService;
            _DataTransfer.PrintInStatusEvent += PrintInStatus;
        }
        internal void PrintInStatus(string status)
        {
            Status = status;
        }
    }
}