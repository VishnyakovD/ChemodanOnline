var CartItem = (function () {
    function CartItem(productId, count) {
        this.productId = productId;
        this.count = count;
    }
    return CartItem;
}());
var Cart = (function () {
    function Cart() {
        this.listProducts = [];
        this.listIds = [];
        this.dateFrom = new Date();
        this.dateTo = new Date();
        this.getCartCookie();
    }
    Cart.prototype.addToCart = function (id, maxCount) {
        var item = this.listProducts.filter(function (item) { return item.productId === id; });
        if (item.length > 0) {
            if (item[0].count < maxCount) {
                item[0].count += 1;
            }
            else {
                $("#ServerMessage").html("нельзя добавить");
            }
        }
        else {
            this.listProducts.push(new CartItem(id, 1));
        }
        this.setCartCookie();
        this.getCartCookie();
    };
    Cart.prototype.removeFromCart = function (id) {
        var item = this.listProducts.filter(function (item) { return item.productId === id; });
        if (item.length > 0) {
            item[0].count -= 1;
            if (item[0].count < 1) {
                this.listProducts = this.listProducts.filter(function (item) { return item.productId !== id; });
            }
        }
        this.setCartCookie();
        this.getCartCookie();
    };
    Cart.prototype.setCartCookie = function () {
        var curDate = new Date();
        curDate.setMinutes(curDate.getMinutes() + 60);
        $.cookie("cart", JSON.stringify(this), { expires: curDate, path: "/" });
    };
    Cart.prototype.getCartCookie = function () {
        if (!$.cookie("cart")) {
            return;
        }
        var cartCook = JSON.parse($.cookie("cart"));
        this.listProducts = cartCook.listProducts;
        this.dateFrom = cartCook.dateFrom;
        this.dateTo = cartCook.dateTo;
        console.log(this);
    };
    Cart.prototype.getProductIds = function () {
        if (this.listProducts.length < 1) {
            return [];
        }
        return this.listProducts.map(function (item) { return item.productId; });
    };
    return Cart;
}());
var cart;
$(function () {
    cart = new Cart();
    if ($(".js-addcart").length > 0) {
        $(document).on("click", ".js-addcart", function (e) {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            cart.addToCart(itemId, itemMax);
        });
    }
    if ($(".js-cart").length > 0) {
        $(document).on("click", ".js-cart", function (e) {
            window.location.href = $(e.currentTarget).find("div").data("href") + "?ids=" + cart.getProductIds();
        });
    }
});
