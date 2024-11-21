namespace mouse_lighting.Services.UserDialog
{
    internal enum TypeNotification
    {
        Notification,
        Warning,
        Error
    }

    internal enum TypeDialogBox
    {
        Open,
        OpenMany,
        Save,
    }
    internal interface IUserDialog
    {

        public void ShowNotification(
            string message,
            TypeNotification typeNotification = TypeNotification.Notification,
            string? title = null);

        public string? Save(string? path = null, string? filter = null, int filterIndex = 1);
        public string? Open(string? path = null, string? filter = null, int filterIndex = 1);
        public IEnumerable<string> OpenMany(string? path = null, string? filter = null, int filterIndex = 1);

    }
}
