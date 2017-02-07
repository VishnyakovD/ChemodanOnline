using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Modules;

namespace Shop.Models.Builders
{


    public class MenuBuilder 
    {
        public IDataService dataService;
        public IImagesPath imagesPath;

        public MenuBuilder(IDataService dataService, IImagesPath imagesPath)
        {
            this.dataService = dataService;
            this.imagesPath = imagesPath;
        }

        public List<MenuItem> BuildMenu()
        {
       
              return dataService.List<StaticCategory>()
                    .Select(
                        cat =>
                            new MenuItem()
                            {
                                idCat = cat.id,
                                name = cat.name,
                                url =string.Empty /*new UrlHelper(new RequestContext()).Action("ListSkuByCategory", "Sku", new {idCat = cat.id})*/
                            }).ToList();
           
           
        }

        public List<MenuItem> BuildMenu(List<StaticCategory> listStaticCategory)
        {

            return listStaticCategory.Select(
                      cat =>
                          new MenuItem()
                          {
                              idCat = cat.id,
                              name = cat.name,
                              url = string.Empty
                          }).ToList();


        }

        public List<db.Entities.MenuItem> BuildTopMenu()
        {
            return dataService.ListMenuItem(1);
        }


        public MenuModel BuildAllMenu()
        {
            var menus = new MenuModel { menu = BuildMenu(), topMenuItems = dataService.ListMenuItem(1)};
            return menus;
        }


    }
}