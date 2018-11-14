using System;
namespace Kernel
{
    public class DuplicatedDataAttributeException : Exception
    {
        public DuplicatedDataAttributeException(string message) :base(message)
        {
        }
    }
}
