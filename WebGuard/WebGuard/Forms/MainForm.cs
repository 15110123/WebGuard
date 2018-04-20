using System;
using System.Windows.Forms;
using WebGuard.Utils;

namespace WebGuard.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            pnlIcon.AddIcon("wifi_connected.html", "#454545");
        }
    }
}
