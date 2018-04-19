using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevMeetingWeb.Models
{
    public class Meeting
    {
        public readonly string Title;

        public Meeting(string title)
        {
            Title = title;
        }
    }
}
