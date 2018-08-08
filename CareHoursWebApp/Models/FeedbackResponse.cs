using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CareHoursWebApp.Models
{
    [DataContract]
    public class FeedbackResponse
    {
        [DataMember(Name = "value")]
        public decimal Value { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
