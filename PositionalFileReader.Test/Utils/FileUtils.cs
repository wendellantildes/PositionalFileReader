using System;

namespace PositionalFileReader.Test.Utils
{
    public static class FileUtils
    {
        
		private const string filesFolder = "Files";
		public const string MultiformDirectoryReaderFolder = "MultiformDirectoryReader";
		public const string UniformDirectoryReaderFolder = "UniformDirectoryReader";
		public const string MultiformFileReaderFolder = "MultiformFileReader";
		public const string UniformFileReaderFolder = "UniformFileReader";
		public const string OkFile = "Ok.txt";
		public const string CustomConverterFile = "CustomConverter.txt";
		public const string PokeTrainerCustomWithOptionalValueFile = "PokeTrainerWithOptionalValue.txt";
		public const string PokeTrainerWithDataLengthSmallerThanMaxLineLengthFile = "PokeTrainerWithDataLengthSmallerThanMaxLineLength.txt";
		public const string MalFormed = "MalFormed.txt";
		public const string PokeTrainerWithHeaderAndTrailerFile = "PokeTrainerWithHeaderAndTrailer.txt";
		public const string PokeTrainerWithHeaderAndTrailerWhenHeaderIsMalformedFile = "PokeTrainerWithHeaderAndTrailerWhenHeaderIsMalformed.txt";
		public const string PokeTrainerWithHeaderAndTrailerWhernOneTrainerIsMalformedFile = "PokeTrainerWithHeaderAndTrailerWhenOneTrainerIsMalformed.txt";


		public static string GetFullPath(string directory, string filename){
			return System.IO.Path.GetFullPath($"{filesFolder}/{directory}/{filename}");
		}

		public static string GetFullPath(string directory)
        {
			return System.IO.Path.GetFullPath($"{filesFolder}/{directory}/");
        }

    }
}
