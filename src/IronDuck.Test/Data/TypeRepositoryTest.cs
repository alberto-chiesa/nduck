using NUnit.Framework;

namespace NDuck.Data
{
    [TestFixture]
    public class TypeRepositoryTest
    {
        [Test]
        public void Test()
        {
            var repo = new TypeRepository();

            repo.LoadAssemblyTypes(@".\NDuck.exe");

            var dataNs = repo.Namespaces["NDuck"].SubNamespaces["Data"];
            Assert.That(dataNs.Name, Is.EqualTo("Data"));
            Assert.That(dataNs.Fullname, Is.EqualTo("NDuck.Data"));
            Assert.That(dataNs.Types.ContainsKey("TypeRepository"));
            Assert.That(dataNs.Types["TypeRepository"].AssemblyName, Is.EqualTo("NDuck"));
        }


    }
}
