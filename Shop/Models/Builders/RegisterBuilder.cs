using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;
using Shop.Modules;

namespace Shop.Models.Builders
{

    public interface IRegisterBuilder
    {
        RegisterModel Build();
    }

    public class RegisterBuilder : MenuBuilder, IRegisterBuilder
    {
        public RegisterBuilder(IDataService dataService, IImagesPath imagesPath)
            : base(dataService, imagesPath)
        {
        }

        public RegisterModel Build()
        {
            var model = new RegisterModel();
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }
    }
}