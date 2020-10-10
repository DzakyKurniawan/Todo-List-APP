using API.Service.Interface;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.Service
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public Token Get(string username)
        {
            return _tokenRepository.Get(username);
        }

        public int Insert(TokenViewModel tokenViewModel)
        {
            return _tokenRepository.Insert(tokenViewModel);
        }

        public int Update(TokenViewModel tokenViewModel)
        {
            return _tokenRepository.Update(tokenViewModel);
        }
    }
}
