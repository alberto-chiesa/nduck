using System;
using System.Collections.Generic;
using System.Linq;

namespace NDuck.TestClasses
{
    /// <summary>
    /// This class exists only to define a complete set of
    /// all the comments defined. As an example, here I am
    /// using a <c>&lt;c&gt; tag</c> to represent some code.
    /// </summary>
    /// <example>
    /// As an example, some c# code snippet:
    /// <code>public void SomeMethod()
    /// {
    ///     A = B * C;
    /// }</code>
    /// </example>
    /// <remarks>
    /// Including also a remarks section, used to show the use of paragraphs.
    /// <para>
    ///   This text should be represented in a separate paragraph (and be deindented).
    /// </para>
    /// <para>
    /// I can include also a &lt;see&gt; tag in order to reference a type:
    /// <see cref="NDuck.TestClasses.Enum1"/>
    /// </para>
    /// </remarks>
    class CommentsExamples
    {
        /// <summary>
        /// Just a property. For this small property, we are also including a list of points.
        /// A list can contain three type of points:
        /// <list type="bullet">
        ///   <listheader>
        ///     <term>Point Type</term>
        ///     <description>Description</description>
        ///   </listheader>
        ///   <item>
        ///     <term>bullet</term>
        ///     <description>Bullet defines a bulleted list. You can skip headers and term definitions.</description>
        ///   </item>
        ///   <item>
        ///     <term>number</term>
        ///     <description>Number defines a numbered list. You can skip headers and term definitions.</description>
        ///   </item>
        ///   <item>
        ///     <term>table</term>
        ///     <description>Table defines a table of rows. If you provide both term and description, the
        ///     table will be a bulleted list. You can skip headers and term definitions.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <value>A value description.</value>
        public Int32 AnInt { get; set; }

        /// <summary>
        /// This method let us experiment with param tags.
        /// </summary>
        /// <param name="par1">A simple param tag used to describe the par1 parameter.</param>
        /// <param name="par2">
        /// The par2 parameter is not like the <paramref name="par1"/> parameter, because
        /// his comment is much nicer, even if it is just another <see cref="System.String"/>.
        /// </param>
        /// <example>
        /// As an example, some c# code snippet:
        /// <code>
        /// 
        ///   public void SomeMethod()
        ///   {
        ///     A = B * C;
        ///   }
        /// </code>
        /// </example>
        public void Method1(string par1, string par2)
        {
            
        }
    }
}
