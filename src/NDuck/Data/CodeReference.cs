using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace NDuck.Data
{
    /// <summary>
    /// CodeReference stores all the data
    /// representing a line/column reference extracted from 
    /// a pdb file. For each code element (e.g. properties and methods)
    /// read from classes the position of the body of the
    /// function is checked and linked.
    /// </summary>
    public class CodeReference
    {
        /// <summary>
        /// Standard Constructor,
        /// converting a Cecil SequencePoint
        /// into a CodeReference.
        /// </summary>
        /// <param name="sequencePoint">
        /// 
        /// </param>
        public CodeReference(SequencePoint sequencePoint)
        {
            if (sequencePoint == null)
                throw new ArgumentNullException("sequencePoint");

            FilePath = sequencePoint.Document.Url;
            StartLine = sequencePoint.StartLine;
            StartColumn = sequencePoint.StartColumn;
            EndLine = sequencePoint.EndLine;
            EndColumn = sequencePoint.EndColumn;
        }

        /// <summary>
        /// Path to the source file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Line number of the first character of
        /// the reference
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Column number of the first character of
        /// the reference
        /// </summary>
        public int StartColumn { get; set; }

        /// <summary>
        /// Line number of the last character of
        /// the reference
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// Column number of the last character of
        /// the reference
        /// </summary>
        public int EndColumn { get; set; }
    }
}
