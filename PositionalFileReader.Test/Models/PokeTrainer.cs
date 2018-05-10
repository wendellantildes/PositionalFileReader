using System;
namespace PositionalFileReader.Test
{
    public class PokeTrainerHeader{
        [Data(StartIndex = 0,Length = 1)]
        public string EvidencialHeader { get; set; } 

        [Data(StartIndex = 2,Length = 8, CustomConverterMethod = nameof(Convert))]
        public DateTime FileDate { get; set; } 

        public DateTime Convert(string date)
        {
            return DateTime.ParseExact(date, "yyyyMMdd", null);
        }
    }

    public class PokeTrainer
    {
        [Data(StartIndex = 0,Length = 3, DataType = typeof(int))]
        public int Id { get; set; } 

        [Data(StartIndex = 3, Length = 15)]
        public string Name { get; set; }

        [Data(StartIndex = 19, Length = 8, CustomConverterMethod = nameof(Convert))]
        public DateTime Enrollment { get; set; }

        public DateTime Convert(string date){
            return DateTime.ParseExact(date, "yyyyMMdd", null);
        }
    }

    public class PokeTrainerTrailer
    {
        [Data(StartIndex = 0, Length = 1)]
        public string EvidencialTrailer { get; set; }

        [Data(StartIndex = 19, Length = 8, DataType = typeof(long))]
        public long NumberOfLines { get; set; }
    }

}
