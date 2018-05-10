using System;
using System.IO;
using System.Collections.Generic;

namespace PositionalFileReader
{
    public class DirectoryReader : IDirectoryReader
    {
        private IFileReader fileReader;

        public DirectoryReader(){
            this.fileReader = new FileReader();
        }

        /// <summary>
        /// It reads from a directory a list of files where the list of lines have the same pattern and deserialize them into an object.
        /// </summary>
        /// <returns>The from directory.</returns>
        /// <param name="path">Path.</param>
        /// <param name="errors">Errors.</param>
        /// <typeparam name="Any">The 1st type parameter.</typeparam>
        public List<Any> ReadFromDirectory<Any>(string path, ref List<string> errors) where Any : new()
        {
            if (Directory.Exists(path))
            {
                var fileNames = Directory.GetFiles(path);
                var anyList = new List<Any>();
                foreach(var filename in fileNames){
                    try{
                        var currentAnyList = this.ReadFileFromFile<Any>(filename);
                        anyList.AddRange(currentAnyList);
                    }catch(Exception ex){
						errors.Add($"Error in file {filename}: {ex.Message}");
                    }
                }
                return anyList;
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory {path} not found.");
            }
        }

        /// <summary>
        /// It reads from file a list of lines with different patterns and deserialize into a specification object.
        /// </summary>
        /// <returns>A specification object with the set of different line patterns.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="AnySpecification">The 1st type parameter.</typeparam>
        public List<AnySpecification> ReadFromDirectoryWithSpecification<AnySpecification>(string path, ref List<string> errors) where AnySpecification : new()
        {
            if (Directory.Exists(path))
            {
                var fileNames = Directory.GetFiles(path);
                var anySpecificationList = new List<AnySpecification>();
                foreach (var filename in fileNames)
                {
                    try
                    {
                        var anySpecification = this.ReadFileFromFileWithSpecification<AnySpecification>(filename);
                        anySpecificationList.Add(anySpecification);
                    }
                    catch (Exception ex)
                    {
						errors.Add($"Error in file {filename}: {ex.Message}");
                    }

                }
                return anySpecificationList;
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory {path} not found.");
            }
        }

        private List<Any> ReadFileFromFile<Any>(string path) where Any : new(){
            return this.fileReader.ReadFromFile<Any>(path);   
        }

        private AnySpecification ReadFileFromFileWithSpecification<AnySpecification>(string path) where AnySpecification : new()
        {
            return this.fileReader.ReadFromFileWithSpecification<AnySpecification>(path);
        }
    }
}
