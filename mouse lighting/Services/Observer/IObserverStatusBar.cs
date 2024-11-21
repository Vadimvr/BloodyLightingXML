using System.Drawing;

namespace mouse_lighting.Services.Observer
{
    internal interface IObserverStatusBar
    {
        void StatusBar(string message, Color color = default!);
    }

    internal class ObserverStatusBar : IObserverStatusBar
    {
        private Action<string, Color>? updateStatusBar;

        internal void UpdateStatusBarEvent(Action<string, Color> action) => updateStatusBar += action;

        public void StatusBar(string message, Color color = default)
        {
            updateStatusBar?.Invoke(message, color);
        }
    }
}