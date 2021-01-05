using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class Address : BaseEntity
    {
        public Address()
        {
            Facilities = new HashSet<Facility>();
            Organizations = new HashSet<Organization>();
        }

        public string Street { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<Facility> Facilities { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
