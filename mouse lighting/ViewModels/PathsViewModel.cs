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

        public PathsViewModel(IUserDialog UserDialog, IDataService DataService, PathsModel pathsModel)
        {
            _UserDialog = UserDialog;
            _DataService = DataService;
            _PathsModel = pathsModel;
            PathToXml = pathsModel.PathToXML;
            PathToDb = pathsModel.PathToDb;
            _PathsModel.UpdatedPathXMLEvent += UpdatedPathXML;
            _PathsModel.UpdatedDbEvent += UpdatedDb;
        }

        private void UpdatedPathXML() => PathToXml = _PathsModel.PathToXML;
        private void UpdatedDb() => PathToDb = _PathsModel.PathToDb;


        #region SelectPathToDbCommand - описание команды 
        private LambdaCommand? _SelectPathToDbCommand;
        public ICommand SelectPathToDbCommand => _SelectPathToDbCommand ??=
            new LambdaCommand((p) => _PathsModel.OpenPathDb());
        #endregion

        #region SelectPatToXmlCommand - описание команды 
        private LambdaCommand? _SelectPatToXmlCommand;
        public ICommand SelectPatToXmlCommand => _SelectPatToXmlCommand ??=
            new LambdaCommand((p) => _PathsModel.OpenPathToXml());
        #endregion

    }
}
