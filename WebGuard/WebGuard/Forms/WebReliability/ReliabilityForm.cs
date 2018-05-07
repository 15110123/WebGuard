using System.Windows.Forms;
using WebGuard.UserControls.WebReliability;

namespace WebGuard.Forms.WebReliability
{
    public partial class ReliabilityForm : Form
    {
        public ReliabilityForm()
        {
            InitializeComponent();
            pnlProgressDetails.Controls.Add(new ProgressDetails
            {
                Dock = DockStyle.Fill
            });
        }
    }
}
