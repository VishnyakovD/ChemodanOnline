using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shop.db.Entities;

namespace Shop.Models
{
    public class MainPage:MenuModel
    {
        public string Title { set; get; }
        public string TitleProduct { set; get; }
        public List<InfoBlockItem> ListBlockInfo { set; get; }
        public List<CaruselItem> ListCaruselItem { set; get; }

        public MainPage()
        {
            ListBlockInfo = new List<InfoBlockItem>();
            ListCaruselItem=new List<CaruselItem>();
        }


    
    }

    public class InfoBlockItemModel:InfoBlockItem
    {

    }

    public class CaruselItemModel: CaruselItem
    {

    }
}
