using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using PositionalFileReader.Test.Models;
using System.IO;
using PositionalFileReader.Test.Utils;

namespace PositionalFileReader.Test
{
    [TestClass]
    public class UniformFileReaderTest
    {

		private IFileReader fileReader;

        private List<PokeTrainer> GetAllTrainers(){
            var trainers = new List<PokeTrainer>();
            trainers.Add(new PokeTrainer { Id = 1, Name = "Ash Ketchum", Enrollment = new DateTime(2017, 11, 20) });
            trainers.Add(new PokeTrainer { Id = 2, Name = "Brock", Enrollment = new DateTime(2017, 11, 21) });
            trainers.Add(new PokeTrainer { Id = 3, Name = "Red", Enrollment = new DateTime(2017, 11, 21) });
            trainers.Add(new PokeTrainer { Id = 4, Name = "May", Enrollment = new DateTime(2018, 1, 2) });
            trainers.Add(new PokeTrainer { Id = 5, Name = "Max", Enrollment = new DateTime(2018, 2, 3) });
            trainers.Add(new PokeTrainer { Id = 6, Name = "Dawn", Enrollment = new DateTime(2018, 2, 2) });
            trainers.Add(new PokeTrainer { Id = 7, Name = "Misty", Enrollment = new DateTime(2017, 3, 25) });
            return trainers;
        }

        private List<PokeTrainerWithOptionalValue> GetAllTrainersWithOptionalValue()
        {
            var trainers = new List<PokeTrainerWithOptionalValue>();
            trainers.Add(new PokeTrainerWithOptionalValue { Id = 1, Name = "Ash Ketchum", Enrollment = new DateTime(2017, 11, 20), Observation = null  });
            trainers.Add(new PokeTrainerWithOptionalValue { Id = 2, Name = "Brock", Enrollment = new DateTime(2017, 11, 21), Observation = null  });
            trainers.Add(new PokeTrainerWithOptionalValue { Id = 3, Name = "Red", Enrollment = new DateTime(2017, 11, 21), Observation = "This is the best of the best." });
            trainers.Add(new PokeTrainerWithOptionalValue { Id = 4, Name = "May", Enrollment = new DateTime(2018, 1, 2), Observation = null  });
            trainers.Add(new PokeTrainerWithOptionalValue { Id = 5, Name = "Max", Enrollment = new DateTime(2018, 2, 3), Observation = null  });
            trainers.Add(new PokeTrainerWithOptionalValue { Id = 6, Name = "Dawn", Enrollment = new DateTime(2018, 2, 2), Observation = null  });
            trainers.Add(new PokeTrainerWithOptionalValue { Id = 7, Name = "Misty", Enrollment = new DateTime(2017, 3, 25), Observation = null  });
            return trainers;
        }

        private List<PokeTrainerCustomConverter> GetAllTrainersCustomConverter()
        {
            var trainers = new List<PokeTrainerCustomConverter>();
            trainers.Add(new PokeTrainerCustomConverter {  Birthday = new DateTime(2018, 1, 2) });
            trainers.Add(new PokeTrainerCustomConverter {  Birthday = new DateTime(2018, 12, 12) });
            return trainers;
        }

        private List<PokeTrainerWithDataLengthSmallerThanMaxLineLength> GetAllTrainersWithDataLengthSmallerThanMaxLineLength() 
        {
            var trainers = new List<PokeTrainerWithDataLengthSmallerThanMaxLineLength>();
            trainers.Add(new PokeTrainerWithDataLengthSmallerThanMaxLineLength { Id = 1, Name = "Ash Ketchum" });
            return trainers;
        }

		[TestInitialize]
		public void SetUp(){
			this.fileReader = new FileReader();
		}

