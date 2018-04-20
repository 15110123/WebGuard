using CefSharp;
using System.Drawing;
using System.Threading.Tasks;

namespace WebGuard.Supplier
{
    public static class ChromiumExtension
    {
        public static async Task<ChromiumObject> ScrollNextViewHeight(this ChromiumObject obj)
        {
            await obj.Browser.EvaluateScriptAsync($"window.scrollTo(0, window.scrollY + {obj.ViewHeight});");
            return obj;
        }

        public static async Task CaptureScreenTillEnd(this ChromiumObject obj)
        {
            while (!await obj.IsEndScroll())
            {
                await obj.CaptureScreenAndScroll();
            }
        }

        public static async Task CaptureScreenAndScroll(this ChromiumObject obj)
        {
            await (await obj.TakeScreenshot())
                .To(obj._screenshots, obj)
                .ScrollNextViewHeight();
        }

        private static async Task<bool> IsEndScroll(this ChromiumObject obj)
        {
            //Scroll position Y-axis
            var curScrollPos = double.Parse((await obj.Browser.EvaluateScriptAsync("window.scrollY")).Result.ToString());

            //Not 100% correct, '1' is for rounding value
            return obj.ViewHeight + curScrollPos + 1 > obj.TotalHeight;
        }

        public static async Task<Bitmap> TakeScreenshot(this ChromiumObject obj)
        {
            await Task.Delay(2000);
            var printscreen = new Bitmap(obj.Browser.Width, obj.Browser.Height);
            var gp = Graphics.FromImage(printscreen);
            gp.CopyFromScreen(0, 0, 0, 0, printscreen.Size, CopyPixelOperation.SourceCopy);
            await Task.Delay(2000);
            return printscreen;
        }
    }
}
