using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Shared.Contracts.Exceptions
{
    public class InSufficientException : Exception
    {
        public InSufficientException(string entity, int stock) : base($"InSufficient Stock for {entity} only has {stock} Nos. Please try again") { }

        public InSufficientException() : base("Insufficient Stock!") { }
    }
}
