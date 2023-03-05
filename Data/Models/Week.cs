using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Week
    {
        public int WeekNr { get; set; }

        public Week(int weekNr)
        {
            WeekNr = weekNr;
        }   
    }
}
