using CefSharp;
using CefSharp.WinForms;
using WebGuard.CustomBrowser.BrowserHandler;

namespace WebGuard.CustomBrowser
{
    public class CustomChromium : ChromiumWebBrowser
    {
        public CustomChromium(string address, IRequestContext requestContext = null) : base(address, requestContext)
        {
            MenuHandler = new CustomContextMenuHandler();
        }
    }
}
