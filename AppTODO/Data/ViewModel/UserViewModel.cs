using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class UserViewModel : IdentityUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
    }
}
