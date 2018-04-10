using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebGuard.Supplier
{
    public static class ChromiumExtension
    {
        public static async Task<ChromiumWebBrowser> ScrollNextViewHeight(this ChromiumWebBrowser brw)
        {
            var height = (await brw.EvaluateScriptAsync("window.innerHeight")).Result.ToString();
            var res = await brw.EvaluateScriptAsync($"window.scrollTo(0, window.scrollY + {height});");
            return brw;
        }

        public static async Task<ChromiumWebBrowser> WhileIsNotEndScroll(this ChromiumWebBrowser brw, Action action)
        {
            while (!await brw.IsEndScroll())
            {
                action.Invoke();
                await Task.Delay(1000);
            }
            return brw;
        }

        private static async Task<bool> IsEndScroll(this IWebBrowser brw)
        {
            //Window height (viewable)
            var height = int.Parse((await brw.EvaluateScriptAsync("window.innerHeight")).Result.ToString());

            //Scroll height (viewable + non-viewable)
            var scrollHeight = int.Parse((await brw.EvaluateScriptAsync("document.body.scrollHeight")).Result.ToString());

            //Scroll position Y-axis
            var curScrollPos = int.Parse((await brw.EvaluateScriptAsync("window.scrollY")).Result.ToString());

            //Not 100% correct, '1' is for rounding value
            return height + curScrollPos + 1 > scrollHeight;
        }

        public static Bitmap TakeScreenshot(this ChromiumWebBrowser brw, Form brwForm)
        {
            var printscreen = new Bitmap(brw.Width, brw.Height);
            var gp = Graphics.FromImage(printscreen);
            brw.Invoke((Action) (() =>
            {
                gp.CopyFromScreen(brw.PointToScreen(new Point(0, 0)), new Point(0, 0), new Size(brw.Width, brw.Height));
            }));
            return printscreen;
        }
    }
}
