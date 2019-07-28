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
        static int Renames;
        static void Main(string[] args)
        {
            try
            {
#if DEBUG
                Environment.CurrentDirectory = @"M:\Music\UnprocessedMusic\Grateful Dead\_Bootleg\Anthem of the Sun [Bonus Tracks]";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"DEBUG mode, current directory changed to:");
                Console.WriteLine(Environment.CurrentDirectory);
                Console.ForegroundColor = ConsoleColor.Gray;
#endif
                switch (args[0].ToLower())
                {
                    case "regex":
                        RegexFix.CMDExecute(args);
                        break;
                    case "cleannames":
                        CleanNames.CMDExecute(args);
                        break;
                    case "help":
                    case "-h":
                    case "?":
                        ShowHelp(args);
                        break;
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
                Console.ReadKey();
            }
        }
        static void ShowHelp(string[] args)
        {
            Console.WriteLine("regex pattern replace");
            Console.WriteLine
        }
    }
}
