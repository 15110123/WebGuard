using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace WebGuard.Supplier
{
    public partial class MainForm : Form
    {
        private readonly List<Bitmap> _screenshots = new List<Bitmap>();

        public MainForm()
        {
            InitializeComponent();
            InitializeChromium("vnexpress.net").LoadingStateChanged += async (o, e) =>
            {
                if (e.IsLoading || !(o is ChromiumWebBrowser browser)) return;
                await browser.WhileIsNotEndScroll(
                    async () =>
                    {
                        await browser.TakeScreenshot(this)
                            .To(_screenshots, browser)
                            .ScrollNextViewHeight();
                    }
                    );
                Invoke((Action) (() =>
                {
                    var frm = new Form();
                    var ptb = new PictureBox
                    {
                        Image = _screenshots[0],
                        Dock = DockStyle.Fill
                    };
                    frm.Controls.Add(ptb);
                    frm.Show();
                    //Close();
                }));
            };
        }

        public ChromiumWebBrowser InitializeChromium(string url)
        {
            var settings = new CefSettings
            {
                BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe",
                UncaughtExceptionStackSize = 0,
                LogSeverity = LogSeverity.Disable
            };
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            var chromeBrowser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(chromeBrowser);
            return chromeBrowser;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
