using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronDuck.Data;
using IronDuck.TestData;
using NUnit.Framework;

namespace IronDuck.XmlDoc
{
    [TestFixture]
    public class XmlProcessorTest
    {
        public XmlProcessor proc { get; set; }

        [SetUp]
        public void SetupProcessor()
        {
            proc = new XmlProcessor();
        }

        [Test]
        public void TestAssemblyNameReading()
        {
            var res = proc.ProcessXml(XmlTestDocs.log4net);

            Assert.That(res.AssemblyName, Is.EqualTo("log4net"));

            res = proc.ProcessXml(XmlTestDocs.NHibernate);

            Assert.That(res.AssemblyName, Is.EqualTo("NHibernate"));
        }

        [Test]
        public void TestMemberNameRead()
        {
            var res = proc.ProcessXml(XmlTestDocs.log4net);

            Assert.That(res.Members.Count, Is.GreaterThan(0));
            Assert.That(res.Members[0].Name, Is.EqualTo("log4net.Appender.AdoNetAppender"));
            Assert.That(res.Members[0].Type, Is.EqualTo(MemberType.Type));
            Assert.That(res.Members[4].Name, Is.EqualTo("log4net.Appender.IAppender.Close"));
            Assert.That(res.Members[4].Type, Is.EqualTo(MemberType.Method));

            res = proc.ProcessXml(XmlTestDocs.NHibernate);

            Assert.That(res.Members.Count, Is.GreaterThan(0));
            Assert.That(res.Members[0].Name, Is.EqualTo("NHibernate.Action.BulkOperationCleanupAction"));
            Assert.That(res.Members[0].Type, Is.EqualTo(MemberType.Type));
        }


    }
}
