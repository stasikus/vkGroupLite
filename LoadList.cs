using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vkGroupWall
{
    class LoadList
    {
        public static List<string> groups = new List<string>();
        public static List<string> proxys = new List<string>();

        public static void loadGroupList(string path)
        {
            groups = new List<string>();
            using (var sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    groups.Add(sr.ReadLine());
                    if (groups.Contains(""))
                    {
                        groups = groups.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    }
                }
            }
        }

        public static void loadProxyList(string path)
        {
            var logFile = ReadLogLines(path);
            foreach (var s in logFile)
            {
                proxys.Add(s);
            }
        }

        public static IEnumerable<string> ReadLogLines(string filePath)
        {
            using (StreamReader reader = File.OpenText(filePath))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
