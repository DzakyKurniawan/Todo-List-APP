using Data.Base;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Model
{
    public class User : IdentityUser
    {
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }
        public bool IsDelete { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenExpiration { get; set; }
    }
}
