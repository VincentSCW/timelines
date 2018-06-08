﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Timelines.OAuth2Provider
{
	public class LinkedInProvider : AuthProviderBase
	{
		public LinkedInProvider(OAuth2Config config)
			: base(config)
		{
		}

		protected override string AccessTokenUrl => "https://www.linkedin.com/uas/oauth2/accessToken";
		protected override string UserInfoUrl => "https://api.linkedin.com/v1/people/~:(id,formatted-name,picture-url)?format=json";
		protected override int GetUserInfoMethod => 0;

		protected override UserInfo GetUserInfo(JObject obj)
		{
			return new UserInfo
			{
				Id = (string)obj["id"],
				DisplayName = (string)obj["formattedName"],
				AvatarUrl = (string)obj["pictureUrl"]
			};
		}
	}
}
