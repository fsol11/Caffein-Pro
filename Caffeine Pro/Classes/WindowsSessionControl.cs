﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caffeine_Pro.Classes
{
    public static class WindowsSessionControl
    {
        public enum SessionAction
        {
            [Description("Do Nothing")]
            None,
            [Description("Exit Program")]
            Exit,
            [Description("Lock")]
            Lock,
            [Description("Sign Out")]
            SignOut,
            [Description("Shutdown")]
            Shutdown,
            [Description("Force Shutdown")]
            ForceShutdown,
            [Description("Restart")]
            Restart,
            [Description("Force Restart")]
            ForceRestart,
            [Description("Hibernate")]
            Hibernate,
            [Description("Sleep")]
            Sleep,
            [Description("Monitor Off")]
            MonitorOff
        }

        // P/Invoke declaration for SendMessage
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("PowrProf.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once IdentifierTypo
        private const int SC_MONITORPOWER = 0xF170;
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once IdentifierTypo
        private const int WM_SYSCOMMAND = 0x0112;
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once IdentifierTypo
        private const int MONITOR_OFF = 2;
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once IdentifierTypo
        private const int HWND_BROADCAST = 0xFFFF;

        public static void ExecuteSessionAction(SessionAction action)
        {
            switch (action)
            {
                case SessionAction.Lock:
                    System.Diagnostics.Process.Start("rundll32.exe", "user32.dll,LockWorkStation");
                    break;
                case SessionAction.SignOut:
                    System.Diagnostics.Process.Start("shutdown", "/l");
                    break;
                case SessionAction.Shutdown:
                    System.Diagnostics.Process.Start("shutdown", "/s");
                    break;
                case SessionAction.ForceShutdown:
                    System.Diagnostics.Process.Start("shutdown", "/s /f");
                    break;
                case SessionAction.Restart:
                    System.Diagnostics.Process.Start("shutdown", "/r");
                    break;
                case SessionAction.ForceRestart:
                    System.Diagnostics.Process.Start("shutdown", "/r /f");
                    break;
                case SessionAction.Hibernate:
                    System.Diagnostics.Process.Start("shutdown", "/h /f");
                    break;
                case SessionAction.Sleep:
                    SetSuspendState(false, true, false);
                    break;
                case SessionAction.MonitorOff:
                    SendMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
                    break;
                case SessionAction.None:
                    break;
                default:
                    break;
            }
        }
    }
}
