using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IronDuck.XmlDoc
{
    /// <summary>
    /// Utility Class containing methods used for Xml
    /// data extraction and manipulation
    /// </summary>
    public static class XmlExtensions
    {
        private static readonly Regex RowSplit = new Regex("\r\n|\r|\n");

        private static readonly Regex IndentWhiteSpace = new Regex("( |\t)*");

        /// <summary>
        /// Takes the text inside an <see cref="System.Xml.Linq.XElement"/>, 
        /// searches for a common indentation pattern and removes the
        /// indentation common to all the block, when available.
        /// </summary>
        /// <param name="xml">An xml element to be de-indented</param>
        /// <returns>The xml tag content as a string.</returns>
        public static String ValueUnindented(this XElement xml)
        {
            if (xml == null) return null;

            var lines = RowSplit.Split(xml.Value).SkipWhile(String.IsNullOrEmpty).ToArray();

            var prefix = FindPrefix(lines);

            if (String.IsNullOrEmpty(prefix)) return String.Join("\n", lines);

            var prefixLength = prefix.Length;

            // do not return the last row, if it's only a prefix
            return String.Join("\n",
                lines.Take((lines[lines.Length - 1] == prefix) ? lines.Length - 1 : lines.Length)
                    .Select(s => s.Substring(prefixLength)));
        }

        /// <summary>
        /// Finds the common string prefix
        /// between the provided lines of text.
        /// </summary>
        /// <param name="lines">A non empty array of lines</param>
        /// <returns></returns>
        public static string FindPrefix(string[] lines)
        {
            if (lines == null) throw new ArgumentNullException("lines");

            string prefix = null;

            foreach (var line in lines)
            {
                var m = IndentWhiteSpace.Match(line);
                var linePrefix = m.Success ? m.Value : String.Empty;
                if (prefix == null)
                {
                    prefix = linePrefix;
                }
                else if (prefix.Length < linePrefix.Length)
                {
                    if (!linePrefix.StartsWith(prefix)) return null;
                }
                else if (prefix.Length > linePrefix.Length)
                {
                    if (!prefix.StartsWith(linePrefix)) return null;
                    prefix = linePrefix;
                }
                else
                {
                    if (prefix != linePrefix) return null;
                }
            }

            return prefix;
        }

    }
}
