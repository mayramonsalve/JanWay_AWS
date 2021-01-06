using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Auth.Models
{
    public class RecoverPasswordViewModel
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
