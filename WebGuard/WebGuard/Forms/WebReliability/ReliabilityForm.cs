using System.Windows.Forms;
using WebGuard.UserControls.WebReliability;

namespace WebGuard.Forms.WebReliability
{
    public partial class ReliabilityForm : Form
    {
        public ReliabilityForm()
        {
            InitializeComponent();
        }

        private void ReliabilityForm_Load(object sender, System.EventArgs e)
        {
            using (var dialog = new AskUrlForm())
            {
                dialog.ShowDialog();
                if (dialog.Url == null)
                {
                    Close();
                    return;
                }
                pnlProgressDetails.Controls.Add(new ProgressDetails(dialog.Url)
                {
                    Dock = DockStyle.Fill
                });
            }
        }
    }
}
