using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using Kernel;

namespace PositionalFileReader
{
    public class FileReader : IFileReader
    {
        /// <summary>
        /// It reads from file a list of lines with the same pattern and deserialize into an object.
        /// </summary>
        /// <returns>A list of objects with the same pattern.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="Any">The 1st type parameter.</typeparam>
        public List<Any> ReadFromFile<Any>(string path) where Any : new(){
            if(File.Exists(path)){
                var anyList = new List<Any>();
                foreach(var line in File.ReadAllLines(path)){
                    var any = this.Deserialize(line, typeof(Any));
                    anyList.Add(any);
                }
                return anyList;
            }else{
                throw new FileNotFoundException("File not found", path);
            }
        }

        /// <summary>
        /// It reads from file a list of lines with different patterns and deserialize into a specification object.
        /// </summary>
        /// <returns>A specification object with the set of different line patterns.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="AnySpecification">The 1st type parameter.</typeparam>
        public AnySpecification ReadFromFileWithSpecification<AnySpecification>(string path) where AnySpecification : new()
        {
            if (File.Exists(path))
            {
                var anySpecification = new AnySpecification();
                var properties = anySpecification.GetType().GetProperties();
                foreach (var line in File.ReadAllLines(path))
                {
                    foreach(var property in properties){
                        var specification = GetSpecificationFrom(property);
                        var response = anySpecification.GetType().GetMethod(specification.TestMethod).Invoke(anySpecification, new object[] { line });
                        var isCorrectLine = Convert.ToBoolean(response);
                        if(isCorrectLine){
                            var t = specification.Any;
                            var currentValue = this.Deserialize(line, specification.Any);
                            var list = property.GetValue(anySpecification) as IList;
                            if(specification.HasMoreThanOne){
                                if(list == null){
                                    throw new LineSpecificationException($"Specification has set attribute HasMoreThanOne = true, but the property {property.Name} is not a list.");
                                }else{
                                    list.Add(currentValue);
                                }
                                    
                            }else{
                                if (list == null)
                                {
                                    property.SetValue(anySpecification,currentValue);
                                }
                                else
                                {
                                    throw new LineSpecificationException($"Specification has set attribute HasMoreThanOne = false, but the property {property.Name} is a list.");
                                }
                            }
                        }else{
                            continue;
                        }
                    }
                }
                return anySpecification;
            }
            else
            {
                throw new FileNotFoundException("File not found", path);
            }
        }

        private LineSpecificationAttribute GetSpecificationFrom(PropertyInfo property){
            var dataAttributes = property.GetCustomAttributes(typeof(LineSpecificationAttribute), true);
            if (dataAttributes == null || dataAttributes.Length == 0)
            {
                return null;
            }
            else
            {
                return dataAttributes[0] as LineSpecificationAttribute;
            }
        }

        /// <summary>
        /// Deserialize the specified line into an object.
        /// </summary>
        /// <returns>The object deserialized.</returns>
        /// <param name="line">Line.</param>
        /// <param name="objDeserializableType">Object deserializable type.</param>
        public dynamic Deserialize(string line, Type objDeserializableType)
        {
            var any = Activator.CreateInstance(objDeserializableType);
            var properties = any.GetType().GetProperties();
            foreach (var property in properties)
            {
                var dataAttributes = property.GetCustomAttributes(typeof(DataAttribute), true);
                if (dataAttributes == null || dataAttributes.Length == 0)
                {
                    continue;
                }
                else if (dataAttributes.Length == 1)
                {
                    var dataAttribute = dataAttributes[0] as DataAttribute;
                    var length = dataAttribute.StartIndex + dataAttribute.Length <= line.Length ? dataAttribute.Length : line.Length - dataAttribute.StartIndex;
                    var rawValue = line.Substring(dataAttribute.StartIndex, length);
                    try
                    {
                        if(dataAttribute.Optional && string.IsNullOrWhiteSpace(rawValue)){
                            continue;
                        }
                        if (dataAttribute.CustomConverterMethod == null)
                        {
                            var typedValue = Convert.ChangeType(rawValue, dataAttribute.DataType);
                            property.SetValue(any, typedValue);
                        }
                        else
                        {
                            var method = any.GetType().GetMethod(dataAttribute.CustomConverterMethod);
                            var typedValue = method.Invoke(any, new object[] { rawValue });
                            property.SetValue(any, typedValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new DeserializationException($"Error to parse line {line}.", ex);
                    }
                }
                else
                {
					throw new DuplicatedDataAttributeException($"It's not possible parse {property} because it has two DataAnnotation.");
                }
            }
            return any;
        }
    }
}
