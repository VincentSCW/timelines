using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TimelinesAPI.Settings;

namespace TimelinesAPI.Authentication
{
    public static class AuthenticationExtensions
    {
	    public static IServiceCollection AddAuthenticationOAuth(this IServiceCollection services, JwtSettings settings)
	    {
		    var keyByteArray = Encoding.ASCII.GetBytes(settings.Secret);
		    var signingKey = new SymmetricSecurityKey(keyByteArray);

		    var tokenValidationParameters = new TokenValidationParameters
		    {
			    // The signing key must match!
			    ValidateIssuerSigningKey = true,
			    IssuerSigningKey = signingKey,

			    // Validate the JWT Issuer (iss) claim
			    ValidateIssuer = true,
			    ValidIssuer = settings.Issuer,

			    // Validate the JWT Audience (aud) claim
			    ValidateAudience = true,
			    ValidAudience = settings.Audience,

			    // Validate the token expiry
			    ValidateLifetime = true,

			    ClockSkew = TimeSpan.Zero
		    };
		    services.AddAuthentication(options =>
			    {
				    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			    })
			    .AddJwtBearer(o =>
			    {
#if DEBUG
				    // don't use https for debug
				    o.RequireHttpsMetadata = false;
#endif
				    o.TokenValidationParameters = tokenValidationParameters;
			    });

		    return services;
	    }
    }
}
