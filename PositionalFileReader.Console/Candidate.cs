﻿using System;
namespace PositionalFileReader.Console
{
    public class CandidateHeader{
        [Data(StartIndex = 0,Length = 10)]
        public string  Id { get; set; } 
    }

    public class Candidate
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
}
