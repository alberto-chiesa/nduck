using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDuck.Data;
using NDuck.Output;
using NDuck.TestData;
using NUnit.Framework;

namespace NDuck.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class XmlProcessorTest
    {
        /// <summary>
        /// 
        /// </summary>
        public XmlProcessor Proc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocumentation DocLog4Net { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocumentation DocNh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocumentation DocSelf { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [TestFixtureSetUp]
        public void SetupProcessor()
        {
            Proc = new XmlProcessor();
            DocLog4Net = Proc.ProcessXml(XmlTestDocs.log4net);
            DocNh = Proc.ProcessXml(XmlTestDocs.NHibernate);
            DocSelf = Proc.ProcessXml(File.ReadAllText(Path.Combine(".", "NDuck.Test.xml"), Encoding.UTF8));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestAssemblyNameReading()
        {
            Assert.That(DocLog4Net.AssemblyName, Is.EqualTo("log4net"));

            Assert.That(DocNh.AssemblyName, Is.EqualTo("NHibernate"));
        }

        /// <summary>
        /// 
        /// </summary>
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
    }
}
