using System;
using System.Collections.Generic;

namespace PositionalFileReader.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                var path = System.IO.Path.GetFullPath(@"Files/");
                var errors = new List<string>();
                var candidates = new DirectoryReader().ReadFromDirectory<Candidate>(path, ref errors);
                foreach(var candidate in candidates){
                    System.Console.WriteLine($"{candidate.Id} {candidate.Name} {candidate.Enrollment}");
                }
                if(errors.Count > 0){
                    foreach (var error in errors)
                    {
                        System.Console.WriteLine($"{error}");
                    }
                }
            }catch(Exception ex){
                System.Console.WriteLine(ex.Message);
            }

            try
            {
                var path = System.IO.Path.GetFullPath(@"Files/000.txt");
                var errors = new List<string>();
                var candidateSpecification = new FileReader().ReadFromFileWithSpecification<CandidateFileSpecification>(path);
                foreach (var candidate in candidateSpecification.Candidates)
                {
                    System.Console.WriteLine($"{candidate.Id} {candidate.Name} {candidate.Enrollment}");
                }
                System.Console.WriteLine($"{candidateSpecification.Header.Id}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

        }
    }
}
