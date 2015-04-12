using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IronDuck.Data;

namespace IronDuck.XmlDoc
{
    /// <summary>
    /// Class responsible for
    /// processing of Xml Documentation files.
    /// </summary>
    public class XmlProcessor
    {
        /// <summary>
        /// Processes the passed in xml document,
        /// extracting the relevant information.
        /// </summary>
        /// <param name="xmlString">
        /// Content of a Xml Documentation File.
        /// </param>
        /// <returns></returns>
        public XmlDocumentation ProcessXml(String xmlString)
        {
            try
            {
                var result = new XmlDocumentation();

                var xml = XDocument.Parse(xmlString);

                var docNode = xml.Element("doc");

                if (docNode == null)
                    throw new InvalidOperationException(@"There was an error reading the provided XML Documentation file: no <doc> node was found.");

                result.AssemblyName = ReadName(docNode);

                result.Members = docNode.Element("members").Elements("member").Select(ProcessMember).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static string ReadName(XElement docNode)
        {
            var assemblyNode = docNode.Element("assembly");
            if (assemblyNode != null)
            {
                var theName = assemblyNode.Element("name");
                if (theName != null) return theName.Value;
            }

            return null;
        }

        private XmlMemberDoc ProcessMember(XElement arg)
        {
            var member = new XmlMemberDoc();

            member.ReadName(arg.Attribute("name").Value);

            return member;
        }
    }
}
