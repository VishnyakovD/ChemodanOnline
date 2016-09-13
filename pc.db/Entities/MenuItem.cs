using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class MenuItem:idName
    {
        public virtual string path { get; set; }
        public virtual int type { get; set; }
        public MenuItem() { }
    }
}
