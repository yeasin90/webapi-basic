using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tutWepApi1.Repository.Entities
{
    public class Food
    {
        public int Id;
        public string Description;
        public ICollection<Measure> Measures;
    }
}
