using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tutWebApi1.Models
{
    public class MeasureModel
    {
        public string Url { get; internal set; }
        public string Description;
        public double Calories;
    }
}