using System;
using Kernel;

namespace PositionalFileReader.Test.Models
{
    public class PokeTrainerWithDataLengthSmallerThanMaxLineLength
    {
        [Data(StartIndex = 0, Length = 3, DataType = typeof(int))]
        public int Id { get; set; }

        [Data(StartIndex = 3, Length = 100)]
        public string Name { get; set; }
    }
}
