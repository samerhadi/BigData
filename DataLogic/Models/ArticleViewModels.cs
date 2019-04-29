using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLogic.Models
{
    public class ArticleViewModels
    {
        public class AddArticleModel
        {
            public ArticleEntity Article { get; set; }
            public int BookingSystemServiceType { get; set; }
            public int BookingSystemId { get; set; }
        }
    }
}