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
        [TestCase("NDuck.TestClasses.PublicType1", AccessorType.Public, ClassType.Class)]
        [TestCase("NDuck.TestClasses.IPublic1", AccessorType.Public, ClassType.Interface)]
        [TestCase("NDuck.TestClasses.InternalType1`2", AccessorType.Internal, ClassType.Class)]
        [TestCase("NDuck.TestClasses.InternalType1`2.PrivateClass`1", AccessorType.Private, ClassType.Class)]
        [TestCase("NDuck.TestClasses.InternalType1`2.InternalEnum", AccessorType.Internal, ClassType.Enum)]
        [TestCase("NDuck.TestClasses.Enum1", AccessorType.Public, ClassType.Enum)]
        [TestCase("NDuck.TestClasses.Struct1", AccessorType.Internal, ClassType.Struct)]
        public void TestTypeNaming(String typeName, AccessorType accessor, ClassType classType)
        {
            var typeData =_selfRepo.GetTypeData(typeName);
            
            Assert.That(typeData, Is.Not.Null);
            Assert.That(typeData.Accessor, Is.EqualTo(accessor));
            Assert.That(typeData.ClassType, Is.EqualTo(classType));
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
        [TestCase("PublicMethod", AccessorType.Public, 89)]
        [TestCase("InternalMethod", AccessorType.Internal, null)]
        [TestCase("ProtectedInternalMethod", AccessorType.ProtectedInternal, null)]
        [TestCase("ProtectedMethod", AccessorType.Protected, null)]
        [TestCase("PrivateMethod", AccessorType.Private, null)]
        [TestCase("PrivateMethod2", AccessorType.Private, 123)]
        public void TestMethodsMetadata(string methodName, AccessorType accessor, int? startLine)
        {
            var pubType1 = _selfRepo.GetTypeData("NDuck.TestClasses.PublicType1");

            var method = pubType1.Methods.First(m => m.Name == methodName);
            
            Assert.That(method.Accessor, Is.EqualTo(accessor));
         
            Assert.That(method.IsConstructor, Is.False);

            Assert.That(method.Reference.FilePath.EndsWith(@"TestClasses\PublicType1.cs"));

            if (startLine != null)
                Assert.That(method.Reference.StartLine, Is.EqualTo(startLine.Value));
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
