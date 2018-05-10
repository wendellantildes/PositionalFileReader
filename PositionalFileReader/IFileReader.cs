using System;
using System.Collections.Generic;

namespace PositionalFileReader
{
    public interface IFileReader
    {
        List<Any> ReadFromFile<Any>(string path) where Any : new();
        AnySpecification ReadFromFileWithSpecification<AnySpecification>(string path) where AnySpecification : new();
    }
}
