using System;
using System.Collections.Generic;

namespace IronDuck.Data
{
    /// <summary>
    /// Class representing a &lt;member&gt; xml tag,
    /// extracted from the documentation file
    /// </summary>
    public class XmlMemberDoc
    {
        private static readonly Dictionary<char, MemberType> CharToTypeDict = new Dictionary<char, MemberType>()
        {
            {'E', MemberType.Event},
            {'F', MemberType.Field},
            {'M', MemberType.Method},
            {'P', MemberType.Property},
            {'T', MemberType.Type}
        };

        /// <summary>
        /// Name of the member
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Represents the type of the member
        /// </summary>
        public MemberType Type { get; set; }

        /// <summary>
        /// Reads the name xml attribute value and converts
        /// it into a name and a type
        /// </summary>
        /// <param name="name">
        /// The string read from the Xml.
        /// Must be in the form X:membername.
        /// </param>
        public void ReadName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The name of a member tag must not be null or empty.");

            if (name.Length < 3 || name[1] != ':')
                throw new ArgumentException("The name of a member should be in the form \"X:memberName\" where X is T, M, F or P.");

            Name = name.Substring(2);

            var t = name[0];

            if (!CharToTypeDict.ContainsKey(t))
                throw new InvalidOperationException("The character '" + name[0] + "' is not recognized as a valid member type.");

            Type = CharToTypeDict[t];
        }

    }
}