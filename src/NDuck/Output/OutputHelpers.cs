﻿using System;
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
        public static String StripDocTags(this XElement tag)
        {
            return null;
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

            var lines = RowSplit.Split(xml.Value).SkipWhile(String.IsNullOrEmpty).ToArray();

            var prefix = FindPrefix(lines) ?? string.Empty;

            // do not return the last row, if it's only a prefix
            return (lines[lines.Length - 1] == prefix) ?
                String.Join("\n", lines.Take(lines.Length - 1).Select(s => s.Substring(prefix.Length))) :
                String.Join("\n", lines.Select(s => s.Substring(prefix.Length)));
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

        private static IEnumerable<string> ExtractIndentationWhitespace(string[] lines)
        {
            return lines.Select(line => IndentWhiteSpace.Match(line))
                .Select(m => m.Success ? m.Value : String.Empty);
        }
    }
}
