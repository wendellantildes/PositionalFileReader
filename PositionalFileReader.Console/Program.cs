using System;
using System.Collections.Generic;
using PositionalFileWriter;

namespace PositionalFileReader.Console
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    try{
        //        var path = System.IO.Path.GetFullPath(@"Files/");
        //        var errors = new List<string>();
        //        var candidates = new DirectoryReader().ReadFromDirectory<Candidate>(path, ref errors);
        //        foreach(var candidate in candidates){
        //            System.Console.WriteLine($"{candidate.Id} {candidate.Name} {candidate.Enrollment}");
        //        }
        //        if(errors.Count > 0){
        //            foreach (var error in errors)
        //            {
        //                System.Console.WriteLine($"{error}");
        //            }
        //        }
        //    }catch(Exception ex){
        //        System.Console.WriteLine(ex.Message);
        //    }

        //    try
        //    {
        //        var path = System.IO.Path.GetFullPath(@"Files/000.txt");
        //        var errors = new List<string>();
        //        var candidateSpecification = new FileReader().ReadFromFileWithSpecification<CandidateFileSpecification>(path);
        //        foreach (var candidate in candidateSpecification.Candidates)
        //        {
        //            System.Console.WriteLine($"{candidate.Id} {candidate.Name} {candidate.Enrollment}");
        //        }
        //        System.Console.WriteLine($"{candidateSpecification.Header.Id}");
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Console.WriteLine(ex.Message);
        //    }

        //}

        static void Main(string[] args)
        {
            var ash = new Candidate();
            ash.Id = 1;
            ash.Enrollment = DateTime.Parse("2017-11-20");
            ash.Name = "Ash Ketchum";
            var line = new FileWriter().Serialize(ash);
            System.Console.WriteLine(line);
            System.Console.WriteLine("001Ash Ketchum     20171120");

            var brock = new Candidate();
            brock.Id = 2;
            brock.Enrollment = DateTime.Parse("2017-11-20");
            brock.Name = "Brock";

            var misty = new Candidate();
            misty.Id = 2;
            misty.Enrollment = DateTime.Parse("2017-11-20");
            misty.Name = "Misty";

            var header = new CandidateHeader();
            header.Id = "H";
            var specification = new CandidateFileSpecification();
            specification.Candidates = new List<Candidate> { ash, brock, misty };
            specification.Header = header;

            System.IO.File.WriteAllText("resultado.txt",new FileWriter().SerializeFromSpecification(specification));


            System.Console.ReadLine();
        }
    }

     
}
