using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class UserClaim : IdentityUserClaim<Guid>
    {

        public virtual User User { get; set; }
    }
}
