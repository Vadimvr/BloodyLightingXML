using mouse_lighting.Commands.Base;
using System.Windows;

namespace mouse_lighting.Commands.Other
{
    internal class CloseWindowMenuItem : Command
    {
        private static Window? GetWindow(object? p) => p as Window ?? App.FocusedWindow ?? App.ActiveWindow;

        protected override bool CanExecute(object? p) => p is Window;

        protected override void Execute(object? p) => GetWindow(p)?.Close();
    }
}