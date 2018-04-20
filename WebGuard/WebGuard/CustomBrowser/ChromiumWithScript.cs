using CefSharp;

namespace WebGuard.CustomBrowser
{
    public sealed class ChromiumWithScript : CustomChromium
    {
        public string Script { get; }

        public ChromiumWithScript(string address, string script, IRequestContext requestContext = null) : base(address,
            requestContext)
        {
            Script = script;
            LoadingStateChanged += OnLoadingStateChanged;
        }

        private void OnLoadingStateChanged(object o, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading) return;
            this.ExecuteScriptAsync(Script);
        }
    }
}
