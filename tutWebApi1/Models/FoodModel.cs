using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tutWebApi1.Models
{
    public class FoodModel
    {
        public string Url { get; internal set; }
        public string Description;
        public IEnumerable<MeasureModel> Measures;
    }
}