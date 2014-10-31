using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace vkGroupWall
{
    class vkLogin
    {
        public static int Login(string login, string password, string html, Net http, string proxys)
        {
            string userid = "";
            string post = "email=" + login + "&pass=" + System.Web.HttpUtility.UrlEncode(password,Encoding.GetEncoding(1251)) + "&q=1&act=login&q=1&al_frame=1&expire=&captcha_sid=&captcha_key=&from_host=vk.com&from_protocol=http&ip_h=4e78766a2890ac1115&quick_expire=1";

            html = http.GetHtml("https://vk.com/", "", proxys);
            html = http.GetHtml("https://login.vk.com/?act=login", post, proxys);

            Regex rex4 = new Regex("parent\\.onLoginDone\\(\'(.*?)\'\\)", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            Match matc4 = rex4.Match(html);
            userid = matc4.Groups[1].ToString().Replace("/", "");

            html = http.GetHtml("https://vk.com/" + userid, "", proxys);

            if (html.IndexOf("login?act=blocked") > 0)
            {
                return 2;
            }
            if (html.IndexOf("onLoginFailed") > 0)
            {
                return 3;
            }
            Regex rex1 = new Regex("href=\"\\/edit\"", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            Match matc1 = rex1.Match(html);

            if (matc1.Groups[0].Length == 0)
            {
                return 4;
            }
            return 1;
        }
    }
}
