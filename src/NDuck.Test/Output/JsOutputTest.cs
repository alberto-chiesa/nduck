using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDuck.TestData;
using NDuck.XmlDoc;
using NUnit.Framework;

namespace NDuck.Output
{
    /// <summary>
    /// Tests for the conversion procedure
    /// that takes the data as read into a TypeRepository
    /// and converts it into json objects, ready to be
    /// consumed by a JavaScript application.
    /// </summary>
    [TestFixture]
    public class JsOutputTest
    {
        /// <summary>
        /// Pre-read xml documentation.
        /// </summary>
        [TestFixtureSetUp]
        public void PrepareXmlDocumentation()
        {
            var proc = new XmlProcessor();
            DocLog4Net = proc.ProcessXml(XmlTestDocs.log4net);
            DocNh = proc.ProcessXml(XmlTestDocs.NHibernate);
            DocSelf = XmlProcessor.ReadXmlDocumentation(Path.Combine(".", "NDuck.Test.xml"));
        }

        private XmlDocumentation DocNh { get; set; }

        /// <summary>Just some comment
        ///   in order to check proper deindentation.
        /// </summary>
        private XmlDocumentation DocLog4Net { get; set; }

        /// <summary>
        /// Contains a reference to the documentation
        /// file for this assembly.
        /// </summary>
        public XmlDocumentation DocSelf { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void StripTagsHelperStripsTags()
        {
        }


        /// <summary>
        ///     This comment is useful to check
        ///     what happens with indented comments.
        /// </summary>
        [Test]
        public void TestUnindentXmlValue()
        {
            var expected = "Implementation of BulkOperationCleanupAction.";

            var actual = DocNh.Members[0].SummaryXml.ValueUnindented();

            Assert.That(actual, Is.EqualTo(expected));

            expected = "An operation which may be scheduled for later execution.\n" +
                "Usually, the operation is a database insert/update/delete,\n" +
                "together with required second-level cache management.";
            actual = DocNh.Members[1].SummaryXml.ValueUnindented();

            Assert.That(actual, Is.EqualTo(expected));

            expected = "Called before executing any actions";
            actual = DocNh.Members[2].SummaryXml.ValueUnindented();

            Assert.That(actual, Is.EqualTo(expected));

            var member = DocNh.Members.First(m => m.Name == "NHibernate.Action.DelayedPostInsertIdentifier");
            expected = "Acts as a stand-in for an entity identifier which is supposed to be\n" +
                       "generated on insert (like an IDENTITY column) where the insert needed to\n" +
                       "be delayed because we were outside a transaction when the persist\n" +
                       "occurred (save currently still performs the insert).\n" +
                       "\n" +
                       "The stand-in is only used within the see cref=\"NHibernate.Engine.PersistenceContext\"\n" +
                       "in order to distinguish one instance from another; it is never injected into\n" +
                       "the entity instance or returned to the client...";
            actual = member.SummaryXml.ValueUnindented();

            Assert.That(actual, Is.EqualTo(expected));

            member = DocSelf.Members.First(m => m.Type == MemberType.Type && m.Name.StartsWith("NDuck.TestClasses.PublicType1"));
            Assert.That(member, Is.Not.Null);

            member = DocSelf.Members.First(m => m.Type == MemberType.Type && m.Name.StartsWith("NDuck.TestClasses.InternalType1"));
            Assert.That(member.Name, Is.EqualTo("NDuck.TestClasses.InternalType1`2"));
            expected = "Some text to be used to test\n" +
                       "  * formatting\n" +
                       "  * whitespace preservation\n" +
                       "  * MarkDown";
            Assert.That(member.SummaryXml.ValueUnindented(), Is.EqualTo(expected));

            member = DocSelf.Members.First(m => m.Type == MemberType.Method && m.Name.StartsWith("NDuck.Output.JsOutputTest.TestUnindentXmlValue"));
            expected = "This comment is useful to check\n" +
                       "what happens with indented comments.";
            Assert.That(member.SummaryXml.ValueUnindented(), Is.EqualTo(expected));

            member = DocSelf.Members.First(m => m.Type == MemberType.Property && m.Name.StartsWith("NDuck.Output.JsOutputTest.DocLog4Net"));
            expected = "Just some comment\nin order to check proper deindentation.";
            Assert.That(member.SummaryXml.ValueUnindented(), Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that the xml doc comments are correctly stripped
        /// of the xml parts and converted to Markdown snippets.
        /// </summary>
        [Test]
        public void TestXmlToMarkdownConversion1()
        {
            var member = DocSelf.Members.Single(m => m.Name == "NDuck.TestClasses.CommentsExamples");
            
            var expectedSummary = "This class exists only to define a complete set of\n" +
                "all the comments defined. As an example, here I am\n" +
                "using a __<c> tag__ to represent some code.";
            Assert.That(member.SummaryXml.XmlDocToMd(), Is.EqualTo(expectedSummary));

            var expectedExamples = "As an example, some c# code snippet:\n" +
                "```\n" +
                "public void SomeMethod()\n" +
                "{\n" +
                "    A = B * C;\n" +
                "}\n" +
                "```";
            Assert.That(member.ExampleXml.XmlDocToMd(), Is.EqualTo(expectedExamples));
        }

        /// <summary>
        /// Tests that the xml doc comments are correctly stripped
        /// of the xml parts and converted to Markdown snippets.
        /// </summary>
        [Test]
        public void TestXmlToMarkdownConversion2()
        {
            var member = DocSelf.Members.Single(m => m.Name == "NDuck.TestClasses.CommentsExamples.Method1(System.String,System.String)");

            var expectedExamples = "As an example, some c# code snippet:\n" +
                "```\n" +
                "  public void SomeMethod()\n" +
                "  {\n" +
                "    A = B * C;\n" +
                "  }\n" +
                "```";
            Assert.That(member.ExampleXml.XmlDocToMd(), Is.EqualTo(expectedExamples));
        }
    }
}
