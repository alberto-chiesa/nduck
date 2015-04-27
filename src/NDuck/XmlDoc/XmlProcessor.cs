using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NDuck.XmlDoc
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
                Logger.Debug("Reading xml doc file...");
                var result = new XmlDocumentation();

                var xml = XDocument.Parse(xmlString);

                var docNode = xml.Element("doc");

                if (docNode == null)
                    throw new InvalidOperationException(@"There was an error reading the provided XML Documentation file: no <doc> node was found.");

                result.AssemblyName = ReadAssemblyName(docNode);

                result.Members = ReadMembers(docNode);

                return result;
            }
            catch (Exception e)
            {
                Logger.Error("There was an error reading xml documentation file for {0}: {1}.", xmlString, e.Message);
                throw;
            }
        }

        private static List<XmlMemberDoc> ReadMembers(XElement docNode)
        {
            var members = docNode.Element("members");

            if (members == null)
            {
                Logger.Warn("Could not find members tag in the provided xml file."); 
                return new List<XmlMemberDoc>();
            }

            return members.Elements("member").Select(memberXml => new XmlMemberDoc(memberXml)).ToList();
        }

        private static string ReadAssemblyName(XElement docNode)
        {
            var assemblyNode = docNode.Element("assembly");
            
            if (assemblyNode == null) return null;
            
            var theName = assemblyNode.Element("name");
            
            return theName != null ? theName.Value : null;
        }

        /// <summary>
        /// Processes the passed in xml document,
        /// extracting the relevant information.
        /// </summary>
        /// <param name="xmlDocumentationFilePath">
        /// A path for the xml file to be read.
        /// </param>
        /// <returns>
        /// A ready to use documentation object.
        /// </returns>
        public static XmlDocumentation ReadXmlDocumentation(string xmlDocumentationFilePath)
        {
            Logger.Debug("Processing {0} xml doc...", xmlDocumentationFilePath);

            if (xmlDocumentationFilePath == null) throw new ArgumentNullException("xmlDocumentationFilePath");

            if (!File.Exists(xmlDocumentationFilePath))
                throw new InvalidOperationException("The xml documentation file " + xmlDocumentationFilePath + " was not found.");

            var xml = File.ReadAllText(xmlDocumentationFilePath, Encoding.UTF8);

            var proc = new XmlProcessor();
            
            return proc.ProcessXml(xml);
        }
    }
}
