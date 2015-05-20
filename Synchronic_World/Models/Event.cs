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
        public Event() { }

        [Key, HiddenInput(DisplayValue=false)]
        public int EventId { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime StartDate { get; set; }

    }
}