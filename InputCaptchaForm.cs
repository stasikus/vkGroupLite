using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vkGroupWall
{
    public partial class InputCaptchaForm : Form
    {
        public string capchaNum = "";
        public InputCaptchaForm(ref string sid)
        {
            InitializeComponent();
            this.sidPic(sid);
        }
        

        public void captcha_input_btn_Click(object sender, EventArgs e)
        {
            capchaNum = captcha_input_TB.Text;
            this.Close();
        }

        public void sidPic(string sid)
        {
            var request = WebRequest.Create("http://vk.com/captcha.php?sid=" + sid + "&s=1");

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                captcha_input_pic.Image = Bitmap.FromStream(stream);
            }
        }
    }
}
