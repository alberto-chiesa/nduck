using System;
using System.Linq;
using NDuck.XmlDoc;

namespace NDuck.Data
{
    /// <summary>
    /// Base class defining common behaviour for
    /// documentable code elements, such as types,
    /// methods, etc.
    /// </summary>
    public class DocumentableBase : IDocumentable
    {
        /// <summary>
        /// Stores a link to the Xml Documentation
        /// for the current documentable object.
        /// </summary>
        public virtual XmlMemberDoc Documentation { get; set; }

        /// <summary>
        /// Loads the documentation extracted from
        /// a Visual Studio xml documentation file
        /// into this instance.
        /// </summary>
        /// <param name="doc">
        /// A member documentation object.
        /// </param>
        public virtual void LoadDocumentation(XmlMemberDoc doc)
        {
            if (doc == null) throw new ArgumentNullException("doc");
            Documentation = doc;
        }
    }
}