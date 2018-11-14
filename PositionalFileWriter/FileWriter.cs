using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Kernel;

namespace PositionalFileWriter
{
    public class FileWriter
    {

        public string SerializeFromSpecification<AnySpecification>(AnySpecification anySpecification) where AnySpecification : new(){
            var properties = anySpecification.GetType().GetProperties();
            var textParts = new Dictionary<int, string>();
            foreach(var property in properties){
                var specification = GetSpecificationFrom(property);
                var list = property.GetValue(anySpecification) as IList;
                if (specification.HasMoreThanOne)
                {
                    if (list == null)
                    {
                        throw new LineSpecificationException($"Specification has set attribute HasMoreThanOne = true, but the property {property.Name} is not a list.");
                    }
                    else
                    {
                        var stringBuilder = new StringBuilder();
                        foreach(var value in list){
                            stringBuilder.AppendLine(this.Serialize(value));
                        }
                        textParts.Add(specification.SerializationOrder, stringBuilder.ToString());
                    }

                }
                else
                {
                    if (list == null)
                    {
                        var value = property.GetValue(anySpecification);
                        textParts.Add(specification.SerializationOrder, this.Serialize(value));
                    }
                    else
                    {
                        throw new LineSpecificationException($"Specification has set attribute HasMoreThanOne = false, but the property {property.Name} is a list.");
                    }
                }
            }

            return ConvertToText(textParts);
        }

        private LineSpecificationAttribute GetSpecificationFrom(PropertyInfo property)
        {
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

        public string Serialize<Any>(Any any){
            var lineparts = new Dictionary<int, string>();
            var properties = any.GetType().GetProperties();
            foreach(var property in properties){
                var dataAttributes = property.GetCustomAttributes(typeof(DataAttribute), true);
                if (dataAttributes == null || dataAttributes.Length == 0)
                {
                    continue;
                }else if (dataAttributes.Length == 1){
                    var dataAttribute = (DataAttribute)dataAttributes[0];
                    var rawValue = property.GetValue(any);
                    if (dataAttribute.Optional && rawValue == null)
                    {
                        continue;
                    }
                    if (dataAttribute.CustomUndoMethod == null)
                    {
                        lineparts.Add(dataAttribute.StartIndex, ApplyLeftPadding(dataAttribute, rawValue));
                    }
                    else
                    {
                        var method = any.GetType().GetMethod(dataAttribute.CustomUndoMethod);
                        var newValue = method.Invoke(any, new object[] { rawValue });
                        lineparts.Add(dataAttribute.StartIndex, ApplyLeftPadding(dataAttribute,  newValue));
                    }

                }else{
                    throw new DuplicatedDataAttributeException($"It's not possible parse {property} because it has two DataAnnotation.");   
                }
            }
            return ConvertToLine(lineparts);
        }

        private string ApplyLeftPadding(DataAttribute dataAttribute, object value){
            if(dataAttribute.LeftPadding != '\0' &&  value != null){
                var strValue = Convert.ToString(value);
                var length =  dataAttribute.Length - strValue.Length + 1;
                return strValue.PadLeft(length, dataAttribute.LeftPadding);
            }else if(value == null){
                return "";
            }else{
                return Convert.ToString(value);
            }
        }

        private string ConvertToLine(Dictionary<int,string> lineParts){
            var stringBuilder = new StringBuilder();
            var keys = lineParts.Keys.ToList();
            keys.Sort();
            for (var i = 0; i < keys.Count; i++){
                var currentKey = keys[i];
                var value = lineParts[keys[i]];
                stringBuilder.Append(value);
                var hasNextKey = i + 1 < keys.Count;
                if(hasNextKey && keys[i+1] > stringBuilder.Length){
                    var qtOfEmpty = keys[i + 1] - stringBuilder.Length;
                    stringBuilder.Append("".PadLeft(qtOfEmpty));
                }
            }
            return stringBuilder.ToString();
        }

        private string ConvertToText(Dictionary<int, string> textParts)
        {
            var stringBuilder = new StringBuilder();
            var keys = textParts.Keys.ToList();
            keys.Sort();
            for (var i = 0; i < keys.Count; i++)
            {
                stringBuilder.AppendLine(textParts[i]);
            }
            return stringBuilder.ToString();
        }
    }
}
