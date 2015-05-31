using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Synchronic_World.Models
{
    public class Event
    {
        public Event() 
        {
            Participants = new HashSet<User>();
        }

        [Key, HiddenInput(DisplayValue=false)]
        public int EventId { get; set; }

        
        [Required]
        [Display(Name = "Start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name="Title")]
        public string Title { get; set; }
        
        [Required]
        [Display(Name="Content")]
        public string Content { get; set; }
        
        [Required]
        [Display(Name="Address")]
        public string Address { get; set; }
        
        [HiddenInput(DisplayValue= false)]
        public int? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User UserTable { get; set; }
        
        //[HiddenInput(DisplayValue= false)]
        //public int TypeEventId  { get; set; }

        //[ForeignKey("TypeEventId")]
        //public virtual TypeEvent TypeEventTable { get; set; }

        //[HiddenInput(DisplayValue = false)]
        //public int StatusEventId { get; set; }

        //[ForeignKey("StatusEventId")]
        //public virtual StatusEvent StatusEventTable { get; set; }

        [Required]
        public Type.StatusEvent StatusEvent { get; set; }

        [Required]
        public Type.TypeEvent TypeEvent { get; set; }

        [HiddenInput(DisplayValue = false)]
        public virtual ICollection<User> Participants { get; set; }

    }
}