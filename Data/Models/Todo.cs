using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Todo : DataEntityBase
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public int NrTotal { get; set; }
        public int NrDone { get; set; }
        public TodoUnit Unit { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }

        public enum TodoUnit
        {
            HOURS,
            DAYS,
            TIMES,

        }
    }
}

