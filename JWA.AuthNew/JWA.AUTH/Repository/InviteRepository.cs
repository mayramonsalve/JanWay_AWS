using JWA.AUTH.Interface;
using JWA.AUTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JWA.AUTH.Repository
{
    public class InviteRepository : IInviteService
    {
        private readonly AppDbContext context;

        public InviteRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Invite DeleteInvite(Invite invite)
        {
            var ExistingInvite = context.invites.FirstOrDefault(x => x.email == invite.email);
            if (ExistingInvite != null)
            {
                context.invites.Remove(ExistingInvite);
                context.SaveChanges();                
            }
            return invite;
        }
        public Invite GetInviteByEmailId(string email)
        {
            return context.invites.FirstOrDefault(x => x.email == email);
        }
        public Invite InsertInvite(Invite invite)
        {
            var result = context.invites.Add(invite);
            if (result.State.ToString() == "Added")
            {
                context.SaveChanges();
                Invite invite1 = new Invite();
                invite1.facility_id = result.Entity.facility_id;
                invite1.organization_id = result.Entity.organization_id;
                invite1.role_id = result.Entity.role_id;
                invite1.email = result.Entity.email;
                invite1.creation_date = result.Entity.creation_date;
                invite1.id = result.Entity.id;
                return invite1;
            }
            return invite;
        }
    }
}
