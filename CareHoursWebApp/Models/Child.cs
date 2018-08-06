using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CareHoursWebApp.Models
{
    [DataContract]
    public class Child
    {
        [DataMember(Name = "ChildId")]
        public int ChildId { get; set; }
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }
    }
}
