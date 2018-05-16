using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevMeetingWeb.Models
{
    public class Meeting
    {
        public readonly int Id;
        public string Title { get; set; }
        public User AssignedTo { get; set; }
        public string State { get; set; }
        public DateTime? StartDate { get; set; }
        public float? Duration { get; set; }

        public Meeting(int id)
        {
            this.Id = id;
        }
    }
}
