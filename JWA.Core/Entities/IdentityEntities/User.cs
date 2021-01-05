using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class User : IdentityUser<Guid, UserLogin, UserRole, UserClaim>
    {
        public User()
        {
            Supervisors = new HashSet<Supervisor>();
            UserClaims = new HashSet<UserClaim>();
            UserLogins = new HashSet<UserLogin>();
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserToken>();
        }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<Supervisor> Supervisors { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
