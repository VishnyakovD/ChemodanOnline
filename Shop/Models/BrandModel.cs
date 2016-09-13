using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shop.db.Entities;

namespace Shop.Models
{
    public class BrandModel:idName
    {

        public BrandModel()
        {
            
        }

        public Brand GetBrandDB()
        {
            return new Brand(){id = this.id,name=this.name };
        }
    
    }
}
