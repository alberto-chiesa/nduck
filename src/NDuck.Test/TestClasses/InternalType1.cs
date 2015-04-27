using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Local

namespace NDuck.TestClasses
{
    /// <summary>
    /// Some text to be used to test
    ///   * formatting
    ///   * whitespace preservation
    ///   * MarkDown
    /// </summary>
    /// <typeparam name="T1">T1</typeparam>
    /// <typeparam name="T2">T2</typeparam>
    internal class InternalType1<T1, T2>
    {
        /// <summary>
        /// Internal enumeration
        /// </summary>
        internal enum InternalEnum
        {
            /// <summary>
            /// v1
            /// </summary>
            Value1,
            /// <summary>
            /// v2
            /// </summary>
            Value2
        }

        /// <summary>
        /// Private test class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class PrivateClass<T>
        {
            /// <summary>
            /// A generic type property.
            /// </summary>
            public T Prop { get; set; }
        }
    }
}
