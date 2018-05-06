using CefSharp;
using System;
using System.Windows.Forms;
using WebGuard.Forms;

namespace WebGuard
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var settings = new CefSettings
            {
                BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe",
                UncaughtExceptionStackSize = 0,
                LogSeverity = LogSeverity.Disable
            };
            settings.CefCommandLineArgs.Add("--disable-speech-api", "1");
            settings.CefCommandLineArgs.Add("--disable-low-res-tiling", "1");
            settings.CefCommandLineArgs.Add("--disable-threaded-scrolling", "1");
            settings.CefCommandLineArgs.Add("--disable-infobars", "1");
            settings.CefCommandLineArgs.Add("--disable-offline-auto-reload", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            settings.CefCommandLineArgs.Add("--disable-smooth-scrolling", "1");
            settings.CefCommandLineArgs.Add("--disable-spell-checking", "1");
            settings.CefCommandLineArgs.Add("--disable-sync", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("--allow-file-access-from-files", "1");
            settings.DisableGpuAcceleration();
            settings.CefCommandLineArgs.Add("--disable-xss-auditor", "1");
            // Initialize cef with the provided settings
            Cef.Initialize(settings);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
