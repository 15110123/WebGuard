using System;
using System.Windows.Forms;
using CefSharp;
using WebGuard.CustomBrowser;

namespace WebGuard.Utils
{
    public static class ChromiumUtils
    {
        public static void AddIcon(this Panel container, string name, string backgroundColor)
        {
            var brw = new ChromiumWithScript(Environment.CurrentDirectory + $"/motion/{name}.html", $"document.body.style.backgroundColor = \"{backgroundColor}\"")
            {
                Dock = DockStyle.Fill
            };
            container.Controls.Add(brw);
        }
    }
}
