using mouse_lighting.Services.UserDialog;
using System.IO;

namespace mouse_lighting.Models
{
    internal class PathsModel
    {
        private IUserDialog _UserDialog;
        private Setting _Setting;

        public event Action? UpdatedPathXMLEvent;
        public event Action? UpdatedDbEvent;
        public PathsModel(IUserDialog UserDialog, Setting setting)
        {
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

        public string PathToXML { get => _Setting.PathToXML;  }
        public string PathToDb { get => _Setting.PathToDb;  }
    }
}
