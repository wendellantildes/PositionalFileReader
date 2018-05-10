using System;
namespace PositionalFileReader
{
    [AttributeUsage(AttributeTargets.Property,
                AllowMultiple = false,
                Inherited = true)]
    public class DataAttribute : Attribute
    {
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public Type DataType { get; set; } = typeof(string);
        public bool Optional { get; set; } = false;
        public string CustomConverterMethod { get; set; } = null;

    }
}
