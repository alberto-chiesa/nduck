using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDuck.Data;
using NDuck.TestData;
using NUnit.Framework;

namespace NDuck.XmlDoc
{
    [TestFixture]
    public class XmlProcessorTest
    {
        public XmlProcessor Proc { get; set; }

        public XmlDocumentation DocLog4Net { get; set; }

        public XmlDocumentation DocNh { get; set; }

        [TestFixtureSetUp]
        public void SetupProcessor()
        {
            Proc = new XmlProcessor();
            DocLog4Net = Proc.ProcessXml(XmlTestDocs.log4net);
            DocNh = Proc.ProcessXml(XmlTestDocs.NHibernate);
        }

        [Test]
        public void TestAssemblyNameReading()
        {
            Assert.That(DocLog4Net.AssemblyName, Is.EqualTo("log4net"));

            Assert.That(DocNh.AssemblyName, Is.EqualTo("NHibernate"));
        }

        [Test]
        public void TestMemberNameRead()
        {
            Assert.That(DocLog4Net.Members.Count, Is.GreaterThan(0));
            Assert.That(DocLog4Net.Members[0].Name, Is.EqualTo("log4net.Appender.AdoNetAppender"));
            Assert.That(DocLog4Net.Members[0].Type, Is.EqualTo(MemberType.Type));
            Assert.That(DocLog4Net.Members[4].Name, Is.EqualTo("log4net.Appender.IAppender.Close"));
            Assert.That(DocLog4Net.Members[4].Type, Is.EqualTo(MemberType.Method));

            Assert.That(DocNh.Members.Count, Is.GreaterThan(0));
            Assert.That(DocNh.Members[0].Name, Is.EqualTo("NHibernate.Action.BulkOperationCleanupAction"));
            Assert.That(DocNh.Members[0].Type, Is.EqualTo(MemberType.Type));
            Assert.That(DocNh.Members[0].SummaryXml, Is.Not.Null);
        }

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
        }
    }
}
