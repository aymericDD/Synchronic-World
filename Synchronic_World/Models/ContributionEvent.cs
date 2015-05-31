using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Synchronic_World.Models
{
    public class ContributionEvent
    {
        public ContributionEvent()
        {

        }

        [Key, HiddenInput(DisplayValue = false)]
        public int ContributionEventId { get; set; }

        [Required]
        [Display(Name="Name")]
        public string ContributionEventName { get; set; }

        [Required]
        [Display(Name="Quantity")]
        public float ContributionEventQuantity { get; set; }

        [Required]
        public Type.TypeContributionEvent TypeContributionEvent { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User UserTable { get; set; }

        [Required]
        public int? EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event EventTable { get; set; }
    }
}