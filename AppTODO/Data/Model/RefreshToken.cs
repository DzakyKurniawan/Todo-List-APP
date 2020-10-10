using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Model
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}
