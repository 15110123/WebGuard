using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebGuard.Supplier.ChromiumHandler;
using static System.Environment;

namespace WebGuard.Supplier
{
    public class ChromiumObject
    {
        public readonly IList<Bitmap> _screenshots = new List<Bitmap>();
        public readonly Form Container;
        public readonly string Url;
        public ChromiumWebBrowser Browser;
        public double TotalHeight;
        public double ViewHeight;
        private bool _isStartCapturing;

        public ChromiumObject(string url, Form container)
        {
            Url = url;
            Container = container;
            Container.Load += Container_Load;
        }

        private ChromiumWebBrowser InitializeChromium(string url)
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
            settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            settings.CefCommandLineArgs.Add("--disable-smooth-scrolling", "1");
            settings.CefCommandLineArgs.Add("--disable-spell-checking", "1");
            settings.CefCommandLineArgs.Add("--disable-sync", "1");
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.DisableGpuAcceleration();
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            var chromeBrowser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
                Enabled = false,
                RequestHandler = new CustomRequestHandler()
            };
            Container.Controls.Add(chromeBrowser);
            return chromeBrowser;
        }

        private void Container_Load(object sender, EventArgs ev)
        {
            Browser = InitializeChromium(Url ?? "theguardian.com");
            Browser.LoadingStateChanged += (o, e) =>
            {
                if (e.IsLoading) return;

                Container.Invoke((Action)(async () =>
                {
                    if (_isStartCapturing) return;
                    _isStartCapturing = true;
                    //Set height property
                    ViewHeight = double.Parse((await Browser.EvaluateScriptAsync("window.innerHeight")).Result.ToString());
                    TotalHeight = double.Parse((await Browser.EvaluateScriptAsync("document.body.scrollHeight")).Result.ToString());

                    await this.CaptureScreenTillEnd();

                    var path = _screenshots.MergeIntoOneBitmap().SaveTo($@"{CurrentDirectory}\screenshot");
                    Console.Write(path);
                    Container.Close();
                }));
            };
        }

        public void KillZombieProcess()
        {
            foreach (var ele in Process.GetProcessesByName("CefSharp.BrowserSubprocess"))
            {
                try
                {
                    ele.Kill();
                }
                catch
                {
                    //ignore
                }
            }
        }
    }
}
