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
            var timer = new System.Diagnostics.Stopwatch();
            try
            {
                timer.Start();
                Console.ForegroundColor = ConsoleColor.Cyan;
                fix.RecursiveRegexReplaceFSEntities(path);
                timer.Stop();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                string time;
                if (timer.ElapsedMilliseconds > 15000)
                    time = timer.Elapsed.ToString();
                else
                    time = timer.ElapsedMilliseconds + "ms";
                Console.WriteLine($"{fix.Renames} Renames done in {time}");
            }
            catch
            {
                Console.WriteLine($"Crashed. {fix.Renames} Renames successfull.");
                throw;
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
                if (oldFileName != newFileName)
                {
                    Console.WriteLine($"Renaming file {newFileName} from:({oldFileName})");
                    File.Move(file, Path.Combine(parentPath, newFileName));
                    Renames++;
                }
            }
        }

        private void RegexReplaceDirectory(string dir)
        {
            RegexReplaceFiles(dir);
            string oldDirName = dir.Split('\\').Last();
            if (!Regex.Match(oldDirName, Pattern, RegexOptions.IgnoreCase).Success)
                return;
            string newDirName = Regex.Replace(oldDirName, Pattern, Replace, RegexOptions.IgnoreCase);
            string parentPath = dir.Substring(0, dir.LastIndexOf(oldDirName));
            if (oldDirName != newDirName)
            {
                Console.WriteLine($"Renaming folder {newDirName} from:({oldDirName})");
                Directory.Move(dir, Path.Combine(parentPath, newDirName));
                Renames++;
            }
        }

        public void RecursiveRegexReplaceFSEntities(string path)
        {
            RecursiveRegexReplaceFSEntities(path, false);
        }

        private void RecursiveRegexReplaceFSEntities(string path, bool includingThisOne)
        {
            string[] folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (string folder in folders)
                RecursiveRegexReplaceFSEntities(folder, true);
            if (includingThisOne)
                RegexReplaceDirectory(path);
        }
    }
}
