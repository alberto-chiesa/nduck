using System;
using System.Collections.Generic;

namespace IronDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Property
    /// </summary>
    public class MethodData
    {
        /// <summary>
        /// The name of the method.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The Accessor specified for this Method.
        /// </summary>
        public AccessorType Accessor { get; set; }

        /// <summary>
        /// The full name of the Return Type for this method.
        /// </summary>
        public String ReturnType { get; set; }

        /// <summary>
        /// Represents a list of the parameters to be passed to this method
        /// </summary>
        public List<ParameterData> Parameters { get; set; }

        /// <summary>
        /// The text of the documentation summary
        /// related to this method.
        /// </summary>
        public String SummaryText { get; set; }

        /// <summary>
        /// The text of the documentation remarks section,
        /// related to this method.
        /// </summary>
        public String RemarksText { get; set; }

        /// <summary>
        /// The text for the Example section.
        /// </summary>
        public String ExampleText { get; set; }

        /// <summary>
        /// Represents the 
        /// </summary>
        public class ParameterData
        {
            /// <summary>
            /// The name of the parameter.
            /// </summary>
            public String Name { get; set; }

            /// <summary>
            /// The full name of the Type of the parameter.
            /// </summary>
            public String Type { get; set; }

            /// <summary>
            /// True if the parameter is configured as Out.
            /// </summary>
            public Boolean IsOut { get; set; }

            /// <summary>
            /// True if the parameter is configured as Out.
            /// </summary>
            public Boolean IsRef { get; set; }
        }
    }
}