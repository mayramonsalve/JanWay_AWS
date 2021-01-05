using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class State : BaseEntity
    {
        public State()
        {
            Addresses = new HashSet<Address>();
        }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
