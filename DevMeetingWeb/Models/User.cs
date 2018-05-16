using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevMeetingWeb.Models
{
    public class User
    {
        public User(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
    }
}
