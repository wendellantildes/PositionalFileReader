using System;
using System.Collections.Generic;

namespace PositionalFileReader.Console
{
    public class CandidateFileSpecification
    {
        [LineSpecification(TestMethod = nameof(IsHeader), Any = typeof(CandidateHeader))]
        public CandidateHeader Header { get; set; }

        [LineSpecification(TestMethod = nameof(IsCandidate), Any = typeof(Candidate), HasMoreThanOne = true)]
        public List<Candidate> Candidates { get; set; } = new List<Candidate>();

        public bool IsHeader(string line){
            return line.Substring(0, 1) == "H";
        }

        public bool IsCandidate(string line)
        {
            var id = line.Substring(0, 3);
            int result;
            return Int32.TryParse(id, out result);
        }
    }
}
