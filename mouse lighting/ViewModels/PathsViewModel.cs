using mouse_lighting.Commands.Base;
using mouse_lighting.Models;
using mouse_lighting.ViewModels.Base;
using System.Windows.Input;

namespace mouse_lighting.ViewModels
{
    internal class PathsViewModel : ViewModelBase
    {
        private readonly PathsModel _PathsModel;

        private string _PathToDb = default!;
        public string PathToDb { get => _PathToDb; set => Set(ref _PathToDb, value); }

        private string _PathToXml = default!;
        public string PathToXml { get => _PathToXml; set => Set(ref _PathToXml, value); }

        public PathsViewModel(PathsModel pathsModel)
        {
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

        #region FindFolderCommand - описание команды 
        private LambdaCommand? _FindFolderCommand;
        public ICommand FindFolderCommand => _FindFolderCommand ??=
            new LambdaCommand(OnFindFolderCommandExecuted, CanFindFolderCommandExecute);
        private bool CanFindFolderCommandExecute(object? p) => true;
        private void OnFindFolderCommandExecuted(object? p) { _PathsModel.FindFolder(); }
        #endregion

    }
}
