using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevMeetingWeb.Logic;
using DevMeetingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevMeetingWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MeetingManager _meetingMgr;

        public Meeting[] MeetingList { get; set; }

        public IndexModel(MeetingManager meetingMgr)
        {
            this._meetingMgr = meetingMgr;
        }

        public void OnGet()
        {
            MeetingList = _meetingMgr.ListMeetingsAsync().Result;
        }
    }
}
