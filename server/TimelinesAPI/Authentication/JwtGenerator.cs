﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TimelinesAPI.DataVaults;
using TimelinesAPI.Settings;

namespace TimelinesAPI.Authentication
{
    public static class JwtGenerator
    {
	    public static string Generate(JwtSettings config, UserEntity user)
	    {
		    var now = DateTime.UtcNow;

		    var claims = new Claim[]
		    {
			    new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
			    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(CultureInfo.InvariantCulture),
				    ClaimValueTypes.Integer64),
			    new Claim(ClaimTypes.Sid, user.Id.ToString()),
			    new Claim(ClaimTypes.Name, user.DisplayName),
			    new Claim(ClaimTypes.Role, "todo")
		    };

		    var keyByteArray = Encoding.ASCII.GetBytes(config.Secret);
		    var signingKey = new SymmetricSecurityKey(keyByteArray);

		    var jwt = new JwtSecurityToken(
			    issuer: config.Issuer,
			    audience: config.Audience,
			    claims: claims,
			    notBefore: now,
			    expires: now.AddDays(90),
			    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
		    );

		    var token = new JwtSecurityTokenHandler().WriteToken(jwt);
		    return token;
	    }
	}
}
