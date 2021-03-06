﻿using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class Organization : BaseEntity
    {
        public Organization()
        {
            Facilities = new HashSet<Facility>();
            Supervisors = new HashSet<Supervisor>();
            Invites = new HashSet<Invite>();
        }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int AddressId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Facility> Facilities { get; set; }
        public virtual ICollection<Supervisor> Supervisors { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }

        public static implicit operator Organization(Facility v)
        {
            throw new NotImplementedException();
        }
    }
}
