using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PositionalFileReader.Test.Utils;

namespace PositionalFileReader.Test
{
	[TestClass]
    public class UniformDirectoryReaderTest
    {

		private IDirectoryReader directoryReader;

		[TestInitialize]
        public void SetUp()
        {
			this.directoryReader = new DirectoryReader();
        }

		[TestMethod]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void ReadFromDirectory_WhenDirectoryNotFound_ThrowDirectoryNotFoundException()
        {
			var path = FileUtils.GetFullPath("DirectoryNotFound/");
            var errors = new List<string>();
			var deserializedPokeTrainers = directoryReader.ReadFromDirectory<PokeTrainer>(path, ref errors);
        }

		[TestMethod]
		public void ReadFromDirectory_WhenCalled_ReturnDeserializedPokeTrainers()
        {
			var path = FileUtils.GetFullPath(FileUtils.UniformDirectoryReaderFolder);
            var errors = new List<string>();
            var deserializedPokeTrainers = directoryReader.ReadFromDirectory<PokeTrainer>(path, ref errors);
			Assert.IsTrue(deserializedPokeTrainers.Count == 40);
			Assert.IsTrue(errors.Count == 1);
        }
    }
}
