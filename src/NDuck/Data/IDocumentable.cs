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
    public interface IDocumentable
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
        /// Stores a link to the Xml Documentation
        /// for the current documentable object.
        /// </summary>
        XmlMemberDoc Documentation { get; set; }
    }
}
