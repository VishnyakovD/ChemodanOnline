using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class ChemodanProvider : BaseUser
    {
        public virtual string fullName { get; set; }
        public virtual string typeOwnership { get; set; }
        
        //public ProviderState ProviderState { get; set; }


        public ChemodanProvider():base()
        {
          //  ProviderState = new ProviderState();
        }
    }
}
