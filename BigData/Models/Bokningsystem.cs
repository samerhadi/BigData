using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigData.Models
{
    //skapar klassen för bokningtjänsten
    public class Bokningsystem

    {
        public int BookningSystemId { get; set; }
        public string SystemName { get; set; }
        public string SystemDescription { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Website { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public int ContactPhone { get; set; }
        public string Adress { get; set; }
        public string LatitudeAndLongitude { get; set; }
        public string PostaICode {get; set;}
        public string City { get; set; }

    }
}