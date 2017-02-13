var CartItem = (function () {
    function CartItem(ProductId, Count) {
        this.ProductId = ProductId;
        this.Count = Count;
    }
    return CartItem;
}());
var Cart = (function () {
    function Cart() {
        this.ListProducts = [];
        this.DateFrom = new Date();
        this.DateTo = new Date();
        this.GetCartCookie();
    }
    Cart.prototype.AddToCart = function (id) {
        var item = this.ListProducts.filter(function (item) { return item.ProductId === id; });
        if (item.length > 0) {
            item[0].Count += 1;
        }
        else {
            this.ListProducts.push(new CartItem(id, 1));
        }
        this.SetCartCookie();
        this.GetCartCookie();
    };
    Cart.prototype.SetCartCookie = function () {
        $.cookie("cart", JSON.stringify(this), { expires: 1800 });
    };
    Cart.prototype.GetCartCookie = function () {
        var cartCook = JSON.parse($.cookie("cart"));
        if (!cartCook) {
            return;
        }
        this.ListProducts = cartCook.ListProducts;
        this.DateFrom = cartCook.DateFrom;
        this.DateTo = cartCook.DateTo;
        console.log(this);
    };
    return Cart;
}());
var cart;
$(function () {
    if ($(".js-addcart").length > 0) {
        cart = new Cart();
        $(document).on("click", ".js-addcart", function (e) {
            var itemId = $(e.currentTarget).data("id");
            cart.AddToCart(itemId);
        });
    }
});
