using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigData.Controllers
{
    public class SuggestionController : BaseController
    {
        public double CalculateDistance(string string1, string string2)
        {
            ConvertLatAndLongToDouble(string1, string2);

            double calculatedDistance = 0;
            return calculatedDistance;
        }

        //public List<double> ConvertLatAndLongToDouble(string string1, string string2)
        //{
        //    var listOfCalculatedLatAndLong = new List<double>();

        //    double calculatedLatAndLong1x = 0;
        //    double calculatedLatAndLong2x = 0;
        //    double calculatedLatAndLong1y = 0;
        //    double calculatedLatAndLong2y = 0;

        //    calculatedLatAndLong1x = Convert.ToDouble(string1.Substring(0, 9));
        //    calculatedLatAndLong2x = Convert.ToDouble(string1.Substring(11, 19));
        //    calculatedLatAndLong1y = Convert.ToDouble(string2.Substring(0, 9));
        //    calculatedLatAndLong2y = Convert.ToDouble(string2.Substring(11, 19));


        //    return listOfCalculatedLatAndLong;
        }
    }
}