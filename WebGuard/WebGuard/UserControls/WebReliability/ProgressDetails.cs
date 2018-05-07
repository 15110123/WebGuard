using System.Windows.Forms;
using WebGuard.Utils;

namespace WebGuard.UserControls.WebReliability
{
    public partial class ProgressDetails : UserControl
    {
        public ProgressDetails()
        {
            InitializeComponent();

            pnlLoading1.AddIcon("cup_game_loader_2", "#454545");
            pnlLoading2.AddIcon("cup_game_loader_2", "#454545");
            pnlLoading3.AddIcon("cup_game_loader_2", "#454545");
        }
    }
}
