using System;
using System.Windows.Forms;
using WebGuard.Forms.XSS;
using WebGuard.Utils;

namespace WebGuard.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            pnlIcon.AddIcon("wifi_connected", "#454545");
        }

        private void BtnXSS_Click(object sender, EventArgs e)
        {
            var xssForm = new XSSForm();
            xssForm.ShowDialog();
        }
    }
}
