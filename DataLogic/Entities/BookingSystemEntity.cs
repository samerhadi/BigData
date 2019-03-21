using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataLogic.Entities
{

    //skapa en bookingsystem model som används för att spara all information om tjänster
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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }
        public string PostaICode { get; set; }
        public string City { get; set; }
        //skapar en variabel av enum klassen
        public ServiceType ServiceType { get; set; }
      
    }

    //skapa en enum som innehåller alla tjänster
    public enum ServiceType
        {
            Frisör = 1,
            Massage = 2,
            Ansiktebehandling = 3,
            Yoga = 4
            
            
        }
}