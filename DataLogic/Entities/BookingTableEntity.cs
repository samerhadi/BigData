using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataLogic.Entities
{
    public class BookingTableEntity
    {
        [Key]
        public int BookingTableId { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }
        public int UserMobile { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Price { get; set; }
        public int ArticleId { get; set; }
      
    }
}
