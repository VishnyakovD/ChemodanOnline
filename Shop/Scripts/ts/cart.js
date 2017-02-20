var CartItem = (function () {
    function CartItem(productId, count, price) {
        this.productId = productId;
        this.count = count;
        this.price = price;
    }
    return CartItem;
}());
var Cart = (function () {
    function Cart() {
        this.listProducts = [];
        this.dateFrom = new Date();
        this.dateTo = new Date(2017, 1, 25);
        this.getCartCookie();
    }
    Cart.prototype.addToCart = function (id, maxCount, price) {
        var item = this.listProducts.filter(function (it) { return it.productId === id; });
        if (item.length > 0) {
            if (item[0].count < maxCount) {
                item[0].count += 1;
            }
            else {
                $("#ServerMessage").html("нельзя добавить");
            }
        }
        else {
            this.listProducts.push(new CartItem(id, 1, price));
        }
        this.setCartCookie();
        //this.getCartCookie();
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
        this.dateFrom = new Date(cartCook.dateFrom.toString());
        this.dateTo = new Date(cartCook.dateTo.toString());
        console.log(this);
    };
    Cart.prototype.getProductIds = function () {
        if (this.listProducts.length < 1) {
            return [];
        }
        return this.listProducts.map(function (item) { return item.productId; });
    };
    Cart.prototype.getDays = function () {
        var oneDay = 1000 * 60 * 60 * 24; // hours*minutes*seconds*milliseconds
        var t = this.dateTo.getTime();
        var f = this.dateFrom.getTime();
        var days = Math.round(Math.abs((t - f) / oneDay));
        return days;
    };
    return Cart;
}());
var CartManager = (function () {
    function CartManager() {
        this.cart = new Cart();
        this.cartCount = $(".js-count");
        this.cartSum = $(".js-order-summ");
    }
    CartManager.prototype.setCartSum = function () {
        var summ = 0;
        this.cart.listProducts.forEach(function (item) {
            summ += (item.count * item.price);
        });
        var days = this.cart.getDays();
        if (days > 0) {
            summ = summ * days;
        }
        this.cartSum.html(summ.toString());
    };
    CartManager.prototype.setProductsCount = function () {
        var summ = 0;
        this.cart.listProducts.forEach(function (item) {
            summ += item.count;
        });
        this.cartCount.html(summ.toString());
    };
    return CartManager;
}());
var cartManager;
$(function () {
    cartManager = new CartManager();
    if ($(".js-cart").length > 0) {
        cartManager.setProductsCount();
        $(document).on("click", ".js-cart", function (e) {
            window.location.href = $(e.currentTarget).find("div").data("href") + "?ids=" + cartManager.cart.getProductIds();
        });
    }
    if ($(".js-addcart").length > 0) {
        $(document).on("click", ".js-addcart", function (e) {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            var itemPrice = $(e.currentTarget).data("price");
            cartManager.cart.addToCart(itemId, itemMax, itemPrice);
            cartManager.setProductsCount();
        });
    }
    if ($(".js-order-summ").length) {
        cartManager.setCartSum();
    }
});
