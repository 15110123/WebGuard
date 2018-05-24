using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebGuard.Helpers;
using WebGuard.Utils;
using static System.Environment;
using static System.Console;
using static System.Diagnostics.Process;
using static System.Threading.Tasks.Task;
using static WebGuard.Utils.ImageUtils;
using static WebGuard.Utils.StringDistanceUtils;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace WebGuard.UserControls.WebReliability
{
    //Step 1: Get all image from src in <img/> 
    //Step 2: Get all src in <iframe/> 
    //Step 3: Get content (innerText) from html 

    // TH1: onload, onafter, <script></script>
    // TH2: Lồng ghép 
    // TH3: <script src="">

    public sealed partial class ProgressDetails : UserControl
    {
        public event EventHandler DoneCapturingPc;
        public event EventHandler DoneCapturingServer;

        public string Url { get; }

        public int Step { get; } = 0;
        public string FilePathFromPc { get; set; }
        public string FilePathFromServer { get; set; }
        public Bitmap ScreenShotFromPc { get; set; }
        public Bitmap ScreenShotFromServer { get; set; }
        public string HtmlFromServer { get; set; }
        public string HtmlFromPc { get; set; }

        public ProgressDetails(string url)
        {
            Url = url;

            InitializeComponent();

            pnlLoading1.AddIcon("cup_game_loader_2", "#454545");
            pnlLoading2.AddIcon("cup_game_loader_2", "#454545");
        }

        private void ProgressDetails_Load(object o, EventArgs e)
        {
            Invoke((Action)(async () =>
            {
                pnlLoading1.Visible = true;
                pnlLoading2.Visible = true;

                try
                {
                    var analyzeServer = default(string);
                    GetAnalyzeServerResult();

                    HtmlFromPc = await GetHtmlFromPc();
                    var analyzePc = await AnalyzePcImg();
                    pnlLoading2.Visible = false;
                    while (analyzeServer == default(string))
                    {
                        await Delay(1);
                    }
                    pnlLoading1.Visible = false;

                    #region Debug
                    //WriteLine(analyzeServer);
                    //WriteLine(analyzePc);
                    //WriteLine(HtmlFromPc);
                    //WriteLine(HtmlFromServer);
                    #endregion

                    double percentImg = -1, percentHtml = -1;

                    LevenshteinHtml();
                    LevenshteinImg();

                    while (percentImg == -1 || percentHtml == -1) await Delay(1);

                    var percents = (percentImg + percentHtml) / 2;

                    lblPercents.Text = Math.Round(percents, 2) + "%";

                    MessageBox.Show("Đã xong!");

                    string result;

                    if (percents == 0)
                        result = "Xin chúc mừng! Trang web không bị thay đổi";
                    else if (percents <= 30)
                        result = "Trang web có một số thay đổi nhưng ít nguy hiểm";
                    else if (percents <= 70)
                        result = "Trang web có nhiều thay đổi, cần cảnh giác";
                    else
                        result = "Trang web có quá nhiều thay đổi, nên tránh xa";

                    lblResult.Text = result;

                    async void GetAnalyzeServerResult()
                    {
                        HtmlFromServer = await GetHtmlFromServer();
                        analyzeServer = await AnalyzeServerImg();
                    }

                    async void LevenshteinImg()
                    {
                        percentImg = await WordDistanceEqualPercents(analyzeServer, analyzePc, false);
                    }

                    async void LevenshteinHtml()
                    {
                        //WriteLine(HtmlFromServer);
                        //WriteLine("---------------------");
                        //WriteLine(HtmlFromPc);
                        percentHtml = await WordDistanceEqualPercents(HtmlFromServer, HtmlFromPc, true);
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                    WriteLine(ex.StackTrace);
                }
            }));
        }

        private async Task<string> AnalyzeServerImg()
        {
            var bitmap = await DownloadBitmap("http://172.27.73.28:5000/api/screenshot", Method.Post, ("url", Url), ("html", "0"));

            {
                var guid = Guid.NewGuid().ToString();
                FilePathFromServer = $@"{CurrentDirectory}\screenshot\{guid}";
            }

            bitmap.Save(FilePathFromServer);
            var fileNameFromServer = FilePathFromServer.Substring(FilePathFromServer.LastIndexOf('\\'));
            var filebytes = File.ReadAllBytes(FilePathFromServer);

            using (var ms = new MemoryStream(filebytes))
            {
                ScreenShotFromServer = new Bitmap(ms);
                ScreenShotFromServer.SaveJpeg(
                    $@"{CurrentDirectory}\img\{fileNameFromServer}", 50);
                DoneCapturingServer?.Invoke(ScreenShotFromServer, EventArgs.Empty);
            }

            var visionHelper = new VisionHelper("visualFeatures=Categories,Tags,Description,Faces,ImageType,Color&language=en");
            return await visionHelper.MakeAnalysisRequest<string>(FilePathFromServer);
        }

        private async Task<string> AnalyzePcImg()
        {
            var ps = new Process
            {
                StartInfo = new ProcessStartInfo(
                    $@"{CurrentDirectory}\WebGuard.Supplier.exe")
                {
                    Arguments = $"0 {Url}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            ps.Start();
            var filePath = ps.StandardOutput.ReadLine();

            while (GetProcessesByName("WebGuard.Supplier").Length != 0)
            {
                await Delay(2000);
            }

            if (filePath == null) return null;
            FilePathFromPc = filePath;
            var fileNameFromPc = filePath.Substring(filePath.LastIndexOf('\\'));
            var filebytes = File.ReadAllBytes(filePath);

            using (var ms = new MemoryStream(filebytes))
            {
                ScreenShotFromPc = new Bitmap(ms);
                ScreenShotFromPc.SaveJpeg(
                    $@"{CurrentDirectory}\img\{fileNameFromPc}", 50);
                DoneCapturingPc?.Invoke(ScreenShotFromPc, EventArgs.Empty);
            }

            var visionHelper = new VisionHelper("visualFeatures=Categories,Tags,Description,Faces,ImageType,Color&language=en");
            return await visionHelper.MakeAnalysisRequest<string>(FilePathFromPc);
        }

        private async Task<string> GetHtmlFromServer()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("url", Url);
                httpClient.DefaultRequestHeaders.Add("html", "1");

                return await (await httpClient.PostAsync("http://172.27.73.28:5000/api/screenshot", null))
                    .Content
                    .ReadAsStringAsync();
            }
        }

        private async Task<string> GetHtmlFromPc()
        {
            var ps = new Process
            {
                StartInfo = new ProcessStartInfo(
                    $@"{CurrentDirectory}\WebGuard.Supplier.exe")
                {
                    Arguments = $"1 {Url}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            ps.Start();
            var html = await ps.StandardOutput.ReadToEndAsync();

            while (GetProcessesByName("WebGuard.Supplier").Length != 0)
            {
                await Delay(2000);
            }

            return html;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (ScreenShotFromServer == null) return;
            ShowImgInForm(ScreenShotFromServer);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (ScreenShotFromPc == null) return;
            ShowImgInForm(ScreenShotFromPc);
        }

        private static void ShowImgInForm(Image img)
        {
            var frm = new Form
            {
                WindowState = FormWindowState.Maximized,
                AutoScroll = true
            };
            frm.Controls.Add(new PictureBox
            {
                Image = img,
                Width = img.Width,
                Height = img.Height
            });
            frm.Show();
        }
    }
}
