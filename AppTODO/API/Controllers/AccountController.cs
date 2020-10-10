using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Service.Interface;
using Client.Models;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        public AccountController(IConfiguration configuration, UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Username", user.Id)
                };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        authClaims,
                        expires: DateTime.UtcNow.AddMinutes(40),
                        signingCredentials: signIn);
                    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                    var expirationToken = DateTime.UtcNow.AddMinutes(40).Ticks;
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    var expirationRefreshToken = DateTime.UtcNow.AddMinutes(140).Ticks;

                    Token resp = _tokenService.Get(model.Email);
                    if (resp == null)
                    {
                        _tokenService.Insert(new TokenViewModel
                        {
                            Username = model.Email,
                            AccessToken = accessToken,
                            ExpireToken = DateTime.UtcNow.AddMinutes(40).Ticks,
                            RefreshToken = refreshToken,
                            ExpireRefreshToken = expirationRefreshToken
                        });
                    }
                    else
                    {
                        _tokenService.Update(new TokenViewModel
                        {
                            Username = model.Email,
                            AccessToken = accessToken,
                            ExpireToken = DateTime.UtcNow.AddMinutes(40).Ticks,
                            RefreshToken = refreshToken,
                            ExpireRefreshToken = expirationRefreshToken
                        });
                    }
                    return Ok(new
                    {
                        AccessToken = accessToken,
                        ExpireToken = expirationToken,
                        RefreshToken = refreshToken,
                        ExpireRefreshToken = expirationRefreshToken
                    }
                        );
                }
            }
            catch (Exception e)
            {
                return Unauthorized(e);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenViewModel tokenViewModel)
        {
            try
            {
                var refToken = _tokenService.Get(tokenViewModel.Username);
                if(refToken.ExpireRefreshToken < DateTime.UtcNow.Ticks)
                {
                    return Unauthorized();
                }
                if (refToken.RefreshToken == tokenViewModel.RefreshToken)
                {
                    var authClaim = new List<Claim>
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var acctoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        authClaim,
                        expires: DateTime.UtcNow.AddMinutes(40),
                        signingCredentials: signIn);
                    var accessToken = new JwtSecurityTokenHandler().WriteToken(acctoken);
                    var expirationToken = DateTime.UtcNow.AddMinutes(40).Ticks;
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    var expirationRefreshToken = DateTime.UtcNow.AddMinutes(140).Ticks;

                    Token resp = _tokenService.Get(tokenViewModel.Username);
                    if (resp == null)
                    {
                        _tokenService.Insert(new TokenViewModel
                        {
                            Username = tokenViewModel.Username,
                            AccessToken = accessToken,
                            ExpireToken = DateTime.UtcNow.AddMinutes(40).Ticks,
                            RefreshToken = refreshToken,
                            ExpireRefreshToken = expirationRefreshToken
                        });
                    }
                    else
                    {
                        _tokenService.Update(new TokenViewModel
                        {
                            Username = tokenViewModel.Username,
                            AccessToken = accessToken,
                            ExpireToken = DateTime.UtcNow.AddMinutes(40).Ticks,
                            RefreshToken = refreshToken,
                            ExpireRefreshToken = expirationRefreshToken
                        });
                    }
                    return Ok(new
                    {
                        AccessToken = accessToken,
                        ExpireToken = expirationToken,
                        RefreshToken = refreshToken,
                        ExpireRefreshToken = expirationRefreshToken
                    }
                        );
                }
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
            return Unauthorized();
        }
    }
}