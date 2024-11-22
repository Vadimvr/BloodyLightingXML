using mouse_lighting.Services.UserDialog;
using System.Drawing;

namespace mouse_lighting.Services.Observer
{
    internal interface IObserverStatusBar
    {
        void StatusBar(string message, TypeNotification typeNotification = TypeNotification.Notification);
    }

    internal class ObserverStatusBar : IObserverStatusBar
    {
        private Action<string, TypeNotification>? updateStatusBar;

        internal void UpdateStatusBarEvent(Action<string, TypeNotification> action) => updateStatusBar += action;

        public void StatusBar(string message, TypeNotification typeNotification = default)
        {
            updateStatusBar?.Invoke(message, typeNotification);

        }
    }
}