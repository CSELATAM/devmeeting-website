using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevMeetingWeb
{
    public class VsTaskItem
    {
        public int Id { get; set; }
        public IDictionary<string,object> Fields { get; set; }

        public object Get(string key)
        {
            Fields.TryGetValue(key, out object value);

            return value;
        }
    }
}
