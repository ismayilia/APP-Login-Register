using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Helpers;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class TokenService : ITokkenService
	{
		private readonly JWTSettings _jwt;

        public TokenService(IOptions<JWTSettings> options)
        {
            _jwt = options.Value;
        }
        public string GenerateJwtToken(AppUser user, List<string> roles)
		{
			var claims = new List<Claim>
		{
				// adi sub olsun type-i username olsun
			new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.NameIdentifier, user.UserName),
			new Claim(ClaimTypes.Email, user.Email)
			// claim-lere username falan elave edir burda
		}; // claim-in nameidentifer propertisi username olsun---heshdiyir

			// bashga neyise elave etmek istesek onuda foreache saliriq 
			roles.ForEach(role =>
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			});

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwt.ExpireDays));

			var token = new JwtSecurityToken(
				_jwt.Issuer,
				_jwt.Issuer,
				claims,
				expires: expires,
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
