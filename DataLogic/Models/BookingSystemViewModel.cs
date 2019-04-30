using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLogic.Models
{
    public class BookingSystemViewModel
    {
        public class BookingSystemInformationModel
        {
            public BookingSystemEntity BookingSystem { get; set; }
            public List<ArticleEntity> ListOFArticles { get; set; }
            public ArticleEntity Article { get; set; }
        }
    }
}