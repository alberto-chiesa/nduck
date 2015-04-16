using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDuck.TestClasses
{
    public class PublicType1
    {
        public PublicType1()
        { }

        internal PublicType1(String s)
        { }

        PublicType1(Int16 i)
        { }

        protected PublicType1(List<String> l)
        { }

        protected internal PublicType1(Int32 i)
        { }

        public void PublicMethod()
        {
            
        }

        protected internal void ProtectedInternalMethod()
        {
            
        }

        protected List<String> ProtectedMethod()
        {
            return null;
        }

        internal PublicType1 InternalMethod()
        {
            return null;
        }

        void PrivateMethod2()
        {

        }

        private String PrivateMethod()
        {
            return String.Empty;
        }

    }
}
