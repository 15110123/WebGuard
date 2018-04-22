using CefSharp;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;
using WebGuard.CustomBrowser;
using WebGuard.CustomBrowser.BrowserHandler;
using WebGuard.Utils;

namespace WebGuard.Forms.XSS
{
    // ReSharper disable once InconsistentNaming
    public partial class XSSForm : Form
    {
        private bool _isCrawling;

        public XSSForm()
        {
            InitializeComponent();
            pnlLoading.AddIcon("loading", "#454545");
        }

        private void XSSForm_Load(object sender, EventArgs e)
        {
            var brw = new ChromiumWithScript("http://testfire.net", "")
            {
                Dock = DockStyle.Fill
            };
            brw.LoadingStateChanged += BrwOnLoadingStateChanged;
            pnlBrowser.Controls.Add(brw);
        }

        private async void BrwOnLoadingStateChanged(object o, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading || 
                _isCrawling || 
                !(o is ChromiumWithScript brw)) return;
            _isCrawling = true;
            var result = await brw.GetAllPageUrlWithSameOrigin();
            pnlLoading.Visible = false;
            lblWait.Visible = false;
            foreach (var ele in result)
            {
                Console.WriteLine(ele);
            }
        }
    }
}
