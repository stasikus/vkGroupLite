using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vkGroupWall
{
    class LoadUsers
    {
        public static List<string> users = new List<string>();
        public static Dictionary<string, string> userDictionary = new Dictionary<string, string>();

        public static void loadList(string path)
        {
            users = new List<string>();
            userDictionary = new Dictionary<string, string>();

            using (var sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    users.Add(sr.ReadLine());
                    if (users.Contains(""))
                    {
                        users = users.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    }
                }
            }
            splitList();
        }

        public static void splitList()
        {
            try
            {
                for (int i = 0; i < users.Count; i++)
                {
                    userDictionary.Add(users[i].Substring(0, users[i].IndexOf(",")), users[i].Substring(users[i].IndexOf(",")+1));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Файл в неверном формате");
                users.Clear();
            }
            
        }

    }
}
