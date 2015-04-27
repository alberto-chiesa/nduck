using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDuck.TestClasses
{
    /// <summary>
    /// Just a test struct.
    /// </summary>
    struct Struct1
    {
        /// <summary>
        /// Private field.
        /// </summary>
#pragma warning disable 169
        int a;
#pragma warning restore 169
    }
}
