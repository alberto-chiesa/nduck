using System;
using System.IO;
using System.Linq;
using System.Text;
using NDuck.Data.Enum;
using NDuck.XmlDoc;
using NUnit.Framework;

namespace NDuck
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class TypeRepositoryTest
    {
        private TypeRepository _selfRepo;
        private TypeRepository _nduckRepo;

        /// <summary>
        /// 
        /// </summary>
        [TestFixtureSetUp]
        public void DoReflectionBeforeTests()
        {
            _selfRepo = new TypeRepository();
            _selfRepo.LoadAssemblyTypes(Path.Combine(".", "NDuck.Test.dll"));

            var proc = new XmlProcessor();
            var docSelf = proc.ProcessXml(File.ReadAllText(Path.Combine(".", "NDuck.Test.xml"), Encoding.UTF8));
            _selfRepo.LoadXmlDoc(docSelf);

            _nduckRepo = new TypeRepository();
            _nduckRepo.LoadAssemblyTypes(Path.Combine(".", "NDuck.exe"));


        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void SelfReflect()
        {
            Assert.That(_selfRepo.Namespaces.ContainsKey("NDuck"));
            Assert.That(_selfRepo.Namespaces["NDuck"].SubNamespaces.ContainsKey("TestData"));
            Assert.That(_selfRepo.Namespaces["NDuck"].SubNamespaces.ContainsKey("Data"));
            Assert.That(_selfRepo.Namespaces["NDuck"].SubNamespaces.ContainsKey("XmlDoc"));
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestTypeNaming()
        {
            var typeData =_selfRepo.GetTypeData("NDuck.TestClasses.InternalType1`2");

            Assert.That(typeData, Is.Not.Null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="expectedAccessor"></param>
        [Test]
        [TestCase("PublicMethod", AccessorType.Public)]
        [TestCase("InternalMethod", AccessorType.Internal)]
        [TestCase("ProtectedInternalMethod", AccessorType.ProtectedInternal)]
        [TestCase("ProtectedMethod", AccessorType.Protected)]
        [TestCase("PrivateMethod", AccessorType.Private)]
        [TestCase("PrivateMethod2", AccessorType.Private)]
        [TestCase("NDuck.TestClasses.PublicType1.#ctor", AccessorType.Public)]
        public void TestMethodAccessors(string methodName, AccessorType expectedAccessor)
        {
            var pubType1 = _selfRepo.GetTypeData("NDuck.TestClasses.PublicType1");

            Assert.That(pubType1.GetMethod(methodName).Accessor, Is.EqualTo(expectedAccessor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="expectedReturnType"></param>
        [Test]
        [TestCase("PublicMethod", "System.Void")]
        [TestCase("ProtectedMethod", "System.Collections.Generic.List`1<System.String>")]
        [TestCase("InternalMethod", "NDuck.TestClasses.PublicType1")]
        [TestCase("PrivateMethod", "System.String")]
        public void TestMethodReturnTypes(string methodName, string expectedReturnType)
        {
            var pubType1 = _selfRepo.GetTypeData("NDuck.TestClasses.PublicType1");

            Assert.That(pubType1.GetMethod(methodName).ReturnType, Is.EqualTo(expectedReturnType));
        }

        /// <summary>
        /// TestCtorAccessors
        /// </summary>
        [Test]
        public void TestCtorAccessors()
        {
            var pubType1 = _selfRepo.GetTypeData("NDuck.TestClasses.PublicType1");

            Assert.That(pubType1.Methods.Any(m => m.Name == "PublicMethod" && m.Accessor == AccessorType.Public));
            Assert.That(pubType1.Methods.Any(m => m.Name == "InternalMethod" && m.Accessor == AccessorType.Internal));
            Assert.That(pubType1.Methods.Any(m => m.Name == "ProtectedInternalMethod" && m.Accessor == AccessorType.ProtectedInternal));
            Assert.That(pubType1.Methods.Any(m => m.Name == "ProtectedMethod" && m.Accessor == AccessorType.Protected));
            Assert.That(pubType1.Methods.Any(m => m.Name == "PrivateMethod" && m.Accessor == AccessorType.Private));
            Assert.That(pubType1.Methods.Any(m => m.Name == "PrivateMethod2" && m.Accessor == AccessorType.Private));
        }

        /// <summary>
        /// TestNDuck
        /// </summary>
        [Test]
        public void TestNDuck()
        {
            var dataNs = _nduckRepo.Namespaces["NDuck"].SubNamespaces["Data"];
            Assert.That(!_nduckRepo.Namespaces["NDuck"].SubNamespaces.ContainsKey("TestData"));
            Assert.That(dataNs.Name, Is.EqualTo("Data"));
            Assert.That(dataNs.Fullname, Is.EqualTo("NDuck.Data"));
            Assert.That(_nduckRepo.Namespaces["NDuck"].Types.ContainsKey("TypeRepository"));
            Assert.That(_nduckRepo.Namespaces["NDuck"].Types["TypeRepository"].AssemblyName, Is.EqualTo("NDuck"));
        }


    }
}
