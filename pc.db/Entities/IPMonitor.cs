using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class IPMonitor
    {
        public virtual long id { get; set; }
        public virtual string ip { get; set; }
        public virtual string page { get; set; }
        public virtual string clientId { get; set; }
        public virtual DateTime date { get; set; }
        public IPMonitor() { }
    }
}
