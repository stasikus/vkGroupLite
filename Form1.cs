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

        public void loginBtn_Click(object sender, EventArgs e)
        {
            Thread tr = new Thread(() => loginFunc(loginTextBox.Text, passTextBox.Text, ""));
            tr.IsBackground = true;
            tr.Start();

        }

        public void loginFunc(string login, string pass, string proxys)
        {
            loginBtn.BeginInvoke((Action)delegate
            {
                loginBtn.BackColor = Color.WhiteSmoke;
                loginBtn.Enabled = false;
            });

            int status = vkLogin.Login(login, pass, html, http, proxys); //Login

            if (status == 1) // валидный акк
            {
                panel1.BeginInvoke((Action)delegate
                {
                     loginBtn.Enabled = false;
                     loginBtn.BackColor = Color.WhiteSmoke;
                     loadFromFile_btn.BackColor = Color.DarkGray;
                     loadFromFile_btn.Enabled = true;
                     logout_btn.Enabled = true;
                     logout_btn.BackColor = Color.DarkGray;
                     loginTextBox.Enabled = false;
                     passTextBox.Enabled = false;
                     loadFromFile_btn.Enabled = true;
                     loadFromFile_btn.BackColor = Color.DarkGray;
                     waitFrom_tb.Enabled = true;
                     waitTo_tb.Enabled = true;
                     messageTB.Enabled = true;
                     postBtn.Enabled = true;
                     postBtn.BackColor = Color.DarkGray;
                     recurs_check.Enabled = true;
                 });
                MessageBox.Show("Аккаунт валидный");
             }
             else
             {
                MessageBox.Show("Неверный пароль или аккаунт заблокирован");

                loginBtn.BeginInvoke((Action)delegate
                {
                    loginBtn.BackColor = Color.DarkGray;
                    loginBtn.Enabled = true;
                });
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
            panel1.BeginInvoke((Action)delegate
            {
                panel1.Enabled = true;
                recurs_check.Enabled = true;
                recurs_stop_btn.Enabled = false;
                recurs_stop_btn.BackColor = Color.WhiteSmoke;
            });
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
            panel1.Enabled = false;
            recurs_check.Enabled = false;

            recurs_stop_btn.Invoke(new Action(() =>
            {
                recurs_stop_btn.Enabled = true;
                recurs_stop_btn.BackColor = Color.DarkGray;
            }));

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
                clean_btn.Enabled = true;
                clean_btn.BackColor = Color.DarkGray;
            }
        }

        private void clean_btn_Click(object sender, EventArgs e)
        {
            groupList.Items.Clear();
            clean_btn.Enabled = false;
            clean_btn.BackColor = Color.WhiteSmoke;
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
                antigateKey_TB.Enabled = false;
                check_balance_btn.Enabled = false;
                check_balance_btn.BackColor = Color.WhiteSmoke;
            }
            else
            {
                antigateKey_TB.Enabled = true;
                check_balance_btn.Enabled = true;
                check_balance_btn.BackColor = Color.DarkGray;
            }
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
                loginTextBox.Enabled = true;
                passTextBox.Enabled = true;
                loginBtn.Enabled = true;
                loginBtn.BackColor = Color.DarkGray;
                logout_btn.Enabled = false;
                logout_btn.BackColor = Color.WhiteSmoke;
                loadFromFile_btn.Enabled = false;
                loadFromFile_btn.BackColor = Color.WhiteSmoke;
                clean_btn.Enabled = false;
                clean_btn.BackColor = Color.WhiteSmoke;
                messageTB.Enabled = false;
                postBtn.Enabled = false;
                postBtn.BackColor = Color.WhiteSmoke;
                waitFrom_tb.Enabled = false;
                waitTo_tb.Enabled = false;
        }
        
        private void recurs_stop_btn_Click(object sender, EventArgs e)
        {
            recurs_check.Checked = false;
            recurs_stop_btn.Enabled = false;
            recurs_stop_btn.BackColor = Color.WhiteSmoke;
            MessageBox.Show("Ожидаем отправку последнего сообщения");
            //panel1.Enabled = true;
            //recurs_check.Enabled = true;
        }

    }
}
