using System;
using System.Collections.Generic;

namespace PositionalFileReader
{
    public interface IDirectoryReader
    {
        List<Any> ReadFromDirectory<Any>(string path, ref List<string> errors) where Any : new();
        List<AnySpecification> ReadFromDirectoryWithSpecification<AnySpecification>(string path, ref List<string> errors) where AnySpecification : new();
    }
}
