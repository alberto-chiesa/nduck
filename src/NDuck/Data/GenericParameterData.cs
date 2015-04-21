using System;
using Mono.Cecil;

namespace NDuck.Data
{
    /// <summary>
    /// Class used to store data recovered
    /// by Cecil about generic type parameters.
    /// </summary>
    public class GenericParameterData
    {
        /// <summary>
        /// Name of the generic parameter.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Descriptive text extracted from the
        /// xml documentation.
        /// </summary>
        public String TypeParamText { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="gp"></param>
        public GenericParameterData(GenericParameter gp)
        {
            Logger.Debug("Reading generic parameter {0}...", gp.Name);

            Name = gp.Name;
        }

    }
}