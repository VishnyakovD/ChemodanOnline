using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shop.db.Entities;

namespace Shop.Models
{
    public class MainPageModel : MenuModel
    {
        public string Title { set; get; }
        public string TitleProduct { set; get; }
        public List<InfoBlockItem> ListBlockInfo { set; get; }
        public List<InfoBlockItem> ListCaruselItem { set; get; }
        public List<ShortSKUModel> ListProduct { set; get; }

        public MainPageModel()
        {
            ListBlockInfo = new List<InfoBlockItem>();
            ListCaruselItem=new List<InfoBlockItem>();
            ListProduct = new List<ShortSKUModel>();
        }


    
    }


}
