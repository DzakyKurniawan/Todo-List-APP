using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class TokenViewModel
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public long ExpireToken { get; set; }
        public string RefreshToken { get; set; }
        public long ExpireRefreshToken { get; set; }
        public string Username { get; set; }
    }
}
