using System;
namespace PositionalFileReader
{
    public class LineSpecificationException : Exception
    {
        public LineSpecificationException(string message) : base(message)
        {
        }
    }
}
