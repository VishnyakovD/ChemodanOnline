using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.db.Entities
{
    public class Adress:idName
    {
        public virtual string postCode { get; set; }
        public virtual AreaCountry area { get; set; }
        public virtual City city { get; set; }
        public virtual TypeCity typeCity { get; set; }
        public virtual Street street { get; set; }
        public virtual TypeStreet typeStreet { get; set; }
        public virtual string numHome { get; set; }
        public virtual string level { get; set; }
        public virtual string numFlat { get; set; }
        public virtual string bulk { get; set; }
        public virtual string office { get; set; }
        public virtual EditAdress editAdress { get; set; }

        public Adress()
        {
            area= new AreaCountry();
            city = new City();
            typeCity = new TypeCity();
            street = new Street();
            typeStreet = new TypeStreet();
            editAdress = new EditAdress();
        }
    }

}