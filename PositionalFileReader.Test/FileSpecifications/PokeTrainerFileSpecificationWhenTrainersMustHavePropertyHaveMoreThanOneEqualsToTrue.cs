﻿using System;
using System.Collections.Generic;

namespace PositionalFileReader.Test.FileSpecifications
{
    public class PokeTrainerFileSpecificationWhenTrainersMustHavePropertyHaveMoreThanOneEqualsToTrue
    {
        [LineSpecification(TestMethod = nameof(IsHeader), Any = typeof(PokeTrainerHeader))]
        public PokeTrainerHeader Header { get; set; }

        [LineSpecification(TestMethod = nameof(IsCandidate), Any = typeof(PokeTrainer))]
		public List<PokeTrainer> Trainers { get; set; } = new List<PokeTrainer>();

        [LineSpecification(TestMethod = nameof(IsTrailer), Any = typeof(PokeTrainerTrailer))]
        public PokeTrainerTrailer Trailer { get; set; }

        public bool IsHeader(string line)
        {
            return line.Substring(0, 1) == "H";
        }

        public bool IsCandidate(string line)
        {
            var id = line.Substring(0, 3);
            int result;
            return Int32.TryParse(id, out result);
        }

        public bool IsTrailer(string line)
        {
            return line.Substring(0, 1) == "T";
        }
    }
}
