using System.Collections.Generic;
using System.Xml.Linq;
using Mono.Cecil;
using NDuck.XmlDoc;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to an Event
    /// </summary>
    public class EventData : DocumentableBase
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
            Logger.Debug("Reading event {0}...", eventDefinition.Name);

            Name = eventDefinition.Name;
            FullName = eventDefinition.FullName;
        }
    }
}