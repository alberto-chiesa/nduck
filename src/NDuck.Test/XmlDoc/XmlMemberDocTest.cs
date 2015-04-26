using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NDuck.XmlDoc
{
    /// <summary>
    /// Tests for the <see cref="NDuck.XmlDoc.XmlMemberDoc"/> class.
    /// </summary>
    [TestFixture]
    public class XmlMemberDocTest
    {
        /// <summary>
        /// Tests various error conditions.
        /// </summary>
        [Test]
        public void TestNamingValidation()
        {
            var m = new XmlMemberDoc();
            Assert.Throws<ArgumentNullException>(() => m.ReadNameAndType(null));
            Assert.Throws<ArgumentNullException>(() => m.ReadNameAndType(""));
            Assert.Throws<ArgumentException>(() => m.ReadNameAndType("T:"));
            Assert.Throws<ArgumentException>(() => m.ReadNameAndType("Tlongstringwithout:"));
            Assert.Throws<InvalidOperationException>(() => m.ReadNameAndType("A:Validnamewithinvalidtype"));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [TestCase("M:Namespace.TypeName.ToString(System.String)", "<Method>Namespace.TypeName.ToString(System.String)")]
        [TestCase("T:Namespace.SubNs.Type<T>", "<Type>Namespace.SubNs.Type<T>")]
        [TestCase("F:Whatever", "<Field>Whatever")]
        [TestCase("P:Namespace.TypeName.Prop", "<Property>Namespace.TypeName.Prop")]
        public void TestToString(String xmlName, String expectedToString)
        {
            var m = new XmlMemberDoc();
            m.ReadNameAndType(xmlName);
            Assert.That(m.ToString(), Is.EqualTo(expectedToString));
        }
    }
}
