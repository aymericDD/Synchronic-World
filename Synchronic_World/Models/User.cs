using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Synchronic_World.Models
{
    public class User
    {
        public User()
        {
            friends = new HashSet<User>();
            Events = new HashSet<Event>();
            ParticpationEvents = new HashSet<Event>();
        }

        [Key, HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "User email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddressAttribute]
        public string UserEmail { get; set; }

        [Required]
        [Display(Name = "User password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2}      characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Display(Name = "Enrollment date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [HiddenInput(DisplayValue= false)]
        public int? UserRoleId { get; set; }

        [ForeignKey("UserRoleId")]
        public virtual RoleUser RoleUserTable { get; set; }

        public virtual ICollection<User> friends { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Event> ParticpationEvents { get; set; }
    }
}