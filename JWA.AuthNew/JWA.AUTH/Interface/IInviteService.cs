using JWA.AUTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Interface
{
    public interface IInviteService
    {
        Invite GetInviteByEmailId(string email);
        Invite DeleteInvite(Invite invite);
        Invite InsertInvite(Invite invite);
    }
}
