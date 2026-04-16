using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Shared.Contracts.Exceptions
{
    public class InvalidInputIdException : Exception
    {
        public InvalidInputIdException(int id) : base($"The provided {id} is invalid. IDs must be a positive integer.")
        {
        }
    }
}
