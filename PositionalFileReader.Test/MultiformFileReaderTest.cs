using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PositionalFileReader.Test.FileSpecifications;
using PositionalFileReader.Test.Utils;

namespace PositionalFileReader.Test
{
    [TestClass]
    public class MultiformFileReaderTest
    {
		private IFileReader fileReader;
        
        private List<PokeTrainer> GetAllTrainers()
        {
            var trainers = new List<PokeTrainer>();
            trainers.Add(new PokeTrainer { Id = 1, Name = "Ash Ketchum", Enrollment = new DateTime(2017, 11, 20) });
            trainers.Add(new PokeTrainer { Id = 2, Name = "Brock", Enrollment = new DateTime(2017, 11, 20) });
            trainers.Add(new PokeTrainer { Id = 3, Name = "Misty", Enrollment = new DateTime(2017, 11, 20) });
            return trainers;
        }

        private PokeTrainerHeader GetPokeTrainerHeader()
        {
            return new PokeTrainerHeader { EvidencialHeader = "H", FileDate = new DateTime(2017, 02, 01) };
        }

        private PokeTrainerTrailer GetPokeTrainerTrailer()
        {
            return new PokeTrainerTrailer { EvidencialTrailer = "T", NumberOfLines = 5 };
        }

		[TestInitialize]
        public void SetUp()
        {
            this.fileReader = new FileReader();
        }

        [TestMethod]
		public void ReadFromFileWithSpecification_WhenCalled_ReturnDeserializedPokeTrainers()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, FileUtils.PokeTrainerWithHeaderAndTrailerFile);
            var errors = new List<string>();
            var deserializedPokeTrainerFile = fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecification>(path);
            var pokeTrainers = this.GetAllTrainers();
            Assert.IsTrue(deserializedPokeTrainerFile.Trainers.Count == pokeTrainers.Count);
            for (var i = 0; i < deserializedPokeTrainerFile.Trainers.Count; i++)
            {
                var currentDeserializedPokeTrainer = deserializedPokeTrainerFile.Trainers[i];
                var currentPokeTrainer = pokeTrainers[i];
                Assert.IsTrue(currentDeserializedPokeTrainer.Id == currentPokeTrainer.Id);
                Assert.IsTrue(currentDeserializedPokeTrainer.Name.Trim() == currentPokeTrainer.Name);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Day == currentPokeTrainer.Enrollment.Day);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Month == currentPokeTrainer.Enrollment.Month);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Year == currentPokeTrainer.Enrollment.Year);
            }
            var header = this.GetPokeTrainerHeader();
            Assert.IsTrue(deserializedPokeTrainerFile.Header.EvidencialHeader == header.EvidencialHeader);
            Assert.IsTrue(deserializedPokeTrainerFile.Header.FileDate == header.FileDate);
            var trailer = this.GetPokeTrainerTrailer();
            Assert.IsTrue(deserializedPokeTrainerFile.Trailer.EvidencialTrailer == trailer.EvidencialTrailer);
            Assert.IsTrue(deserializedPokeTrainerFile.Trailer.NumberOfLines == trailer.NumberOfLines);
        }

        [TestMethod]
        [ExpectedException(typeof(DeserializationException))]
		public void ReadFromFileWithSpecification_WhenHeaderIsMalformed_ReturnErrors()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, FileUtils.PokeTrainerWithHeaderAndTrailerWhenHeaderIsMalformedFile);
            var errors = new List<string>();
            fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecification>(path);
        }
        
        [TestMethod]
        [ExpectedException(typeof(DeserializationException))]
		public void ReadFromFileWithSpecification_WhenOneTrainerIsMalformed_ReturnErrors()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, FileUtils.PokeTrainerWithHeaderAndTrailerWhernOneTrainerIsMalformedFile);
            var errors = new List<string>();
            fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecification>(path);
        }

        [TestMethod]
        [ExpectedException(typeof(LineSpecificationException))]
		public void ReadFromFileWithSpecification_WhenTrainerMustBeAList_ReturnErrors()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, FileUtils.PokeTrainerWithHeaderAndTrailerFile);
            var errors = new List<string>();
            fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecificationWhenTrainerMustBeAList>(path);
        }

        [TestMethod]
		public void  ReadFromFileWithSpecification_WhenTrainerMustBeAList_ReturnSpecificError()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, FileUtils.PokeTrainerWithHeaderAndTrailerFile);
            var errors = new List<string>();
            try{
                fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecificationWhenTrainerMustBeAList>(path);
            }catch(LineSpecificationException ex){
                var errorMessage = $"Specification has set attribute HasMoreThanOne = true, but the property Trainers is not a list.";
                Assert.IsTrue(ex.Message == errorMessage);
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(LineSpecificationException))]
		public void  ReadFromFileWithSpecification_WhenTrainersMustHavePropertyHasMoreThanOneEqualsToTrue_ThrowLineSpecificationException()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, FileUtils.PokeTrainerWithHeaderAndTrailerFile);
            var errors = new List<string>();
            fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecificationWhenTrainersMustHavePropertyHaveMoreThanOneEqualsToTrue>(path);
        }

        [TestMethod]
		public void ReadFromFileWithSpecification_WhenTrainersMustHavePropertyHasMoreThanOneEqualsToTrue_ReturnSpecificError()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, FileUtils.PokeTrainerWithHeaderAndTrailerFile);
            var errors = new List<string>();
            try
            {
                fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecificationWhenTrainersMustHavePropertyHaveMoreThanOneEqualsToTrue>(path);
            }
            catch (LineSpecificationException ex)
            {
                var errorMessage = $"Specification has set attribute HasMoreThanOne = false, but the property Trainers is a list.";
                Assert.IsTrue(ex.Message == errorMessage);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
		public void ReadFromFileWithSpecification_WhenFileNotExists_ThrowFileNotFoundException()
        {
			var path = FileUtils.GetFullPath(FileUtils.MultiformFileReaderFolder, "Not_Found.txt");
            var errors = new List<string>();
            var deserializedPokeTrainers = fileReader.ReadFromFileWithSpecification<PokeTrainerFileSpecification>(path);
        }
    }
}
