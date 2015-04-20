using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using NUnit.Framework;

namespace NDuck.Data
{
    /// <summary>
    /// Tests for the TypeData class.
    /// </summary>
    [TestFixture]
    public class TypeDataTest
    {
        /// <summary>
        /// Tests for the GetFullName method in the
        /// TypeData class.
        /// </summary>
        [Test]
        [TestCase(typeof(List<String>), "System.Collections.Generic.List<System.String>")]
        [TestCase(typeof(NDuck.TestClasses.InternalType1<,>), "NDuck.TestClasses.InternalType1`2")]
        [TestCase(typeof(NDuck.TestClasses.InternalType1<String,List<String>>),
            "NDuck.TestClasses.InternalType1<System.String,System.Collections.Generic.List<System.String>>")]
        [TestCase(typeof(NDuck.TestClasses.InternalType1<String, List<Object>>),
            "NDuck.TestClasses.InternalType1<System.String,System.Collections.Generic.List<System.Object>>")]
        public void TestNaming(Type type, String expectedTypeName)
        {
            var module = ModuleDefinition.CreateModule("test", ModuleKind.Dll);
            var typeReference = module.Import(type);

            var nameOfType = TypeData.GetFullName(typeReference);

            Assert.That(nameOfType, Is.EqualTo(expectedTypeName));
        }

    }
}
