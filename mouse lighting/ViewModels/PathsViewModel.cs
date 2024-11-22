using mouse_lighting.Commands.Base;
using mouse_lighting.Models;
using mouse_lighting.Services.DataService;
using mouse_lighting.Services.UserDialog;
using mouse_lighting.ViewModels.Base;
using System.Windows.Input;

namespace mouse_lighting.ViewModels
{
    internal class PathsViewModel : ViewModelBase
    {
        private readonly IUserDialog _UserDialog;
        private readonly IDataService _DataService;
        private readonly PathsModel _PathsModel;

        private string _PathToDb = default!;
        public string PathToDb { get => _PathToDb; set => Set(ref _PathToDb, value); }


        private string _PathToXml = default!;
        public string PathToXml { get => _PathToXml; set => Set(ref _PathToXml, value); }

        public PathsViewModel(IUserDialog UserDialog, IDataService DataService)
        {
            _UserDialog = UserDialog;
            _DataService = DataService;
            _PathsModel = new PathsModel(UserDialog, UpdatePathDb, UpdatePathXml, DataService.Setting);

        }

        void UpdatePathDb() { if (_PathsModel != null && !string.IsNullOrEmpty(_PathsModel.PathToDb)) { PathToDb = _PathsModel.PathToDb; } }
        void UpdatePathXml() { if (_PathsModel != null && !string.IsNullOrEmpty(_PathsModel.PathToXML)) { PathToXml = _PathsModel.PathToXML; } }


        #region SelectPathToDbCommand - описание команды 
        private LambdaCommand? _SelectPathToDbCommand;
        public ICommand SelectPathToDbCommand => _SelectPathToDbCommand ??=
            new LambdaCommand(OnSelectPathToDbCommandExecuted, CanSelectPathToDbCommandExecute);
        private bool CanSelectPathToDbCommandExecute(object? p) => true;
        private void OnSelectPathToDbCommandExecuted(object? p) 
        {
            _PathsModel.OpenPathDb();
        }
        #endregion


        #region SelectPatToXmlCommand - описание команды 
        private LambdaCommand? _SelectPatToXmlCommand;
        public ICommand SelectPatToXmlCommand => _SelectPatToXmlCommand ??=
            new LambdaCommand(OnSelectPatToXmlCommandExecuted, CanSelectPatToXmlCommandExecute);
        private bool CanSelectPatToXmlCommandExecute(object? p) => true;
        private void OnSelectPatToXmlCommandExecuted(object? p) 
        {
            _PathsModel.OpenPathToXml();
        }
        #endregion

    }
}
