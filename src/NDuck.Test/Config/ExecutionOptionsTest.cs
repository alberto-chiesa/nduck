using System;
using System.Collections.Generic;
using System.Linq;
using NDuck.TestData;
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
            var json = XmlTestDocs.TestOptions;
            var opts = ExecutionOptions.ReadFromJson(json);

            Assert.That(opts.ContentDirectory, Is.EqualTo("./content"));
            Assert.That(opts.Verbosity, Is.EqualTo(Logger.OutputLevel.Debug));
            CollectionAssert.AreEqual(new[] {"Assembly1", "Assembly2"}, opts.Assemblies);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [TestCase(new[] { "-v" }, Logger.OutputLevel.Debug)]
        [TestCase(new[] { "--v" }, Logger.OutputLevel.Debug)]
        [TestCase(new[] { "/verbose" }, Logger.OutputLevel.Debug)]
        [TestCase(new[] { "-in=Test1" }, Logger.OutputLevel.Warning)]
        [TestCase(new[] { "-in=Test1", "/silent" }, Logger.OutputLevel.Error)]
        [TestCase(new[] { "-v", "/silent" }, Logger.OutputLevel.Error)]
        [TestCase(new[] { "-v", "-svsv" }, Logger.OutputLevel.Debug)]
        public void TestVerbosity(String[] args, Logger.OutputLevel expectedLevel)
        {
            //String[] args = {"-v"};

            var opts = new ExecutionOptions(args);

            Assert.That(opts.Verbosity, Is.EqualTo(expectedLevel));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestArgsForInit()
        {
            String[] args = { "-v" , "-in", "Test1", "-in", "Test2", "iNIt", "-out=out", "-content", "contentDir", "-p=duckduck.json"};

            var opts = new ExecutionOptions(args);

            Assert.That(opts.Verbosity, Is.EqualTo(Logger.OutputLevel.Debug));
            CollectionAssert.AreEqual(new[] {"Test1", "Test2"}, opts.Assemblies);
            Assert.That(opts.OutputPath, Is.EqualTo("out"));
            Assert.That(opts.ContentDirectory, Is.EqualTo("contentDir"));
            Assert.That(opts.ConfigurationFile, Is.EqualTo("duckduck.json"));
            Assert.That(opts.Command, Is.EqualTo(ExecutionOptions.ExecutionCommand.Init));
        }
    }
}
