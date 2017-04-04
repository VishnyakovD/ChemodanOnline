using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Client : BaseUser
    {
        public virtual Sex sex { get; set; }

        public Client():base()
        {
            sex = new Sex();
        }
    }
}
