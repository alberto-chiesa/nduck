using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NDuck.XmlDoc;

namespace NDuck.Data
{
    /// <summary>
    /// Interface defining elements in the type
    /// hierarchy, as extracted from the assembly.
    /// </summary>
    interface IDocumentable
    {
        /// <summary>
        /// Loads the documentation extracted from
        /// a Visual Studio xml documentation file
        /// into this instance.
        /// </summary>
        /// <param name="doc">
        /// A member documentation object.
        /// </param>
        void LoadDocumentation(XmlMemberDoc doc);

        /// <summary>
        /// Summary tag.
        /// </summary>
        XElement Summary { get; set; }

        /// <summary>
        /// Example tag.
        /// </summary>
        XElement Example { get; set; }

        /// <summary>
        /// Remarks tag.
        /// </summary>
        XElement Remarks { get; set; }

        /// <summary>
        /// A List of Param tags.
        /// </summary>
        List<XElement> ParamList { get; set; }

        /// <summary>
        /// A List of Param tags.
        /// </summary>
        List<XElement> TypeParamList { get; set; }

        /// <summary>
        /// Contains the list of Exception tags
        /// </summary>
        List<XElement> ExceptionList { get; set; }

        /// <summary>
        /// Contains the list of Exception tags
        /// </summary>
        XElement Value { get; set; }
    }
}
