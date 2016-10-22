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

    public interface IClientModelBuilder
    {
        ClientDataModel Build(long id);
    }

    public class ClientModelBuilder : MenuBuilder, IClientModelBuilder
    {
        public ClientModelBuilder(IDataService dataService, IImagesPath imagesPath)
            : base(dataService,imagesPath)
        {

        }

        public ClientDataModel Build(long id)
        {
           
            var model = new ClientDataModel();
            var modelDB = dataService.Get<Client>(id);
            model.client = modelDB != null ? modelDB : new Client();

            if (model.client.editAdress == null)
                model.client.editAdress = new EditAdress();

            if (model.client.sex == null)
                model.client.sex = new Sex();

            model.sexs= dataService.List<Sex>();
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }



    }
}