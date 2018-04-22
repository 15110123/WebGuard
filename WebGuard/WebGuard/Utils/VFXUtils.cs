using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebGuard.Utils
{
    // ReSharper disable once InconsistentNaming
    public static class VFXUtils
    {
        public static async Task ChangeText(this Label label, string content, Control container)
        {
            //Store pre-color
            var preColor = label.ForeColor;

            //Hide label
            await label.ChangeTextColor(container.BackColor, 1);
            
            //Change text
            label.Text = content;
            label.CenterXIn(container);

            //Show label
            await label.ChangeTextColor(preColor, 2);
        }

        public static async Task ChangeTextColor(this Label label, Color finalColor, int num)
        {
            while (label.ForeColor.R != finalColor.R
                   || label.ForeColor.G != finalColor.G
                   || label.ForeColor.B != finalColor.B)
            {
                if (label.ForeColor.R < finalColor.R)
                {
                    label.ForeColor = Color.FromArgb(label.ForeColor.R + 1, label.ForeColor.G, label.ForeColor.B);
                }

                if (label.ForeColor.R > finalColor.R)
                {
                    label.ForeColor = Color.FromArgb(label.ForeColor.R - 1, label.ForeColor.G, label.ForeColor.B);
                }

                if (label.ForeColor.G < finalColor.G)
                {
                    label.ForeColor = Color.FromArgb(label.ForeColor.R, label.ForeColor.G + 1, label.ForeColor.B);
                }

                if (label.ForeColor.G > finalColor.G)
                {
                    label.ForeColor = Color.FromArgb(label.ForeColor.R, label.ForeColor.G - 1, label.ForeColor.B);
                }

                if (label.ForeColor.B < finalColor.B)
                {
                    label.ForeColor = Color.FromArgb(label.ForeColor.R, label.ForeColor.G, label.ForeColor.B + 1);
                }

                if (label.ForeColor.B > finalColor.B)
                {
                    label.ForeColor = Color.FromArgb(label.ForeColor.R, label.ForeColor.G, label.ForeColor.B - 1);
                }

                await Task.Delay(1);
            }
        }

        public static void CenterXIn(this Control control, Control to)
        {
            control.Location = new Point((to.Width - control.Width)/2, control.Location.Y);
        }
    }
}
