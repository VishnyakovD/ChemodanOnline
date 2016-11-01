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

    public interface ICartBuilder
    {
        CartModel Build(Cart sessionCart);
        ListCartsModel BuildListCarts(List<Cart> carts, DateTime start, DateTime end, int stateValue);

    }

    public class CartBuilder : MenuBuilder, ICartBuilder
    {
        private IImagesPath imagesPath;
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        public CartBuilder(IDataService dataService, IAccountAdminModelBuilder iAccountAdminModelBuilder, IImagesPath imagesPath)
            : base(dataService,imagesPath)
        {
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
            this.imagesPath = imagesPath;
        }

        public CartModel Build(Cart sessionCart)
        {
            var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
            var model = new CartModel();
            model.cart = sessionCart;
            foreach (var cartItem in sessionCart.listSku)
            {
                var sku = dataService.GetSkuById(cartItem.idSku);
                sku.smalPhoto.path = string.Format("{0}/{1}", imagesPath.GetImagesPath(), sku.smalPhoto.path);
                    model.listSku.Add(sku);                
            }

            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            model.listState = dataService.ListCartState();
            return model;
        }

        public ListCartsModel BuildListCarts(List<Cart> carts, DateTime start, DateTime end, int stateValue)
        {
            var model=new ListCartsModel();
            var list = new List<CartModel>();

            foreach (var cart in carts)
            {
                list.Add(new CartModel() { cart = cart, listSku = new List<Sku>() });
                foreach (var cartItem in cart.listSku)
                {
                    var sku = dataService.GetSkuById(cartItem.idSku);
                    sku.price = cartItem.price;
                    sku.priceAct = cartItem.priceAct;
                    list.Last().listSku.Add(sku);
                }
            }
            model.startDate = start;
            model.endDate = end;
            model.listState = dataService.ListCartState();
            model.state = model.listState.First(it => it.value == stateValue);
            model.listCarts = list;
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }



    }
}