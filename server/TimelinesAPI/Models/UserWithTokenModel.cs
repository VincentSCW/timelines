using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimelinesAPI.DataVaults;

namespace TimelinesAPI.Models
{
    public class UserWithTokenModel
    {
	    public UserEntity User { get; set; }
		public string AccessToken { get; set; }
    }
}
