﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixMusic
{
    static class MoveToParentWithAttibute
    {
        public static void CMDExecute(string[] args)
        {
            string path = Environment.CurrentDirectory;
            string attribute;
            if (args.Length > 1)
                attribute = $"[args(1)]";
            else
                attribute = $"[{Path.GetFileName(path)}]";
            string parent = path.Substring(0, path.LastIndexOf("\\"));
            ConsoleColor fgColor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($@"Do you want to give every item in 
{path}
the prefix {attribute} and move it to the parent?
(y/n)");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = fgColor;
            if (Console.ReadKey(true).Key != ConsoleKey.Y)
                return;
            foreach (var f in Directory.GetFiles(path))
                File.Move(f, Path.Combine(parent, $"{attribute} {Path.GetFileName(f)}"));
            foreach (var d in Directory.GetDirectories(path))
                Directory.Move(d, Path.Combine(parent, $"{attribute} {Path.GetFileName(d)}"));
            Directory.Delete(path);
        }

    }
}
