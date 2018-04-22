using System;
using CefSharp;
using CefSharp.WinForms;
using WebGuard.CustomBrowser.BrowserHandler;

namespace WebGuard.CustomBrowser
{
    public class CustomChromium : ChromiumWebBrowser
    {
        public string Origin { get;  }

        public CustomChromium(string address, IRequestContext requestContext = null) : base(address, requestContext)
        {
            Origin = new Uri(address).Host;
            Enabled = false;
            MenuHandler = CustomContextMenuHandler.Handler;
        }
    }
}
