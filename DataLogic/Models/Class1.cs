using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLogic.Entities;

namespace DataLogic.Models
{
    public class Class1
    {
        public DateTime Tid { get; set; }
    }

    public class whatever
    {
        public DateTime Tid { get; set; }
        public BookingSystemEntity BookingSystem { get; set; }
    }
}