        [TestMethod]
		public void ReadFromFile_WhenCalled_ReturnDeserializedPokeTrainers()
        {
			var path = FileUtils.GetFullPath(FileUtils.UniformFileReaderFolder, FileUtils.OkFile);
            var errors = new List<string>();
            var deserializedPokeTrainers = fileReader.ReadFromFile<PokeTrainer>(path);
            var pokeTrainers = this.GetAllTrainers();
            Assert.IsTrue(deserializedPokeTrainers.Count == pokeTrainers.Count);
            for (var i = 0; i < deserializedPokeTrainers.Count; i++){
                var currentDeserializedPokeTrainer = deserializedPokeTrainers[i];
                var currentPokeTrainer = pokeTrainers[i];
                Assert.IsTrue(currentDeserializedPokeTrainer.Id == currentPokeTrainer.Id);
                Assert.IsTrue(currentDeserializedPokeTrainer.Name.Trim() == currentPokeTrainer.Name);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Day == currentPokeTrainer.Enrollment.Day);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Month == currentPokeTrainer.Enrollment.Month);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Year == currentPokeTrainer.Enrollment.Year);
            }
        }

        [TestMethod]
		public void ReadFromFile_WhenCustomConverterIsUsed_ReturnDeserializedPokeTrainers()
        {
			var path = FileUtils.GetFullPath(FileUtils.UniformFileReaderFolder, FileUtils.CustomConverterFile);
            var errors = new List<string>();
            var deserializedPokeTrainers = fileReader.ReadFromFile<PokeTrainerCustomConverter>(path);
            var pokeTrainers = this.GetAllTrainersCustomConverter();
            Assert.IsTrue(deserializedPokeTrainers.Count == pokeTrainers.Count);
            for (var i = 0; i < deserializedPokeTrainers.Count; i++)
            {
                var currentDeserializedPokeTrainer = deserializedPokeTrainers[i];
                var currentPokeTrainer = pokeTrainers[i];
                Assert.IsTrue(currentDeserializedPokeTrainer.Birthday.Day == currentPokeTrainer.Birthday.Day);
                Assert.IsTrue(currentDeserializedPokeTrainer.Birthday.Month == currentPokeTrainer.Birthday.Month);
                Assert.IsTrue(currentDeserializedPokeTrainer.Birthday.Year == currentPokeTrainer.Birthday.Year);
            }
        }

        [TestMethod]
		public void ReadFromFile_WhenOptionalValueIsUsed_ReturnDeserializedPokeTrainers()
        {
			var path = FileUtils.GetFullPath(FileUtils.UniformFileReaderFolder, FileUtils.PokeTrainerCustomWithOptionalValueFile);
            var errors = new List<string>();
            var deserializedPokeTrainers = fileReader.ReadFromFile<PokeTrainerWithOptionalValue>(path);
            var pokeTrainers = this.GetAllTrainersWithOptionalValue();
            Assert.IsTrue(deserializedPokeTrainers.Count == pokeTrainers.Count);
            for (var i = 0; i < deserializedPokeTrainers.Count; i++)
            {
                var currentDeserializedPokeTrainer = deserializedPokeTrainers[i];
                var currentPokeTrainer = pokeTrainers[i];
                Assert.IsTrue(currentDeserializedPokeTrainer.Id == currentPokeTrainer.Id);
                Assert.IsTrue(currentDeserializedPokeTrainer.Name.Trim() == currentPokeTrainer.Name);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Day == currentPokeTrainer.Enrollment.Day);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Month == currentPokeTrainer.Enrollment.Month);
                Assert.IsTrue(currentDeserializedPokeTrainer.Enrollment.Year == currentPokeTrainer.Enrollment.Year);
                Assert.IsTrue(currentDeserializedPokeTrainer.Observation == currentPokeTrainer.Observation);
            }
        }

        [TestMethod]
		public void  ReadFromFile_WhenDataLengthSmallerThanDataLength_ReturnDeserializedPokeTrainers()
        {
			var path = FileUtils.GetFullPath(FileUtils.UniformFileReaderFolder, FileUtils.PokeTrainerWithDataLengthSmallerThanMaxLineLengthFile);
            var errors = new List<string>();
            var deserializedPokeTrainers = fileReader.ReadFromFile<PokeTrainerWithDataLengthSmallerThanMaxLineLength>(path);
            var pokeTrainers = this.GetAllTrainersWithDataLengthSmallerThanMaxLineLength();
            Assert.IsTrue(deserializedPokeTrainers.Count == pokeTrainers.Count);
            for (var i = 0; i < deserializedPokeTrainers.Count; i++)
            {
                var currentDeserializedPokeTrainer = deserializedPokeTrainers[i];
                var currentPokeTrainer = pokeTrainers[i];
                Assert.IsTrue(currentDeserializedPokeTrainer.Id == currentPokeTrainer.Id);
                Assert.IsTrue(currentDeserializedPokeTrainer.Name.Trim() == currentPokeTrainer.Name);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(DeserializationException))]
		public void ReadFromFile_WhenCalled_ThrowDeserializationException()
        {
			var path = FileUtils.GetFullPath(FileUtils.UniformFileReaderFolder, FileUtils.MalFormed);
            var errors = new List<string>();
            var deserializedPokeTrainers = fileReader.ReadFromFile<PokeTrainer>(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
		public void ReadFromFile_WhenCalled_ThrowFile_NotFoundException()
        {
			var path = FileUtils.GetFullPath(FileUtils.UniformFileReaderFolder, "Not_Found.txt");
            var errors = new List<string>();
            var deserializedPokeTrainers = fileReader.ReadFromFile<PokeTrainer>(path);
        }
    }
}
