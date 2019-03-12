using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataLogic.Entities
{
    public class BookingSystemEntity
    {
        [Key]
        public int BookningSystemId { get; set; }
        public string SystemName { get; set; }
        public string SystemDescription { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Adress { get; set; }
        public string LatitudeAndLongitude { get; set; }
        public string PostaICode { get; set; }
        public string City { get; set; }
        public ServiceType ServiceType { get; set; }
      
    }

    public enum ServiceType
        {
            Frisör = 1,
            Massage = 2,
            Ansiktebehandling = 3,
            Yoga = 4
            
            
        }
}