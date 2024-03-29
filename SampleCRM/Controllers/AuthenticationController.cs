﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// Please use whatever name you want for auth request and 'admin' as password. After receiving bearer token, push 'Authorize' button on top of the page and insert it in the appeared field with the preceding 'bearer ' word.
        /// </summary>
        /// <param name="authRequest"></param>
        /// <param name="signingEncodingKey"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<string> Post(AuthenticationRequest authRequest, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            if (string.IsNullOrWhiteSpace(authRequest.Name) || string.IsNullOrWhiteSpace(authRequest.Password) || authRequest?.Password != AppSettings.LoadAppSettings().AdminPassword)
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