using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Environment;

namespace WebGuard.API.Controllers
{
    [Route("api/[controller]")]
    public class ScreenshotController : Controller
    {
        // POST
        [HttpPost]
        public async Task<IActionResult> GetScreenShot([FromHeader]string url, [FromHeader]string html)
        {
            //return Content($@"{CurrentDirectory}\webguard.supplier\WebGuard.Supplier.exe");
            try
            {
                var ps = new Process
                {
                    StartInfo = new ProcessStartInfo(
                                $@"{CurrentDirectory}\webguard.supplier\WebGuard.Supplier.exe")
                    {
                        Arguments = $"{html} {url}",
                        RedirectStandardOutput = true
                    }
                };
                ps.Start();
                var filenameOrHtml = ps.StandardOutput.ReadLine();

                while (Process.GetProcessesByName("WebGuard.Supplier").Length != 0)
                {
                    await Task.Delay(2000);
                }

                if (html != "0") return Content(filenameOrHtml);

                var filebytes = await System.IO.File.ReadAllBytesAsync(filenameOrHtml);
                return new FileContentResult(filebytes, "image/jpeg");

            }
            catch (Exception e)
            {
                return Content(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
