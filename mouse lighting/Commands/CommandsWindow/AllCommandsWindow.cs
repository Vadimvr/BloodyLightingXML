using mouse_lighting.Commands.Base;
using mouse_lighting.Commands.CommandsWindow.WindowMessageUser32;
using System.Windows;

namespace mouse_lighting.Commands.CommandsWindow
{
    internal class CloseWindow : Command
    {
        private static Window? GetWindow(object? p) => p as Window ?? App.FocusedWindow ?? App.ActiveWindow;

        protected override bool CanExecute(object? p) => p is Window;

        protected override void Execute(object? p) => GetWindow(p)?.Close();
    }
    internal class StateNormalMaximizedWindow : Command
    {
        protected override bool CanExecute(object? p) => p is Window;

        protected override void Execute(object? p)
        {
            var window = p as Window;
            if (window != null)
            {
                switch (window.WindowState)
                {
                    case WindowState.Normal:
                        SystemCommands.MaximizeWindow(window);
                        break;
                    case WindowState.Maximized:
                        SystemCommands.RestoreWindow(window);
                        break;
                }
            }
        }
    }
    internal class StateMinimizedWindow : Command
    {
        protected override bool CanExecute(object? p) => p is Window;

        protected override void Execute(object? p)
        {
            var window = p as Window;
            if (window != null)
            {
                SystemCommands.MinimizeWindow(window);
            }
        }
    }
    internal class MoveWindowCommand : Command
    {
        protected override bool CanExecute(object? p) => p is Window;

        protected override void Execute(object? p)
        {
            var window = p as Window;
            if (window != null)
            {
                window.DragMove();
            }
        }
    }

    // 1 click on the application icon
    internal class SnowSystemMiniMenu : Command
    {
        protected override bool CanExecute(object? p) => p is Window;

        protected override void Execute(object? p)
        {
            if (p is Window window)
            {
                window.SendMessage(WM.SYSCOMMAND, SC.KEYMENU);
            }

        }
    }
}
