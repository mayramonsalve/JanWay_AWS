using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
