using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Settings;
namespace WebApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenService
    {
        /// <summary>
        /// 
        /// </summary>
        public TokenConfigurations TokenConfigurations { get; }

        /// <summary>
        /// 
        /// </summary>
        public SigningConfigurations SigningConfigurations { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenConfigurations"></param>
        /// <param name="signingConfigurations"></param>
        public TokenService(TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations)
        {
            TokenConfigurations = tokenConfigurations;
            SigningConfigurations = signingConfigurations;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public TokenResult GenerateTokenResult(User user)
        {
            DateTime created = DateTime.Now;
            DateTime expires = created.AddHours(TokenConfigurations.Hours);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = TokenConfigurations.Issuer[0],
                Audience = TokenConfigurations.Audience[0],
                SigningCredentials = SigningConfigurations.SigningCredentials,
                NotBefore = created,
                Expires = expires,
                Subject = new ClaimsIdentity(new Claim[] {
              new Claim(ClaimTypes.Name, user.Email),
              new Claim(ClaimTypes.Email, user.Email),              
            //new Claim(ClaimTypes.Role, user.Role.ToString())
          })
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResult(tokenHandler.WriteToken(token), created, expires, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult GenerateTokenJsonResult(User user)
        {
            return new JsonResult(GenerateTokenResult(user));
        }
    }
}