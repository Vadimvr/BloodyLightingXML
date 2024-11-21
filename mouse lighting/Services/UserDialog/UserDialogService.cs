using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace mouse_lighting.Services.UserDialog
{
    internal class UserDialogService : IUserDialog
    {
        readonly string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        readonly string defaultFilter = "All files (*.*)|*.*";
        readonly string Title = App.AppName;
        readonly OpenFileDialog ofd = new OpenFileDialog();
        readonly SaveFileDialog sfd = new SaveFileDialog();


        private bool _Open(string? path = null, string? filter = null, int filterIndex = 1, bool multiselect = false)
        {
            ofd.InitialDirectory = string.IsNullOrEmpty(path) ? defaultPath : Directory.Exists(path) ? path : defaultPath;
            ofd.Filter = string.IsNullOrEmpty(filter) ? defaultFilter : "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = filterIndex >= 1 ? filterIndex : 1;
            ofd.RestoreDirectory = true;
            ofd.Multiselect = multiselect;
            return ofd.ShowDialog() == DialogResult.OK;
        }

        public string? Open(string? path = null, string? filter = null, int filterIndex = 1)
        {
            if (_Open(path, filter, filterIndex)) { return ofd.FileName; }
            else { return null; }
        }


        public IEnumerable<string> OpenMany(string? path = null, string? filter = null, int filterIndex = 1)
        {
            if (_Open(path, filter, filterIndex, multiselect: true)) { return ofd.FileNames; }
            else { return Array.Empty<string>(); }
        }

        public string? Save(string? path = null, string? filter = null, int filterIndex = 1)
        {
            sfd.InitialDirectory = string.IsNullOrEmpty(path) ? defaultPath : Directory.Exists(path) ? path : defaultPath;
            sfd.Filter = string.IsNullOrEmpty(filter) ? defaultFilter : "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FilterIndex = filterIndex >= 1 ? filterIndex : 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK) { return sfd.FileName; }
            else { return null; }
        }

        public void ShowNotification(string message, TypeNotification typeNotification, string? title = null)
        {
            title = title ?? Title;
            MessageBoxImage icon;
            switch (typeNotification)
            {
                case TypeNotification.Notification:
                    icon = MessageBoxImage.Information;
                    break;
                case TypeNotification.Warning:
                    icon = MessageBoxImage.Warning;
                    break;
                case TypeNotification.Error:
                    icon = MessageBoxImage.Error;
                    break;
                default:
                    return;
            }
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result;
            result = System.Windows.MessageBox.Show(message, title, button, icon, MessageBoxResult.Yes);
        }
    }
}
