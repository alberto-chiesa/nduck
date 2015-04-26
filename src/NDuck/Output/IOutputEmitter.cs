using System;
using System.Collections.Generic;
using System.Linq;
using NDuck.Config;

namespace NDuck.Output
{
    /// <summary>
    /// An Output Emitter gathers
    /// all the data contained in a
    /// <see cref="NDuck.TypeRepository"/> and transforms it.
    /// The kind of operation is 
    /// </summary>
    public interface IOutputEmitter
    {
        /// <summary>
        /// Defines a name to be used
        /// for type resolution from
        /// the command line.
        /// </summary>
        string EmitterName { get; }

        /// <summary>
        /// Processes the information gathered
        /// in a <see cref="NDuck.TypeRepository"/>
        /// and outputs it.
        /// </summary>
        /// <param name="repository">
        /// A pre-filled TypeRepository.
        /// </param>
        /// <param name="options">
        /// Execution options, as read from the
        /// command line or a project file.
        /// </param>
        void EmitOutput(TypeRepository repository, ExecutionOptions options);
    }
}

