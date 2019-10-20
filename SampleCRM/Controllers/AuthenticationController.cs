using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleCRM.Models;
using SampleCRM.Utilities;

namespace SampleCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [AllowAnonymous]
        public ActionResult<string> Post(AuthenticationRequest authRequest, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            if (authRequest.Password != "testPassword")
            {
                return Unauthorized();
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, authRequest.Name)
            };

            var token = new JwtSecurityToken(
                issuer: "AiratApp",
                audience: "AiratClients",
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: new SigningCredentials(
                        signingEncodingKey.GetKey(),
                        signingEncodingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}