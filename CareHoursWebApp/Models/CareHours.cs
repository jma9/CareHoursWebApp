using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CareHoursWebApp.Models
{
    [DataContract]
    public class CareHours
    {
        [DataMember(Name = "EventId")]
        public int EventId { get; set; }

        [DataMember(Name = "ChildId")]
        public int ChildId { get; set; }

        [DataMember(Name = "StartTime")]
        public string StartTime { get; set; }

        [DataMember(Name = "EndTime")]
        public string EndTime { get; set; }
    }
}
