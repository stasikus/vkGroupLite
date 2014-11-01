using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vkGroupWall
{
    class ChangeStatus
    {
        public static void activStatus(Control[] controlObj)
        {
            for (int i = 0; i < controlObj.Count(); i++)
            {
                int j = i;
                controlObj[j].BeginInvoke((Action)delegate
                {
                    if (controlObj[j] is TextBox || controlObj[j] is CheckBox || controlObj[j] is Panel)
                        controlObj[j].Enabled = true;
                    else
                    {
                        controlObj[j].BackColor = Color.DarkGray;
                        controlObj[j].Enabled = true;
                    }
                });
            }
        }

        public static void notActivStatus(Control[] controlObj)
        {
            for (int i = 0; i < controlObj.Count(); i++)
            {
                int j = i;
                controlObj[j].BeginInvoke((Action)delegate
                {
                    if (controlObj[j] is TextBox || controlObj[j] is CheckBox || controlObj[j] is Panel)
                        controlObj[j].Enabled = false;
                    else
                    {
                        controlObj[j].BackColor = Color.WhiteSmoke;
                        controlObj[j].Enabled = false;
                    }
                });
            }
        }
    }
}
