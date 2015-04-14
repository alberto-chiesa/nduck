using Mono.Cecil;

namespace IronDuck.Data
{
    /// <summary>
    /// Class containing every information related to an Event
    /// </summary>
    public class EventData
    {
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
        }
    }
}