using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimelinesAPI.Settings
{
    public class JwtSettings
    {
	    public string Secret { get; set; }
	    public string Issuer => "timelines";
	    public string Audience => "timelines";
    }
}
