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
        public int Service { get; set; }
    }

    public enum Hairdresser
    {
        Haircut = 1,
        Coloring = 2
    }

    public enum Workshop
    {
        TireChange = 1,
        Service = 2,
    }

    public enum Massage
    {
        Swedish = 1,
        Neck = 2
    }

    public enum BeautySalon
    {
        Face = 1,
        Nails = 2
    }

}