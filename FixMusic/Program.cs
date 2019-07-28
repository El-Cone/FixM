using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FixMusic
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
#if DEBUG
                Environment.CurrentDirectory = @"M:\Music\UnprocessedMusic\Grateful Dead\_Bootleg";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"DEBUG mode, current directory changed to:");
                Console.WriteLine(Environment.CurrentDirectory);
                Console.ForegroundColor = ConsoleColor.Gray;
#endif
                switch (args.Length == 0 ? "?" : args[0].ToLower())
                {
                    case "batch": Batch(); break;
                    case "cleannames": CleanNames.CMDExecute(args); break;
                    case "files": Files.CMDExecute(args); break;
                    case "groupcd": GroupCD.CMDExecute(args); break;
                    case "movetoparentwithattibute": MoveToParentWithAttibute.CMDExecute(args); break;
                    case "regex": RegexFix.CMDExecute(args); break;
                    case "help": case "-h": case "?": ShowHelp(args); break;
                    default:
                        Console.WriteLine("unknown command, type help for help.");
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
#if DEBUG
                Console.ReadKey();
#endif
            }
        }
        static void ShowHelp(string[] args)
        {
            Console.WriteLine("regex pattern replace");
            Console.WriteLine("cleannames");
            Console.WriteLine("groupCD");
            Console.WriteLine("MoveToParentWithAttibute");
        }
        static void Batch()
        {
            Files.CMDExecute("", "rem", "txt", "nfo", "log", "accurip", "dat", "exe", "inf", "html", "htm", "swf", "x32", "x16");
            Files.CMDExecute("", "move", "cue", "m3u");
            CleanNames.CMDExecute(null);
            GroupCD.CMDExecute(null);
            removed = 0;
            Console.WriteLine("Removing empty directories");
            RemoveEmptyDirectories(Environment.CurrentDirectory);
            Console.WriteLine($"{removed} Directories removed.");
        }

        static int removed;
        private static void RemoveEmptyDirectories(string path)
        {
            foreach (var d in Directory.GetDirectories(path))
                RemoveEmptyDirectories(d);
            if (Directory.GetFileSystemEntries(path).Length == 0)
            {
                Console.WriteLine($"Removed \"{ path}\"");
                Directory.Delete(path);
                removed++;
            }
        }
    }
}
