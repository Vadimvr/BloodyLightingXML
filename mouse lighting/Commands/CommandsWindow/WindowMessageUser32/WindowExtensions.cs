using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace mouse_lighting.Commands.CommandsWindow.WindowMessageUser32
{
    internal static class WindowExtensions
    {
        private const string user32 = "user32.dll";

        [DllImport(user32, CharSet = CharSet.Auto)]
        private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

        public static nint SendMessage(this Window window, WM Msg, SC wParam, nint lParam = default)
        {
            return SendMessage(
                hWnd: window.GetWindowHandle(),
                Msg: (uint)Msg,
                wParam: (nint)wParam,
                lParam: lParam == default ? ' ' : lParam);
        }

        public static nint GetWindowHandle(this Window window)
        {
            var helper = new WindowInteropHelper(window);
            return helper.Handle;
        }
    }
}
