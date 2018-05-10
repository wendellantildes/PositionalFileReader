using System;
namespace PositionalFileReader
{
    public class DeserializationException :Exception
    {
        public DeserializationException(string message, Exception innerException) : base(message,innerException)
        {
        }
    }
}
