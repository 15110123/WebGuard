using System;
using CefSharp;

namespace WebGuard.CustomBrowser
{
    public sealed class ChromiumWithScript : CustomChromium
    {
        /// <summary>
        /// Initial script, can be empty if you don't want to run one
        /// </summary>
        public string Script { get; }
        /// <summary>
        /// Output string of Chromium console, override each time there is an execution of console.log()
        /// </summary>
        public string ConsoleOutput { get; private set; }

        public ChromiumWithScript(string address, string script, IRequestContext requestContext = null) : base(address,
            requestContext)
        {
            Script = script;
            LoadingStateChanged += OnLoadingStateChanged;
            ConsoleMessage += OnConsoleMessage;
        }

        private void OnConsoleMessage(object o, ConsoleMessageEventArgs e)
        {
            ConsoleOutput = e.Message;
        }

        private void OnLoadingStateChanged(object o, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading) return;
            this.ExecuteScriptAsync(Script);
        }
    }
}
