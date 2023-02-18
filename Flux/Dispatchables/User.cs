using Flux.Dispatchables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Flux.Dispatchables
{
    public class User : DataEntityBase, IDispatchable
    {
        public ActionType ActionType { get; set; }
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

    }
}
