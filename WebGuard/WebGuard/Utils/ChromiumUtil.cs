using System;
using System.Windows.Forms;
using WebGuard.CustomBrowser;

namespace WebGuard.Utils
{
    public static class ChromiumUtil
    {
        public static void AddIcon(this Panel container, string name, string backgroundColor)
        {
            var brw = new ChromiumWithScript(Environment.CurrentDirectory + $"/motion/{name}", $"document.body.style.backgroundColor = \"{backgroundColor}\"")
            {
                Dock = DockStyle.Fill
            };
            container.Controls.Add(brw);
        }
    }
}
