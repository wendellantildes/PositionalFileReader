using System;
namespace Kernel
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
        public string CustomUndoMethod { get; set; } = null;
        public char LeftPadding { get; set; } = '\0';
    }
}
