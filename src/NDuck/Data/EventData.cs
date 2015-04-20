using System.Collections.Generic;
using System.Xml.Linq;
using Mono.Cecil;
using NDuck.XmlDoc;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to an Event
    /// </summary>
    public class EventData : IDocumentable
    {
        /// <summary>
        /// The name of the Event
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The name of the Event
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EventData()
        {
        }

        /// <summary>
        /// Standard constructor used for
        /// metadata mapping
        /// </summary>
        /// <param name="eventDefinition">
        /// Cecile event definition.
        /// </param>
        public EventData(EventDefinition eventDefinition)
        {
            Name = eventDefinition.Name;
            FullName = eventDefinition.FullName;
        }

        public void LoadDocumentation(XmlMemberDoc doc)
        {
            
        }

        /// <summary>
        /// Links to the Xml Documentation Summary tag.
        /// </summary>
        public XElement Summary { get; set; }

        /// <summary>
        /// Links to the Xml Documentation Example tag.
        /// </summary>
        public XElement Example { get; set; }

        /// <summary>
        /// Links to the Xml Documentation Remarks tag.
        /// </summary>
        public XElement Remarks { get; set; }

        /// <summary>
        /// Links to the list of Xml Documentation Param tags.
        /// </summary>
        public List<XElement> ParamList { get; set; }

        /// <summary>
        /// Links to the list of Xml Documentation TypeParam tags.
        /// </summary>
        public List<XElement> TypeParamList { get; set; }

        /// <summary>
        /// Links to the list of Xml Documentation Exception tags.
        /// </summary>
        public List<XElement> ExceptionList { get; set; }

        /// <summary>
        /// Links to the Xml Documentation Value tag.
        /// </summary>
        public XElement Value { get; set; }
    }
}