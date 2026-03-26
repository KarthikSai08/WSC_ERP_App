using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Shared.Contracts.ValueObjects
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}
