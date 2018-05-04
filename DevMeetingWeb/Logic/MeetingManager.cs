using DevMeetingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevMeetingWeb.Vsts;
using Microsoft.Extensions.Options;

namespace DevMeetingWeb.Logic
{
    public class MeetingManager
    {
        int _devMeetingProjectId = 1;

        private VstsRepository _vs;

        readonly string[] _taskFields = new string[] {
            "System.Id",
            "System.Title",
            "System.State",
            "System.AssignedTo",
            "CSEngineering.ActivityStartDate",
            "CSEngineering.ActivityDuration"
        };

        public MeetingManager(VstsRepository vs, IOptions<MeetingManagerOptions> config)
        {
            var options = config.Value;

            if (options.ProjectId <= 0)
                throw new InvalidOperationException("invalid project Id");

            this._vs = vs;
            this._devMeetingProjectId = options.ProjectId;
        }

        public async Task<Meeting[]> ListMeetingsAsync()
        {
            var childTasks = await _vs.ListChildItemsAsync(_devMeetingProjectId, _taskFields);

            return childTasks.Select(t => new Meeting(t.Id)
            {
                Title = (string)t.Get("System.Title"),
                AssignedTo = (string)t.Get("System.AssignedTo"),
                State = (string)t.Get("System.State"),
                StartDate = (DateTime?)t.Get("CSEngineering.ActivityStartDate"),
                Duration = (float?)(double?)t.Get("CSEngineering.ActivityDuration")                
            }).ToArray();
        }
    }
}
