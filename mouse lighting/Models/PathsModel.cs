using mouse_lighting.Services.UserDialog;
using System.IO;

namespace mouse_lighting.Models
{
    internal class PathsModel
    {
        private readonly FindFolder _FindFolder;
        private readonly IUserDialog _UserDialog;
        private readonly Setting _Setting;

        public event Action? UpdatedPathXMLEvent;
        public event Action? UpdatedDbEvent;
        public PathsModel(IUserDialog UserDialog, Setting setting, FindFolder FindFolder)
        {
            _FindFolder = FindFolder;
            _UserDialog = UserDialog;
            _Setting = setting;
        }

        internal void OpenPathDb()
        {
            var x = _UserDialog.Open();
            if (!string.IsNullOrEmpty(x) && File.Exists(x))
            {
                _Setting.PathToDb = x;
                UpdatedDbEvent?.Invoke();
            }
        }
        internal void OpenPathToXml()
        {
            var x = _UserDialog.OpenFolder();
            if (!string.IsNullOrEmpty(x) && Directory.Exists(x))
            {
                _Setting.PathToXML = x;
                UpdatedPathXMLEvent?.Invoke();
            }
        }
        internal void FindFolder()
        {
            _FindFolder.Find();
        }

        public string PathToXML { get => _Setting.PathToXML; }
        public string PathToDb { get => _Setting.PathToDb; }
    }
}
