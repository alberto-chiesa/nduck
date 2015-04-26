using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NDuck.Config
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ExecutionOptionsTest
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestDeserialization()
        {
            var json = TestData.XmlTestDocs.TestOptions;
            var opts = ExecutionOptions.ReadFromJson(json);

            Assert.That(opts.ContentDirectory, Is.EqualTo("./content"));
            Assert.That(opts.Verbosity, Is.EqualTo(Logger.OutputLevel.Debug));
            CollectionAssert.AreEqual(new[] {"Assembly1", "Assembly2"}, opts.Assemblies);
        }
    }
}
