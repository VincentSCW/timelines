using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Timelines.OAuth2Provider
{
	public interface IAuthProvider
	{
		Task<AuthResponse> GetAuthResponseAsync(string code);
	}
}
