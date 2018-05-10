using System;
namespace PositionalFileReader.Test.Models
{
    public class PokeTrainerCustomConverter
    {
        [Data(StartIndex = 0, Length = 8, CustomConverterMethod = nameof(Convert))]
        public DateTime Birthday { get; set; }

        public DateTime Convert(string date)
        {
            return DateTime.ParseExact(date, "yyyyMMdd", null);
        }
    }
}
