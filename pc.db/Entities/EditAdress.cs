using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.db.Entities
{
    public class EditAdress : idName
    {
        public virtual string postCode { get; set; }
        public virtual string area { get; set; }//область
        public virtual string city { get; set; }
        public virtual string typeCity { get; set; }
        public virtual string street { get; set; }
        public virtual string typeStreet { get; set; }
        public virtual string numHome { get; set; }
        public virtual string level { get; set; }
        public virtual string numFlat { get; set; }
        public virtual string bulk { get; set; }//корпус
        public virtual string office { get; set; }

        public EditAdress()
        {

        }
    }

}