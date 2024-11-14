using Models;
using mouse_lighting.Infrastructure.Commands;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.ViewModels.Base;
using System;
using System.IO;
using System.Windows.Controls;
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
            get { return _PathToDb; }
            set
            {
                if (Set(ref _PathToDb, value))
                {
                    _DataService.Setting.PathToDb = _PathToDb;
                }
            }
        }


        private string _PathToXML;
        public string PathToXML
        {
            get => _PathToXML; set
            {
                if (Set(ref _PathToXML, value))
                {
                    _DataService.Setting.PathToXML = _PathToXML;
                }
            }
        }


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


        #region AddXmlInAllFoldersCommand - описание команды 
        private LambdaCommand _AddXmlInAllFoldersCommand;
        public ICommand AddXmlInAllFoldersCommand => _AddXmlInAllFoldersCommand ??=
            new LambdaCommand(OnAddXmlInAllFoldersCommandExecuted, CanAddXmlInAllFoldersCommandExecute);
        private bool CanAddXmlInAllFoldersCommandExecute(object p) => true;
        private void OnAddXmlInAllFoldersCommandExecuted(object p)
        {
            string pathAccount = "C:\\Program Files (x86)\\BloodyWorkShop8\\BloodyWorkShop8\\Accounts\\Guest\\Data";
            if (Path.Exists(pathAccount))
            {
                var directories = Directory.GetDirectories(pathAccount);
                foreach (var item in directories)
                {
                    var locate = item.Substring(pathAccount.Length).Replace("\\",string.Empty);
                    var sLed = $"{item}\\SLED";
                    if (Path.Exists(sLed))
                    {
                        var sLedDirectories = Directory.GetDirectories(sLed);
                        foreach (var pa in sLedDirectories)
                        {
                            var dir = pa.Substring(sLed.Length).Replace("\\", string.Empty);

                            Lighting lighting = new Lighting() { Name = $"{locate} {dir}", Guid = Guid.Parse("00000000-0000-0000-0000-000000000000") };
                            _DataService.SaveToXML(lighting, new System.Collections.Generic.List<FrameCycle>(),pa);
                        }
                    }
                }

            }
        }
        #endregion

    }
}

