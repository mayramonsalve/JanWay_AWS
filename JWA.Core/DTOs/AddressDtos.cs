using System;
using System.Collections.Generic;

namespace JWA.Core.DTOs
{
    /// <summary>
    /// Address data sent over the network.
    /// </summary>
    public class AddressInfoDto
    {
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
    }
}