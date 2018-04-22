using CefSharp;

namespace WebGuard.CustomBrowser.BrowserHandler
{
    public class CustomContextMenuHandler : IContextMenuHandler
    {
        private static CustomContextMenuHandler _handler;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static CustomContextMenuHandler Handler => _handler ?? (_handler = new CustomContextMenuHandler());

        private CustomContextMenuHandler()
        {

        }

        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model)
        {
            model.Clear();
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model, IRunContextMenuCallback callback)
        {
            return true;
        }
    }
}
