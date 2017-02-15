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
    Cart.prototype.AddToCart = function (id, maxCount) {
        var item = this.ListProducts.filter(function (item) { return item.ProductId === id; });
        if (item.length > 0) {
            if (item[0].Count < maxCount) {
                item[0].Count += 1;
            }
            else {
                $("#ServerMessage").html("нельзя добавить");
            }
        }
        else {
            this.ListProducts.push(new CartItem(id, 1));
        }
        this.SetCartCookie();
        this.GetCartCookie();
    };
    Cart.prototype.RemoveFromCart = function (id) {
        var item = this.ListProducts.filter(function (item) { return item.ProductId === id; });
        if (item.length > 0) {
            item[0].Count -= 1;
            if (item[0].Count < 1) {
                this.ListProducts = this.ListProducts.filter(function (item) { return item.ProductId !== id; });
            }
        }
        this.SetCartCookie();
        this.GetCartCookie();
    };
    Cart.prototype.SetCartCookie = function () {
        var curDate = new Date();
        curDate.setMinutes(curDate.getMinutes() + 20);
        $.cookie("cart", JSON.stringify(this), { expires: curDate });
    };
    Cart.prototype.GetCartCookie = function () {
        if (!$.cookie("cart")) {
            return;
        }
        var cartCook = JSON.parse($.cookie("cart"));
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
            var itemMax = $(e.currentTarget).data("max");
            cart.AddToCart(itemId, itemMax);
        });
    }
});
