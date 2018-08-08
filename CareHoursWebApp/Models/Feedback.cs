using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CareHoursWebApp.Models
{
    [DataContract]
    public class Feedback
    {
        [DataMember(Name="feedback")]
        public string FeedbackText { get; set; }
    }
}
