using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static System.Environment;

namespace WebGuard.API.Controllers
{
    [Route("api/[controller]")]
    public class ScreenshotController : Controller
    {
        // POST
        [HttpPost]
        public async Task<IActionResult> GetScreenShot([FromHeader]string url)
        {
            var ps = new Process
            {
                StartInfo = new ProcessStartInfo(@"D:\home\site\wwwroot\webguard.supplier\WebGuard.Supplier.exe")
                {
                    //Ẩn cửa sổ console
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };

            ps.Start();

            var filename = ps.StandardOutput.ReadLine();

            while (Process.GetProcessesByName("WebGuard.Supplier").Length != 0)
            {
                await Task.Delay(1);
            }

            var filebytes = await System.IO.File.ReadAllBytesAsync(filename);

            return new FileContentResult(filebytes, "image/jpeg");
        }
    }
}
