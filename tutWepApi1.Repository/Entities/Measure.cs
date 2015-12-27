using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tutWepApi1.Repository.Entities
{
    public class Measure
    {
        public int Id;
        public int food_id;
        public string Description;
        public double Calories;
        public double Protien;
        public double Carbonhydrates;
        public double Fiber;
        public double Sugar;
    }

    public enum Category
    {
        Sweet, 
        Sour,
        Salt
    }
}
