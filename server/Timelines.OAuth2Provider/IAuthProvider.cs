using System;
using System.Collections.Generic;
using System.Text;

namespace Timelines.OAuth2Provider
{
	public interface IAuthProvider
	{
		AuthResponse GetAuthResponse(string code);
	}
}
