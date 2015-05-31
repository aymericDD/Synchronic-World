using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Synchronic_World.Models
{
    public class TypeEvent
    {
        public TypeEvent()
        {

        }

        [Key, HiddenInput(DisplayValue = false)]
        public int TypeEventId { get; set; }

        [Display(Name="Event")]
        public string Type { get; set; }
    }
}