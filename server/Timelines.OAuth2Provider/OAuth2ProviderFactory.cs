using System;
using System.Collections.Generic;
using System.Text;

namespace Timelines.OAuth2Provider
{
	public class OAuth2ProviderFactory
	{
		private readonly OpenIdAuthorization _openIdAuthorization;

		public OAuth2ProviderFactory(OpenIdAuthorization options)
		{
			_openIdAuthorization = options;
		}

		public IAuthProvider GetProvider(AccountType type)
		{
			switch (type)
			{
				case AccountType.LinkedIn:
					return new LinkedInProvider(_openIdAuthorization.LinkedInConfig);
				//case AccountType.Microsoft:
				//	return new MicrosoftProvider(_openIdAuthorization.MicrosoftConfig);
				default:
					throw new NotImplementedException();
			}
		}
	}
}
