using mouse_lighting.Infrastructure.Commands;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.ViewModels.Base;
using System.Windows.Input;

namespace mouse_lighting.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        private IDataTransferBetweenViews _DataTransfer;
        private readonly IUserDialog _UserDialog;
        private readonly IDataService _DataService;

        #region Status : string - Статус

        /// <summary>Статус</summary>
        private string _Status = "Готов!";

        /// <summary>Статус</summary>
        public string Status { get => _Status; set => Set(ref _Status, value); }

        #endregion


        private string _PathToDb;
        public string PathToDb
        {
            get
            {
                return _PathToDb;
            }

            set
            {
                Set(ref _PathToDb, value);
            }
        }


        private string _PathToXML;
        public string PathToXML { get => _PathToXML; set => Set(ref _PathToXML, value); }


        public MainViewModel(IUserDialog UserDialog, IDataService DataService, IDataTransferBetweenViews DataTransfer)
        {
            _DataTransfer = DataTransfer;
            _UserDialog = UserDialog;
            _DataService = DataService;
            _DataTransfer.PrintInStatusEvent += PrintInStatus;
            PathToDb = DataService.Setting.PathToDb;
            PathToXML = DataService.Setting.PathToXML;

        }
        internal void PrintInStatus(string status)
        {
            Status = status;
        }


        #region UpdateSettingCommand - описание команды 
        private LambdaCommand _UpdateSettingCommand;
        public ICommand UpdateSettingCommand => _UpdateSettingCommand ??=
            new LambdaCommand(OnUpdateSettingCommandExecuted, CanUpdateSettingCommandExecute);
        private bool CanUpdateSettingCommandExecute(object p) => true;
        private void OnUpdateSettingCommandExecuted(object p)
        {
            _DataService.Setting.PathToXML = PathToXML;
            _DataService.Setting.PathToDb = PathToDb;
            _DataService.SaveSetting();
        }
        #endregion

    }
}

