using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace AndreasGustafsson.KnockoutGenerator.Utilities
{
    public static class ToastHelper
    {
        public static int Toast(this IVsUIShell shell, string message, string title, OLEMSGBUTTON buttons)
        {
            var clsid = Guid.Empty;
            int result;

            shell.ShowMessageBox(0, ref clsid,
            title,
            message,
            string.Empty, 0,
            buttons,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
            OLEMSGICON.OLEMSGICON_INFO,
            0, out result);

            return result;
        }

        public static void Toast(this IVsUIShell shell, string message)
        {
            var clsid = Guid.Empty;
            int result;

            shell.ShowMessageBox(0, ref clsid,
            "Knockout Generator Says",
            message,
            string.Empty, 0,
            OLEMSGBUTTON.OLEMSGBUTTON_OK, 
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
            OLEMSGICON.OLEMSGICON_INFO,
            0, out result);
        }
    }
}
