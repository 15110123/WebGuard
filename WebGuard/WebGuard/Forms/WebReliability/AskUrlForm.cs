using System;
using System.Windows.Forms;

namespace WebGuard.Forms.WebReliability
{
    public partial class AskUrlForm : Form
    {
        public string Url { get; private set; }

        public AskUrlForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Url = tbUrl.Text;
            Close();
        }
    }
}
