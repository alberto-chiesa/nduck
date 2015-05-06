using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NDuck.Output
{
    /// <summary>
    /// Contains various helpers to be used
    /// for documentation data manipulation 
    /// during the output phase.
    /// </summary>
    public static class OutputHelpers
    {
        /// <summary>
        /// Strips code documentation tags
        /// from the passed in <see cref="System.Xml.Linq.XElement"/>
        /// object instance.
        /// </summary>
        /// <param name="tag">
        /// An xml element (e.g. a summary tags content which will be
        /// stripped of every extraneous content.
        /// </param>
        /// <returns>
        /// An unindented string representation, with every doc tag
        /// replaced with a corresponding representation.
        /// </returns>
        public static String XmlDocToMd(this XElement tag)
        {
            if (tag == null) throw new ArgumentNullException("tag");

            try
            {
                var prefix = FindPrefix(tag.SplitLines()) ?? string.Empty;

                foreach (var el in tag.Elements())
                    ProcessXmlDocElement(el, prefix);

                return Unprefix(tag, prefix);
            }
            catch (Exception e)
            {
                var msg = String.Format("There was an error while converting an xml doc tag to MarkDown: {0}",
                    e.Message);
                throw new InvalidOperationException(msg, e);
            }
        }

        /// <summary>
        /// This method selects the appropriate conversion for every
        /// known xml doc tag into the corresponding markdown
        /// representation.
        /// </summary>
        /// <param name="el"></param>
        /// <param name="prefix"></param>
        /// <remarks>
        /// Please note that the xml element is replaced with the corresponding MD.
        /// </remarks>
        private static void ProcessXmlDocElement(XElement el, string prefix)
        {
            Object newEl;
            
            switch (el.Name.LocalName)
            {
                case "c":
                    newEl = ReplaceCNode(el);
                    break;
                case "code":
                    newEl = ReplaceCodeNode(el, prefix);
                    break;
                default:
                    newEl = Unprefix(el, prefix);
                    break;
            }

            el.ReplaceWith(newEl ?? String.Empty);
        }

        private static string ReplaceCodeNode(XElement el, string prefix)
        {
            var snippet = Unprefix(el, prefix);

            if (String.IsNullOrEmpty(snippet)) return String.Empty;

            return String.Concat("```",
                (snippet.StartsWith("\r") || snippet.StartsWith("\n")) ? "" : "\n",
                snippet,
                (snippet.EndsWith("\r") || snippet.EndsWith("\n")) ? "" : "\n",
                "```");
        }

        private static String ReplaceCNode(XElement el)
        {
            return "__" + el.Value + "__";
        }

        private static readonly Regex RowSplit = new Regex("\r\n|\r|\n");

        private static readonly Regex IndentWhiteSpace = new Regex("( |\t)*");

        /// <summary>
        /// Takes the text inside an <see cref="System.Xml.Linq.XElement"/>, 
        /// searches for a common indentation pattern and removes the
        /// indentation common to all the block, when available.
        /// </summary>
        /// <param name="xml">An xml element to be de-indented.</param>
        /// <returns>The xml tag content as a string.</returns>
        public static String ValueUnindented(this XElement xml)
        {
            if (xml == null) return null;

            var lines = xml.SplitLines();

            return Unprefix(lines, FindPrefix(lines));
        }

        private static string[] SplitLines(this XElement xml)
        {
            return RowSplit.Split(xml.Value).SkipWhile(String.IsNullOrEmpty).ToArray();
        }

        /// <summary>
        /// Strips a common prefix from an xml element content.
        /// </summary>
        /// <param name="el"></param>
        /// <param name="prefix"></param>
        /// <returns>
        /// The un-prefixed content of the element.
        /// </returns>
        private static String Unprefix(XElement el, string prefix)
        {
            return Unprefix(el.SplitLines(), prefix);
        }

        /// <summary>
        /// Strips a common prefix from an array of text lines.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="prefix"></param>
        /// <returns>
        /// The concatenation of the un-prefixed lines.
        /// </returns>
        private static string Unprefix(string[] lines, string prefix)
        {
            if (prefix == null) prefix = String.Empty;

            var unprefixedLines = lines.Select(s => s.StartsWith(prefix) ? s.Substring(prefix.Length) : s);

            // do not return the last row, if it's only a prefix
            if (lines[lines.Length - 1].Trim() == String.Empty)
                unprefixedLines = unprefixedLines.Take(lines.Length - 1);

            return String.Join("\n", unprefixedLines);
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

            foreach (var linePrefix in ExtractIndentationWhitespace(lines))
            {
                if (prefix == null)
                {
                    // this check skips initial lines.
                    if (!String.IsNullOrEmpty(linePrefix)) prefix = linePrefix;
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

        private static IEnumerable<string> ExtractIndentationWhitespace(string[] lines)
        {
            return lines
                .Where(s => s.Trim() != String.Empty)
                .Select(line => IndentWhiteSpace.Match(line))
                .Select(m => m.Success ? m.Value : String.Empty);
        }
    }
}
