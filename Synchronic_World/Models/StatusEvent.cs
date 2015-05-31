using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Synchronic_World.Models
{
    public class StatusEvent
    {
        public StatusEvent()
        {

        }

        [Key, HiddenInput(DisplayValue = false)]
        public int StatusEventId { get; set; }

        [Display(Name="Status")]
        public string Status { get; set; }
    }
}