using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Library.Framework.Test.HttpService.Entities
{
    [DataContract]
    public class Operative
    {
        [DataMember]
        public int Status { get; set; }
    }
}
