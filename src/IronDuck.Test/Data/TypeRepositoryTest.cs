using NUnit.Framework;

namespace IronDuck.Data
{
    [TestFixture]
    public class TypeRepositoryTest
    {
        [Test]
        public void Test()
        {
            var repo = new TypeRepository();

            repo.LoadAssemblyTypes(@".\IronDuck.exe");

            var dataNs = repo.Namespaces["IronDuck"].SubNamespaces["Data"];
            Assert.That(dataNs.Name, Is.EqualTo("Data"));
            Assert.That(dataNs.Fullname, Is.EqualTo("IronDuck.Data"));
            Assert.That(dataNs.Types.ContainsKey("TypeRepository"));
            Assert.That(dataNs.Types["TypeRepository"].AssemblyName, Is.EqualTo("IronDuck"));
        }


    }
}
