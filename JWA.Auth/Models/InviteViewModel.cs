using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Auth.Models
{
    public class InviteViewModel
    {
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Role Id is missing for inviting user")]
        public Guid RoleId { get; set; }
        [Required(ErrorMessage = "Facility Id is missing for inviting user")]
        public int FacilityId { get; set; }
        [Required(ErrorMessage = "Organization Id is missing for inviting user")]
        public int organization_id { get; set; }
    }
}
