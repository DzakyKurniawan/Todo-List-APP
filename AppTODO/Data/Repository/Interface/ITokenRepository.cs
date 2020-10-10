using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interface
{
    public interface ITokenRepository
    {
        Token Get(string username);
        int Insert(TokenViewModel tokenViewModel);
        int Update(TokenViewModel tokenViewModel);
    }
}
