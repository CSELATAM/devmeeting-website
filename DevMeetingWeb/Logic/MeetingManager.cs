using DevMeetingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevMeetingWeb.Vsts;

namespace DevMeetingWeb.Logic
{
    public class MeetingManager
    {
        private VstsRepository _vs;

        public MeetingManager(VstsRepository vs)
        {
            this._vs = vs;
        }

        public async Task<Meeting[]> ListMeetingsAsync()
        {
            await _vs.CreateAsync();

            return new Meeting[] {
                new Meeting("devMeeting 1"),
                new Meeting("devMeeting 2")
            };
        }
    }
}
