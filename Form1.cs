using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vkGroupWall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Net http = new Net(); //Создаем объект
        string html = "";
        string log = "Список групп и статус сообщения:";
        public Control[] controls;

        public void loginBtn_Click(object sender, EventArgs e)
        {
            Thread tr = new Thread(() => loginFunc(loginTextBox.Text, passTextBox.Text, ""));
            tr.IsBackground = true;
            tr.Start();

        }

        public void loginFunc(string login, string pass, string proxys)
        {
            controls = new Control[] {loginBtn};
            ChangeStatus.notActivStatus(controls);

            int status = vkLogin.Login(login, pass, html, http, proxys); //Login

            if (status == 1) // валидный акк
            {
                controls = new Control[] {loadFromFile_btn, logout_btn, postBtn, waitFrom_tb, waitTo_tb, messageTB, recurs_check};
                ChangeStatus.activStatus(controls);

                controls = new Control[] {loginBtn, loginTextBox, passTextBox};
                ChangeStatus.notActivStatus(controls);

                MessageBox.Show("Аккаунт валидный");
             }
             else
             {
                MessageBox.Show("Неверный пароль или аккаунт заблокирован");

                controls = new Control[] { loginBtn };
                ChangeStatus.activStatus(controls);

            }
        }
        
        void postWall()
        {
            bool inputCaptchaType = true;
            if (captcha_manual.Checked != true)
                inputCaptchaType = false;

            
            string sPath = Directory.GetCurrentDirectory();
            StreamWriter sw = new StreamWriter(@"" + sPath + "\\outLog.txt", true, System.Text.Encoding.UTF8);
            sw.WriteLine(log);

            if(recurs_check.Checked)
            {
                while (recurs_check.Checked)
                {
                    mainSenderMethod(sw, log, inputCaptchaType);
                }
            }
            else
            {
                mainSenderMethod(sw, log, inputCaptchaType);
            }
            
            
            sw.Close();
            log = "";

            MessageBox.Show("Все сообщения были отправлены");

            controls = new Control[] { panel1, recurs_check };
            ChangeStatus.activStatus(controls);

            controls = new Control[] { recurs_stop_btn };
            ChangeStatus.notActivStatus(controls);

        }

        public void mainSenderMethod(StreamWriter sw, string log, bool inputCaptchaType)
        {
            int randomNum;
            int messageCounter = 0;
            while (messageCounter < groupList.Items.Count)
            {
                Random random = new Random();
                if (waitFrom_tb.Text != "" && waitTo_tb.Text != "")
                {
                    randomNum = random.Next(Convert.ToInt32(waitFrom_tb.Text), Convert.ToInt32(waitTo_tb.Text));
                    Thread.Sleep(randomNum * 1000); //random thread sleep between posting the message
                }

                string html = http.GetHtml("https://vk.com/" + groupList.Items[messageCounter].ToString(), "", "");

                string groupID = html.Remove(0, html.IndexOf("\"group_id\":") + 11);
                groupID = groupID.Substring(0, groupID.IndexOf(","));
                string publicID = html.Remove(0, html.IndexOf("\"public_id\":") + 12);
                publicID = publicID.Substring(0, publicID.IndexOf(","));
                string hash = html.Remove(0, html.IndexOf("\"post_hash\":") + 13);
                hash = hash.Substring(0, hash.IndexOf("\""));

                string idForPost;
                if (groupID.Length < 30)
                    idForPost = groupID;
                else
                    idForPost = publicID;

                string post = "Message=" + System.Web.HttpUtility.UrlEncode(messageTB.Text) + "&act=post&al=1&facebook_export=&fixed=&friends_only=&from=&hash=" + hash + "&official=&signed=&status_export=&to_id=-" + idForPost + "&type=all";
                string htmlResp = http.PostMessage("https://vk.com/al_wall.php", groupList.Items[messageCounter].ToString(), post, messageTB.Text, hash, idForPost, inputCaptchaType, antigateKey_TB.Text);

                int errorHtml = htmlResp.IndexOf("<!>8<!>");

                if (errorHtml == -1) // сообщений отправленно
                {
                    totalMessage_lbl.BeginInvoke((Action)delegate
                    {
                        totalMessage_lbl.Text = TotalCounter.SuccessMessages().ToString();
                    });
                    log = groupList.Items[messageCounter] + " - отправленно";
                    sw.WriteLine(log);
                }
                else // сообщений не отправленно
                {
                    totalErrorMsg_lbl.BeginInvoke((Action)delegate
                    {
                        totalErrorMsg_lbl.Text = TotalCounter.FailMessages().ToString();
                    });
                    log = groupList.Items[messageCounter] + " - не отправленно";
                    sw.WriteLine(log);
                }

                messageCounter++;

                if (!recurs_check.Checked)
                    break;
            }
        }

        private void postBtn_Click(object sender, EventArgs e)
        {

            if (messageTB.TextLength > 0 && groupList.Items.Count > 0)
            {
                sendMessages();
            }
            else
            {
                MessageBox.Show("Список групп и\\или сообщение не может быть пустым");
            }
        }

        public void sendMessages()
        {
            controls = new Control[] { recurs_stop_btn };
            ChangeStatus.activStatus(controls);

            controls = new Control[] { panel1, recurs_check };
            ChangeStatus.notActivStatus(controls);
        
            Thread tr = new Thread(postWall);
            tr.IsBackground = true;
            tr.Start();
        }

        private void loadFromFile_btn_Click(object sender, EventArgs e)
        {
            string path;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                groupList.Items.Clear();
                path = openFileDialog.FileName;
                LoadList.loadGroupList(path);
            }
            
            for (int i = 0; i < LoadList.groups.Count; i++)
            {
                groupList.Items.Add(LoadList.groups[i]);
            }

            if (LoadList.groups.Count > 0)
            {
                controls = new Control[] { clean_btn };
                ChangeStatus.activStatus(controls);
            }
        }

        private void clean_btn_Click(object sender, EventArgs e)
        {
            groupList.Items.Clear();
            
            controls = new Control[] { clean_btn };
            ChangeStatus.notActivStatus(controls);
        }

        private void check_balance_btn_Click(object sender, EventArgs e)
        {
            balance_lbl.Text = "0";
            Thread tr = new Thread(checkBalance);
            tr.IsBackground = true;
            tr.SetApartmentState(ApartmentState.STA);
            tr.Start();
        }

        void checkBalance()
        {
            try
            {
                balance_lbl.BeginInvoke((Action)delegate
                {
                    balance_lbl.Text = Anticaptcha.Balance(antigateKey_TB.Text) + " $";
                });
            }
            catch (Exception)
            {
                MessageBox.Show("Данный ключ не валидный");
            }
                
        }

        private void captcha_manual_CheckedChanged(object sender, EventArgs e)
        {
            if (captcha_manual.Checked == true)
            {
                controls = new Control[] { antigateKey_TB, check_balance_btn };
                ChangeStatus.notActivStatus(controls);
            }
            else
            {
                controls = new Control[] { antigateKey_TB, check_balance_btn };
                ChangeStatus.activStatus(controls);
            }
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            controls = new Control[] { loginTextBox, passTextBox, loginBtn };
            ChangeStatus.activStatus(controls);

            controls = new Control[] { logout_btn, loadFromFile_btn, clean_btn, messageTB, postBtn, waitFrom_tb, waitTo_tb, recurs_check };
            ChangeStatus.notActivStatus(controls);
        }
        
        private void recurs_stop_btn_Click(object sender, EventArgs e)
        {
            controls = new Control[] { recurs_check, recurs_stop_btn };
            ChangeStatus.notActivStatus(controls);

            recurs_check.Checked = false;
            MessageBox.Show("Ожидаем отправку последнего сообщения");
        }

    }
}
