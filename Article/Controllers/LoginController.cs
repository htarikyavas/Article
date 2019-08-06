using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Article.Core.Domain;
using Article.Core.Models;
using Article.Services.Authors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Article.WebApi.Controllers
{

    public class LoginController : ControllerBase
    {
        public IAuthorService _authorService;
        private SigningCredentials _signingCredentials;

        public LoginController(IAuthorService authorService, SigningCredentials signingCredentials)
        {
            _authorService = authorService;
            _signingCredentials = signingCredentials;
        }

        [HttpPost]
        [Route("login")]
        public async Task<LoginResponseModel> Login([FromBody] LoginRequestModel loginRequest)
        {
            var author = await _authorService.GetAuthorByEMailAndPassword(loginRequest.Email, loginRequest.Password);
            if (author == null)
            {
                throw new Exception("invalid username/password combination");
            }

            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("id", author.Id.ToString()));
            ci.AddClaim(new Claim("email", author.Email));

            var handler = new JwtSecurityTokenHandler();

            var access_token = handler.CreateEncodedJwt(issuer: "Article", audience: "Article Users", subject: ci, notBefore: DateTime.Now, issuedAt: DateTime.Now, expires: DateTime.Now + TimeSpan.FromHours(24), signingCredentials: _signingCredentials);

            return new LoginResponseModel()
            {
                access_token = access_token
            };
        }
    }
}