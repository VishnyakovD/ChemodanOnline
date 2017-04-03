using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;
using Shop.Modules;

namespace Shop.Models.Builders
{
    public interface IAdminModelBuilder
    {
        AdminModel Build();
    }

    public class AdminModelBuilder : MenuBuilder,IAdminModelBuilder
    {
        public AdminModelBuilder(IDataService dataService, IImagesPath imagesPath):base(dataService,imagesPath)
        {
            
        }
        public AdminModel Build()
        {
            var model = new AdminModel();
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();

            return model;
        }

    }
}