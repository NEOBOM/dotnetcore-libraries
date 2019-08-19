using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Library.Framework.Test.HttpService.Entities
{
    [DataContract]
    public class Book
    {
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public decimal Count { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
