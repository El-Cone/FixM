using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FixMusic
{
    static class CleanNames
    {
        //(?!\]?!\)\W+|^)grateful dead(?!\[?!\(\W+)|(?!\]?!\)\W+)grateful dead(?!\[?!\(\W+|$)
        const string PatternTemplate = @"(\s*[^])\w])*{0}([^[(\w]\s*)*"; //@"(((\W+|^){0}\W+)|(\W+{0}(\W+|$)))";
        static public void CMDExecute(string[] args)
        {
            string path = Environment.CurrentDirectory;
            WriteConsole("Removing parent names from childs");
            RemoveParentNameFromChilds(path);
            WriteConsole("Removing empty brackets");
            RemoveEmptyBrackets(path);
            WriteConsole("Trimming");
            TrimAllNames(path);
        }

        static private void WriteConsole(string text)
        {
            WriteConsole(text, ConsoleColor.Magenta);
        }

        static private void WriteConsole(string text, ConsoleColor foregroundColor)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(text);
            Console.ForegroundColor = prev;
        }

        static private void RemoveParentNameFromChilds(string path)
        {
            string topFolder = Path.GetFileName(path);
            string pattern = string.Format(PatternTemplate, Regex.Escape(topFolder));

            RegexFix.RecursiveRegexReplaceFSEntities(pattern, " ", path);

            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
                RemoveParentNameFromChilds(folder);
        }

        static private void RemoveEmptyBrackets(string path)
        {
            RegexFix.RecursiveRegexReplaceFSEntities(@"\s*[\[\(]\W*[\]\)]\s*", " ", path);
        }

        static private void TrimAllNames(string path)
        {
            RegexFix.RecursiveRegexReplaceFSEntities(@"\s+", " ", path); //remove double spaces
            RegexFix.RecursiveRegexReplaceFSEntities(@"^\s+", "", path); //remove leading spaces
            RegexFix.RecursiveRegexReplaceFSEntities(@"\s+$", "", path); //remove trailing spaces
        }
    }
}
