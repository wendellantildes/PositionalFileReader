using System;
namespace PositionalFileReader
{
    public class LineSpecificationAttribute : Attribute
    {
        /// <summary>
        /// Type of class to Deserialize.
        /// </summary>
        /// <value>Any.</value>
        public Type Any { get; set; }

        /// <summary>
        /// Name of method to test which kind of line it is. The method receive a line and must return bool.
        /// </summary>
        /// <value>The test method.</value>
        public string TestMethod { get; set; }

        /// <summary>
        /// Indicate if there is more than one line to deserialize to the same Entity. 
        /// In the specification the property has to be a List if this property is true.
        /// </summary>
        /// <value><c>true</c> if has more than one; otherwise, <c>false</c>.</value>
        public bool HasMoreThanOne { get; set; } = false;



    }
}
