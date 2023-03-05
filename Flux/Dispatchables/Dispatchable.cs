using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Dispatchables
{
    public class Dispatchable<T> : IDispatchable
    {
        public ActionType ActionType { get; set; }

        public T Value { get; set; }
    }
}
