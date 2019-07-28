using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FixMusic
{
    class RegexFix
    {
        public int Renames { get; private set; }
        public bool Recursive { get; set; }
        public string Pattern { get; set; }
        public string Replace { get; set; }

        public RegexFix(string pattern, string replace)
        {
            Pattern = pattern;
            Replace = replace;
        }

        public static void CMDExecute(string[] args)
        {
            //Parse args
            string pattern = args[1];
            string replace = args.Length >= 3 ? args[2] : "";
            string path = Environment.CurrentDirectory;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($@"Do you want to replace '{pattern}' with '{replace}' in
{path}
all files and subfolders? (y/n)");
            if (Console.ReadKey(true).Key != ConsoleKey.Y)
                return;

            // do work
            RegexFix fix = new RegexFix(pattern, replace);
            try
            {
                fix.RecursiveRegexReplaceFSEntities(path);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Crashed. {fix.Renames} Renames successfull.");
                throw;
            }
        }

        private static void WriteConsole(string type, string old, string nw)
        {
            if (type.Length > 15)
                type = type.Substring(0, 15);
            string lenT = "               ";
            lenT = lenT.Substring(0, type.Length);
            if (old.Length < 32)
                Console.WriteLine($"Renaming {type}: \"{old}\" TO: \"{nw}\"");
            else
            {
                Console.WriteLine($"Renaming {type}: \"{old}\"");
                Console.WriteLine($"     {lenT}  to: \"{nw}\"");
            }
        }

        public void RegexReplaceFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string oldFileName = Path.GetFileName(file);
                string newFileName = Regex.Replace(oldFileName, Pattern, Replace, RegexOptions.IgnoreCase);
                string parentPath = file.Substring(0, file.LastIndexOf(oldFileName));
                if (oldFileName != newFileName && Regex.Match(newFileName, @"\w.*\.\w+").Success)
                {
                    WriteConsole("file", oldFileName, newFileName);
                    File.Move(file, Path.Combine(parentPath, newFileName));
                    Renames++;
                }
            }
        }

        private void RegexReplaceDirectory(string dir)
        {
            string oldDirName = dir.Split('\\').Last();
            if (!Regex.Match(oldDirName, Pattern, RegexOptions.IgnoreCase).Success)
                return;
            string newDirName = Regex.Replace(oldDirName, Pattern, Replace, RegexOptions.IgnoreCase);
            string parentPath = dir.Substring(0, dir.LastIndexOf(oldDirName));
            if (oldDirName != newDirName && Regex.Match(newDirName, @"\w").Success)
            {
                WriteConsole("directory", oldDirName, newDirName);
                Directory.Move(dir, Path.Combine(parentPath, newDirName));
                Renames++;
            }
        }

        public static void RecursiveRegexReplaceFSEntities(string pattern, string replace, string path)
        {
            new RegexFix(pattern, replace).RecursiveRegexReplaceFSEntities(path);
        }

        public void RecursiveRegexReplaceFSEntities(string path)
        {
            ConsoleColor fgColor = Console.ForegroundColor;
            var timer = new System.Diagnostics.Stopwatch();
            Console.ForegroundColor = ConsoleColor.Cyan;
            timer.Start();
            RecursiveRegexReplaceFSEntities(path, false);
            timer.Stop();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string time;
            if (timer.ElapsedMilliseconds > 15000)
                time = timer.Elapsed.ToString();
            else
                time = timer.ElapsedMilliseconds + "ms";
            if (Renames > 0)
                Console.WriteLine($"{Renames} Renames done in {time}");
            Console.ForegroundColor = fgColor;
        }

        private void RecursiveRegexReplaceFSEntities(string path, bool includingThisOne)
        {
            string[] folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            RegexReplaceFiles(path);
            foreach (string folder in folders)
                RecursiveRegexReplaceFSEntities(folder, true);
            if (includingThisOne)
                RegexReplaceDirectory(path);
        }
    }
}
