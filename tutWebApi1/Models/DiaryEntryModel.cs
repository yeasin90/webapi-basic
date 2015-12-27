using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tutWebApi1.Models
{
    public class DiaryEntryModel
    {
        public int Id;
        public int diary_id;
        public string Url { get; internal set; }
        public int input1;
        public string input2;
    }
}