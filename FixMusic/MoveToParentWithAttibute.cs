using System;
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
            string path = Path.Combine(Environment.CurrentDirectory, args[1]);
            string attribute;
            string folder = Path.GetFileName(path);
            if (args.Length > 2)
                attribute = $"[{args[2]}]";
            else
                attribute = $"[{folder}]";
            string parent = Environment.CurrentDirectory;
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
