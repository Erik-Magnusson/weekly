using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Dispatchables
{
    public class Week : IDispatchable
    {
        public ActionType ActionType { get; set; }
        public int WeekNr { get; set; }
        public Week(int weekNr)
        {
            ActionType = ActionType.UPDATE_WEEK;
            WeekNr = weekNr;
        }
    }
}
