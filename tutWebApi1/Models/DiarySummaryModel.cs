using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tutWebApi1.Models
{
    public class DiarySummaryModel
    {
        public DateTime DiaryDate { get; set; }
        public double TotalCalories { get; set; }
    }
}