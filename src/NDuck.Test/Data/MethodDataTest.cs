using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Moq;
using NDuck.Data.Enum;
using NUnit.Framework;

namespace NDuck.Data
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class MethodDataTest
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestConstructorValidation()
        {
            Assert.Throws<ArgumentNullException>(() => new MethodData(null));
        }

        // Cannot Test because of sealed MethodDefinition.
        //[Test]
        //public void ReadAccessorReturnsInvalidForInvalidConfiguration()
        //{
        //    var defMock = new Mock<MethodDefinition>();
        //    defMock.SetupProperty(m => m.IsPublic, false);
        //    defMock.SetupProperty(m => m.IsPrivate, false);
        //    defMock.SetupProperty(m => m.IsFamily, false);
        //    defMock.SetupProperty(m => m.IsFamilyOrAssembly, false);
        //    defMock.SetupProperty(m => m.IsAssembly, false);

        //    Assert.That(MethodData.ReadAccessor(defMock.Object), Is.EqualTo(AccessorType.Invalid));
        //}
    }
}
