using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Synchronic_World.Models
{
    public class RoleUser
    {
        public RoleUser()
        {

        }

        [Key, HiddenInput(DisplayValue = false)]
        public int RoleUserId { get; set; }

        [Display(Name="Role")]
        public string Role { get; set; }
    }
}