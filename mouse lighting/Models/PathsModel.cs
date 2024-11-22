using mouse_lighting.Services.DataService;
using mouse_lighting.Services.UserDialog;
using System.IO;

namespace mouse_lighting.Models
{
    class PathsModel
    {
        private IUserDialog _UserDialog;

        public event Action _UpdatedPathXMLEvent;
        public event Action _UpdatedDbEvent;
        public PathsModel(IUserDialog UserDialog, Action updatedDbPath, Action updatedPathXml)
        {
            _UserDialog = UserDialog;
            _UpdatedDbEvent += updatedDbPath;
            _UpdatedPathXMLEvent += updatedPathXml;
        }

        public PathsModel(IUserDialog UserDialog, Action updatedDbPath, Action updatedPathXml, Setting setting)
            : this(UserDialog, updatedDbPath, updatedPathXml)
        {
            if (setting != null)
            {
                if (string.IsNullOrEmpty(setting.PathToDb) && File.Exists(setting.PathToDb)) { PathToDb = setting.PathToDb;  }
                if (string.IsNullOrEmpty(setting.PathToXML) && Directory.Exists(setting.PathToXML)) { PathToXML = setting.PathToXML; }
            }
        }


        internal void OpenPathDb()
        {
            var x = _UserDialog.Open();
            if (!string.IsNullOrEmpty(x) && File.Exists(x))
            {
                PathToDb = x;
                _UpdatedDbEvent?.Invoke();
            }
        }
        internal void OpenPathToXml()
        {
            var x = _UserDialog.OpenFolder();
            if (!string.IsNullOrEmpty(x) && Directory.Exists(x))
            {
                PathToXML = x;
                _UpdatedPathXMLEvent?.Invoke();
            }
        }

        public string PathToXML { get; set; } = default!;
        public string PathToDb { get; set; } = default!;
    }
}
