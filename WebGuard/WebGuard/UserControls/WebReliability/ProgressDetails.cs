using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebGuard.Helpers;
using WebGuard.Utils;
using static System.Environment;
using static System.Console;

namespace WebGuard.UserControls.WebReliability
{
    public sealed partial class ProgressDetails : UserControl
    {
        public event EventHandler DoneCapturing;

        public string Url { get; }

        public int Step { get; } = 0;
        public string FilePathFromPc { get; set; }
        private Bitmap ScreenShotFromPc { get; set; }

        public ProgressDetails(string url)
        {
            Url = url;

            InitializeComponent();

            pnlLoading1.AddIcon("cup_game_loader_2", "#454545");
            pnlLoading2.AddIcon("cup_game_loader_2", "#454545");
            pnlLoading3.AddIcon("cup_game_loader_2", "#454545");
        }

        private async void ProgressDetails_Load(object o, EventArgs e)
        {
            pnlLoading1.Visible = true;
            pnlLoading2.Visible = true;

            try
            {
                //WriteLine($@"{CurrentDirectory}\WebGuard.Supplier.exe");
                var ps = new Process
                {
                    StartInfo = new ProcessStartInfo(
                        $@"{CurrentDirectory}\WebGuard.Supplier.exe")
                    {
                        Arguments = Url,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                };
                ps.Start();
                var filePath = ps.StandardOutput.ReadLine();

                while (Process.GetProcessesByName("WebGuard.Supplier").Length != 0)
                {
                    await Task.Delay(2000);
                }

                if (filePath == null) return;
                FilePathFromPc = filePath;
                var FileNameFromPc = filePath.Substring(filePath.LastIndexOf('\\'));
                var filebytes = File.ReadAllBytes(filePath);

                using (var ms = new MemoryStream(filebytes))
                {
                    ScreenShotFromPc = new Bitmap(ms);
                    ScreenShotFromPc.SaveJpeg(
                        $@"{CurrentDirectory}\img\{FileNameFromPc}", 50);
                    DoneCapturing?.Invoke(ScreenShotFromPc, EventArgs.Empty);
                }

                var visionHelper = new VisionHelper("visualFeatures=Categories,Tags,Description,Faces,ImageType,Color&language=en");
                var pcImgResult = await visionHelper.MakeAnalysisRequest<string>(FilePathFromPc);
                WriteLine(pcImgResult);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (ScreenShotFromPc == null) return;
            var frm = new Form
            {
                WindowState = FormWindowState.Maximized,
                AutoScroll = true
            };
            frm.Controls.Add(new PictureBox
            {
                Image = ScreenShotFromPc,
                Width = ScreenShotFromPc.Width,
                Height = ScreenShotFromPc.Height
            });
            frm.ShowDialog();
        }
    }
}
