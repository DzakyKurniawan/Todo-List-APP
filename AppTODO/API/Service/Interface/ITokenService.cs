using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service.Interface
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        Token Get(string username);
        int Insert(TokenViewModel tokenViewModel);
        int Update(TokenViewModel tokenViewModel);

    }
}
