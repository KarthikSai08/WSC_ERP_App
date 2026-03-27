using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Shared.Contracts.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string entity, string value) : base($"{entity} is already exisiting in {value}")
        { }
    }
}
