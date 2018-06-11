using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimelinesAPI.DataVaults
{
    public class UserEntity
    {
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string DisplayName { get; set; }
    }
}
