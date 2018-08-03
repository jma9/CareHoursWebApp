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
        [DataMember(Name = "id")]
        public int ChildId { get; set; }
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }
    }
}
