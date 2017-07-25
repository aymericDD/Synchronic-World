using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Synchronic_World.Models
{
    public class Type
    {
        public enum StatusEvent
        {
            Open, Closed, Pending
        }

        public enum TypeEvent
        {
            Party, Lunch
        }

        public enum TypeContributionEvent
        {
            Money, Food, Beverage
        }
    }
}