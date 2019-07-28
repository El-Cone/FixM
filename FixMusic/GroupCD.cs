using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FixMusic
{
    class GroupCD
    {
        const string TopFolderCDPattern = @"(\s*[^])\w])*CD(\d)[^\\\/]*$"; //@"(\s*[^])\w])*CD(\d)$";
        const string SecondFolderCDPattern = @"\\CD(\d)[^\\\/]*$";
        public static void CMDExecute(string[] args)
        {
            string path = Environment.CurrentDirectory;
            CleanNames.RenameCDs(path);
            FindCDsInTopLayer(path);
            FindCDsInSecondLayer(path);
        }

        private static void FindCDsInTopLayer(string path)
        {
            string[] folders = Directory.GetDirectories(path);
            List<string> cds = new List<string>();
            foreach (string folder in folders)
            {
                if (Regex.Match(folder, TopFolderCDPattern).Success)
                {
                    string s = Regex.Replace(folder, TopFolderCDPattern, "");
                    if (!cds.Contains(s))
                        cds.Add(s);
                }
            }
            foreach (string s in cds)
                JoinCDs(path, s);
        }
        
        private static void JoinCDs(string path, string newPath)
        {
            string[] folders = Directory.GetDirectories(path);
            List<string> childs = folders.Where(x => x.StartsWith(newPath)).OrderBy(x => x).ToList();
            if (!int.TryParse(Regex.Match(childs.Last(), TopFolderCDPattern).Groups[2].ToString(), out int i ) || childs.Count != i)
                return;
            Directory.CreateDirectory(newPath);
            foreach (string child in childs)
            {
                string prefix = $"CD{ Regex.Match(child, TopFolderCDPattern).Groups[2].ToString()}";
                ExtractTo(newPath, child, prefix);
            }
        }

        private static void ExtractTo(string newPath, string source, string prefix)
        {
            RegexFix.RecursiveRegexReplaceFSEntities(string.Format(CleanNames.PatternTemplate, prefix), "", source);
            foreach (string s in Directory.GetFiles(source))
                File.Move(s, Path.Combine(newPath, $"{prefix} - {Path.GetFileName(s)}"));
            foreach (string s in Directory.GetDirectories(source))
                Directory.Move(s, Path.Combine(newPath, $"{prefix} - {Path.GetFileName(s)}"));
            Directory.Delete(source);
        }

        private static void FindCDsInSecondLayer(string path)
        {
            foreach (string parent in Directory.GetDirectories(path))
                foreach (string child in Directory.GetDirectories(parent))
                {
                    if (!Regex.Match(child, SecondFolderCDPattern).Success)
                        continue;
                    string prefix = $"CD{ Regex.Match(child, SecondFolderCDPattern).Groups[1].ToString()}";
                    ExtractTo(parent, child, prefix);
                }
        }
    }
}
