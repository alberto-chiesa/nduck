using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDuck.TestClasses
{
    /// <summary>
    /// Public class used for reflection testing.
    /// </summary>
    public class PublicType1
    {
        /// <summary>
        /// 
        /// </summary>
        public String publicField;

        /// <summary>
        /// 
        /// </summary>
        protected internal String protectedInternalField;
        
        /// <summary>
        /// 
        /// </summary>
        internal String internalField;
        
        /// <summary>
        /// 
        /// </summary>
        protected String protectedField;
        
        /// <summary>
        /// 
        /// </summary>
        private String privateField;

        /// <summary>
        /// A public parameterless constructor
        /// </summary>
        public PublicType1()
        {
            privateField = protectedField = internalField = "a";
            //privateField
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="s">A string parameter.</param>
        internal PublicType1(String s)
        { }

        /// <summary>
        /// Private, implicit constructor.
        /// </summary>
        /// <param name="i">Int16 parameter</param>
        PublicType1(Int16 i)
        { }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="l">
        /// A strongly typed generic parameter.
        /// </param>
        protected PublicType1(List<String> l)
        { }

        /// <summary>
        /// Protected Internal constructor.
        /// </summary>
        /// <param name="i">int parameter.</param>
        protected internal PublicType1(Int32 i)
        { }

        /// <summary>
        /// Public action.
        /// </summary>
        public void PublicMethod()
        {
            
        }

        /// <summary>
        /// Protected internal method.
        /// </summary>
        protected internal void ProtectedInternalMethod()
        {
            
        }

        /// <summary>Testing one line summary comments.</summary>
        /// <returns>A Strongly typed <see cref="System.Collections.Generic.List{String}"/>.
        /// Testing the &lt;see> tag resolution.
        /// </returns>
        protected List<String> ProtectedMethod()
        {
            return null;
        }

        /// <summary>
        /// Internal method.
        /// </summary>
        /// <returns>A nice and useless null.</returns>
        internal PublicType1 InternalMethod()
        {
            return null;
        }

        /// <summary>
        /// A Private method.
        /// </summary>
        void PrivateMethod2()
        {

        }

        /// <summary>
        /// Anothe private method
        /// </summary>
        /// <returns>Returns <see cref="System.String.Empty"/>. Yay!</returns>
        private String PrivateMethod()
        {
            return String.Empty;
        }
        
    }
}
