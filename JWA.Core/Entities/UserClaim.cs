using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class UserClaim 
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public virtual User User { get; set; }
    }
}
