using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Modules;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{

    public interface IMainPageBuilder
    {
        MainPageModel Build();
    }

    public class MainPageBuilder : MenuBuilder, IMainPageBuilder
    {
        private IImagesPath imagesPath;
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        public MainPageBuilder(IDataService dataService, IImagesPath imagesPath)
            : base(dataService, imagesPath)
        {
            this.imagesPath = imagesPath;
        }



        public MainPageModel Build()
        {
            var model = new MainPageModel();

            model.Title = "Какой чемодан вы бы хотели арендавать?";
            model.TitleProduct = "Самые востребованные";
            model.ListCaruselItem = dataService.ListInfoBlockItems(DisplayType.MainCarusel);
            model.ListBlockInfo = dataService.ListInfoBlockItems(DisplayType.MainInfoBlock);
           
            model.topMenuItems = BuildTopMenu();
            return model;
        }

    }
}