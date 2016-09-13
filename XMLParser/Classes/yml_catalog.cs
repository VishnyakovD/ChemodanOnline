using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLParser.Classes
{
   public class yml_catalog
    {
        public shop shop { get; set; }
       [XmlAttribute]
        public string catalog_date { get; set; }

        public yml_catalog()
        {
            shop=new shop();
            catalog_date = DateTime.Now.ToString();
        }
    }

    public class shop
    {
        public List<category> category { get; set; }
        public List<offer> offers { get; set; }

        public shop()
        {
            category = new List<category>();
            offers = new List<offer>();
        }
    }

    public class category
    {
        [XmlAttribute]
        public long id { get; set; }
        [XmlAttribute]
        public long parentId { get; set; }
        [XmlText]
        public string name { get; set; }
    }

    public class offer
    {
        [XmlAttribute]
        public bool available { get; set; }
        [XmlAttribute]
        public long group_id { get; set; }
        [XmlAttribute]
        public long id { get; set; }
        public string url { get; set; }
        public decimal price { get; set; }
        public string currencyId { get; set; }
        public long categoryId { get; set; }
        public string picture { get; set; }
        public bool delivery { get; set; }
        public string name { get; set; }
        public string vendorCode { get; set; }
        public string description { get; set; }
        public param param { get; set; }
        public string vendor { get; set; }

        public offer()
        {
            param=new param();
        }
        

    }

    public class param 
    {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string unit { get; set; }
         [XmlText]
        public string bodyText { get; set; }
        

    }


}
