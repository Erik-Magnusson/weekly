using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.Todo;

namespace Data.Models
{
    public class Template : DataEntityBase
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public int NrTotal { get; set; }
        public TodoUnit Unit { get; set; }
    }
}
