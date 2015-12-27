using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tutWepApi1.Repository.Entities
{
    public class Diary
    {
        public int Id;
        public DateTime CurrentDate;
        public string UserName;
        public ICollection<DiaryEntry> DiaryEntires;
    }
}
