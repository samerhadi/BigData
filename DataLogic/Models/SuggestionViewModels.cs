using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLogic.Models
{
    public class SuggestionViewModels
    {
        public class SuggestionViewModel
        {
            public List<Suggestion> ListOfSuggestionsFromDifferentBookingSystems { get; set; }
            public List<Suggestion> ListOfSuggestionsFromSameBookingSystems { get; set; }
            public BookingTableEntity BookingTable { get; set; }
            public ArticleEntity Article { get; set; }
            public BookingSystemEntity BookingSystem { get; set; }
        }

        public class Suggestion
        {
            public ArticleEntity Article { get; set; }
            public BookingSystemEntity BookingSystem { get; set; }
            public List<Times> ListOfTimes { get; set; }
            public DateTime Date { get; set; }
        }
    }
}