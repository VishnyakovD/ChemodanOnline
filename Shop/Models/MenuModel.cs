using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class MenuItem
    {
        public long idCat{set; get;}
        public string name{set; get;}
        public string url{set; get;}

        public MenuItem()
        {
            
        }
    }

 
    public class MenuModel
    {
        public List<MenuItem> menu{ set; get; }
        public List<db.Entities.MenuItem> topMenuItems{ set; get; }

        public MenuModel()
        {
            menu=new List<MenuItem>();
            topMenuItems=new List<db.Entities.MenuItem>();
        }
    }
}