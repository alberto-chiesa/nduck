﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NDuck.XmlDoc
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
        /// Summary tag.
        /// </summary>
        public XElement SummaryXml { get; set; }

        /// <summary>
        /// Summary tag.
        /// </summary>
        public XElement ExampleXml { get; set; }

        /// <summary>
        /// Remarks tag.
        /// </summary>
        public XElement RemarksXml { get; set; }

        /// <summary>
        /// A List of Param tags.
        /// </summary>
        public List<XElement> ParamXmlList { get; set; }

        /// <summary>
        /// A List of Param tags.
        /// </summary>
        public List<XElement> TypeParamXmlList { get; set; }

        /// <summary>
        /// Contains the list of Exception tags
        /// </summary>
        public List<XElement> ExceptionXmlList { get; set; }

        /// <summary>
        /// Contains the list of Exception tags
        /// </summary>
        public XElement ValueXml { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public XmlMemberDoc()
        {
            
        }

        /// <summary>
        /// Extracts information from an Xml Element
        /// </summary>
        /// <param name="memberXml">
        /// </param>
        /// <threadsafety static="true" instance="false"/>
        /// <returns></returns>
        public XmlMemberDoc(XElement memberXml)
        {
            ReadNameAndType(memberXml.Attribute("name").Value);

            SummaryXml = memberXml.Element("summary");
            RemarksXml = memberXml.Element("remarks");
            ExampleXml = memberXml.Element("example");
            ValueXml = memberXml.Element("value");
            ParamXmlList = memberXml.Elements("param").ToList();
            TypeParamXmlList = memberXml.Elements("typeparam").ToList();
            ExceptionXmlList = memberXml.Elements("exception").ToList();
        }

        /// <summary>
        /// Reads the name xml attribute value and converts
        /// it into a name and a type
        /// </summary>
        /// <param name="name">
        /// The string read from the Xml.
        /// Must be in the form X:membername.
        /// </param>
        public void ReadNameAndType(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The name of a member tag must not be null or empty.");

            if (name.Length < 3 || name[1] != ':')
                throw new ArgumentException("The name of a member should be in the form \"X:memberName\" where X is T, M, F or P.");

            Name = name.Substring(2).Replace('{', '<').Replace('}', '>');

            var t = name[0];

            if (!CharToTypeDict.ContainsKey(t))
                throw new InvalidOperationException("The character '" + name[0] + "' is not recognized as a valid member type.");

            Type = CharToTypeDict[t];
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// The objects string representation.
        /// </returns>
        public override string ToString()
        {
            return String.Concat("<", Type, ">", Name);
        }
    }
}