using System;
namespace PositionalFileReader
{
    public class DuplicatedDataAttributeException : Exception
    {
        public DuplicatedDataAttributeException(string message) :base(message)
        {
        }
    }
}
