using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataLogic.Entities
{
    public class ArticleEntity
    {
        [Key]
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public int Price { get; set; }
        public int BookingSystemId { get; set; }
    }
}