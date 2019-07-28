using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FixMusic
{
    class CleanNames
    {
        //(?!\]?!\)\W+|^)grateful dead(?!\[?!\(\W+)|(?!\]?!\)\W+)grateful dead(?!\[?!\(\W+|$)
        const string PatternTemplate = @"(\s*[^])\w])*{0}([^[(\w]\s*)*"; //@"(((\W+|^){0}\W+)|(\W+{0}(\W+|$)))";
        static public void CMDExecute(string[] args)
        {
            string path = Environment.CurrentDirectory;
            var cleanNames = new CleanNames();
            cleanNames.RemoveParentNameFromChilds(path);
        }

        private void RemoveParentNameFromChilds(string path)
        {
            string topFolder = Path.GetFileName(path);
            string pattern = string.Format(PatternTemplate, Regex.Escape(topFolder));

            string[] entities = Directory.GetFileSystemEntries(path);
            foreach (string entity in entities)
                RegexFix.RecursiveRegexReplaceFSEntities(pattern, "", entity);

            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
                RemoveParentNameFromChilds(folder);
            //string[] files = 
        }
    }
}
