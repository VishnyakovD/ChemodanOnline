using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Shop.DataService;
using XMLParser.Classes;

namespace XMLParser
{
    public class XMLBuilder
    {
        public yml_catalog yml_catalog = new yml_catalog();
        public IDataService dataService;

        public XMLBuilder(IDataService iDataService)
        {
            yml_catalog=new yml_catalog();
            dataService = iDataService;
        }

        public string BuildXML()
        {
            var cats = dataService.ListStaticCategoryes();
            var offers = dataService.ListSku();

            if (cats != null && cats.Any())
            {
                yml_catalog.shop.category = cats.Select(c => new category() {id = c.id, name = c.name, parentId = 0}).ToList();
            }

            if (offers != null && offers.Any())
            {
                yml_catalog.shop.offers = offers.Select(c => new offer() {
                    id = c.id,
                    name = c.name,
                    available = c.isHide,
                    categoryId = c.listCategory.First().staticcat.id,
                    currencyId = "UAH",
                    delivery = true,
                    description = "<![CDATA[" + c.description + "]]>",
                    group_id = 0,
                    param = c.listSpecification.Where(i => i.staticspec.id == 10003).Select(st=> new param(){name = st.staticspec.name,unit =st.staticspec.id.ToString(),bodyText = st.value} ).First(),
                    picture = "http://newer.in.ua/Content/Images/" + c.smalPhoto.path,
                    price = c.priceAct,
                    url = "http://newer.in.ua/Home/SkuInfo?idSku="+c.id,
                    vendor = "Newer",
                    vendorCode = "Newer"

                }).ToList();
            }

            var xmlserializer = new XmlSerializer(typeof(yml_catalog));
            var stringWriter = new StringWriter();

            using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Encoding = Encoding.UTF8 }))
            {
                xmlserializer.Serialize(writer, yml_catalog);
                return stringWriter.ToString();
            }

        }

    }



}
