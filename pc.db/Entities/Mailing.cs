using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Mailing:idName
    {
        public virtual string email { get; set; }
        public virtual bool isActive { get; set; }
        public Mailing() { }
    }
}
