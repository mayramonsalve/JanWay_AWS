using JWA.Auth.Interface;
using JWA.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Auth.Repository
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly AppDbContext context;
        public OrganizationServices(AppDbContext context)
        {
            this.context = context;
        }
        public Organization GetOrganization(int id)
        {
            return context.organizations.FirstOrDefault(x => x.id == id);
        }
    }
}
