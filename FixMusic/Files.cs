using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixMusic
{
    static class Files
    {
        static private int Counter;
        public static void CMDExecute(params string[] args)
        {
            string path = Environment.CurrentDirectory;
            if (args.Length < 3)
                throw new ArgumentNullException("more parameters needed");
            string[] extentions = new string[args.Length - 2];
            Array.Copy(args, 2, extentions, 0, extentions.Length);
            Counter = 0;
            ConsoleColor fgColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                if (args[1].StartsWith("rem", StringComparison.InvariantCultureIgnoreCase))
                {
                    RemoveFilesWithExtentions(path, extentions);
                    Console.WriteLine($"{Counter} files with extentions ({string.Join(",", extentions)}) removed");
                }
                else if (args[1].StartsWith("move", StringComparison.InvariantCultureIgnoreCase))
                {
                    MoveFilesWithExtentions(path, extentions);
                    Console.WriteLine($"{Counter} files with extentions ({string.Join(",", extentions)}) moved");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Don't know what to do, try files remove or files move and add some extentions");
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Crashed. {Counter} files successfully processed.");
            }
            finally
            {
                Console.ForegroundColor = fgColor;
            }
        }
        public static void RemoveFilesWithExtentions(string path, string[] extentions)
        {
            foreach (var s in Directory.GetDirectories(path))
                RemoveFilesWithExtentions(s, extentions);
            foreach (var f in Directory.GetFiles(path))
                foreach (var e in extentions)
                    if (Path.GetExtension(f).Equals($".{e}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        File.Delete(f);
                        Counter++;
                    }
        }
        public static void MoveFilesWithExtentions(string path, string[] extentions)
        {
            foreach (var s in Directory.GetDirectories(path))
                RemoveFilesWithExtentions(s, extentions);
            foreach (var f in Directory.GetFiles(path))
                foreach (var e in extentions)
                    if (Path.GetExtension(f).Equals($".{e}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!Directory.Exists(Path.Combine(path, e.ToUpper())))
                            Directory.CreateDirectory(Path.Combine(path, e.ToUpper()));
                        File.Move(Path.Combine(path, Path.GetFileName(f)), Path.Combine(path, e.ToUpper(), Path.GetFileName(f)));
                        Counter++;
                    }
        }
    }
}
