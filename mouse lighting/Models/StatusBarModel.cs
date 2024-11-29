using mouse_lighting.Services.UserDialog;

namespace mouse_lighting.Models
{
    internal class StatusBarModel
    {
        public string Message { get; private set; }
        public string Color { get; private set; }
        event Action MessageSent;
        public StatusBarModel(Action action)
        {
            Message = "Ready";
            Color = "#FFFFFF";
            MessageSent += action;
        }
        public void UpdateStatusBar(string message, TypeNotification notification)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;
                switch (notification)
                {
                    case TypeNotification.Warning:
                        Color = "#E4E532";
                        break;
                    case TypeNotification.Error:
                        Color = "#E50606";
                        break;
                    case TypeNotification.Notification:
                    default:
                        Color = "#ffffff";
                        break;
                }
                MessageSent?.Invoke();
            }
        }
    }
}
