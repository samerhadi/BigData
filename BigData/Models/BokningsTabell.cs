using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigData.Models
{
    //Skapar bokningstabellen
    public class BokningsTabell
    {

        public int BookingId { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }
        public int UserMobile { get; set; }
        public string Subject { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int Price { get; set; }



    }
}