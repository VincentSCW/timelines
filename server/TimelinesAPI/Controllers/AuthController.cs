using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timelines.OAuth2Provider;
using TimelinesAPI.Authentication;
using TimelinesAPI.DataVaults;
using TimelinesAPI.Models;
using TimelinesAPI.Settings;

namespace TimelinesAPI.Controllers
{
	[Route("api/[controller]")]
	public class AuthController : Controller
	{
		private readonly OAuth2ProviderFactory _providerFactory;
		private readonly JwtSettings _settings;
		public AuthController(OAuth2ProviderFactory providerFactory, JwtSettings settings)
		{
			_providerFactory = providerFactory;
			_settings = settings;
		}

		[HttpPost]
		[ProducesResponseType(typeof(UserWithTokenModel), 200)]
		public async Task<IActionResult> Authenticate([FromBody] AuthModel model)
		{
			var type = Enum.Parse<AccountType>(model.AccountType, true);
			var provider = _providerFactory.GetProvider(type);

			try
			{
				var response = await provider.GetAuthResponseAsync(model.Code);

				// Only for one user here
				if (response.UserInfo.Id != "m04Ru5Jrhk")
					throw new InvalidOperationException("Invalid user.");

				UserEntity user = null;
				switch (type)
				{
					case AccountType.LinkedIn:
						user = new UserEntity
						{
							Username = MockUser.Username,
							DisplayName = response.UserInfo.DisplayName,
						};
						break;
					default:
						throw new InvalidOperationException("Not supported account type.");
				}

				return Ok(new UserWithTokenModel
				{
					User = user,
					AccessToken = JwtGenerator.Generate(_settings, user)
				});
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}
