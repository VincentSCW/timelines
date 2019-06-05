using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TimelinesAPI.DataVaults;

namespace TimelinesAPI.Models
{
    public class TimelineModel
    {
		public string Username { get; set; }
		public string TopicKey { get; set; }
		public string Title { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public ProtectLevel ProtectLevel { get; set; }
		public string AccessKey { get; set; }
	    [JsonConverter(typeof(StringEnumConverter))]
		public PeriodGroupLevel PeriodGroupLevel { get; set; }
		public bool IsCompleted { get; set; }
        public DateTime StartTime { get; set; }
    }
}
