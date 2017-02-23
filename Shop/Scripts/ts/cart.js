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
        this.dateTo = new Date();
        this.setDays();
        this.getCartCookie();
    }
    Cart.prototype.addToCart = function (id, maxCount, price) {
        var countResult;
        var item = this.listProducts.filter(function (it) { return it.productId === id; });
        if (item.length > 0) {
            if (item[0].count < maxCount) {
                item[0].count += 1;
                countResult = item[0].count;
            }
            else {
                countResult = item[0].count;
                $("#ServerMessage").html("нельзя добавить");
            }
        }
        else {
            this.listProducts.push(new CartItem(id, 1, price));
            countResult = 1;
        }
        this.setCartCookie();
        return countResult;
    };
    Cart.prototype.removeFromCart = function (id) {
        var countResult = 0;
        var item = this.listProducts.filter(function (item) { return item.productId === id; });
        if (item.length > 0) {
            item[0].count -= 1;
            countResult = item[0].count;
            if (item[0].count < 1) {
                this.listProducts = this.listProducts.filter(function (item) { return item.productId !== id; });
                countResult = 0;
            }
        }
        this.setCartCookie();
        return countResult;
    };
    Cart.prototype.setCartCookie = function () {
        var curDate = new Date();
        curDate.setMinutes(curDate.getMinutes() + 60);
        $.cookie("cart", JSON.stringify(this), { expires: curDate, path: "/" });
    };
    Cart.prototype.getCartCookie = function () {
        if (!$.cookie("cart"))
            return;
        var cartCook = JSON.parse($.cookie("cart"));
        this.listProducts = cartCook.listProducts;
        this.dateFrom = new Date(cartCook.dateFrom.toString());
        this.dateTo = new Date(cartCook.dateTo.toString());
    };
    Cart.prototype.getProductIds = function () {
        if (this.listProducts.length < 1) {
            return [];
        }
        return this.listProducts.map(function (item) { return item.productId; });
    };
    Cart.prototype.setDays = function () {
        var t = this.dateTo.getTime();
        var f = this.dateFrom.getTime();
        var days = Math.round(Math.abs((t - f) / 86400000));
        if (days < 1) {
            days = 1;
        }
        this.days = days;
    };
    return Cart;
}());
var CartManager = (function () {
    function CartManager() {
        this.cart = new Cart();
        this.cartCount = $(".js-count");
        this.cartSum = $(".js-order-summ");
    }
    CartManager.prototype.setCountOneProductInOrderPage = function (curent, count) {
        var countControl = curent.closest(".js-control-count");
        if (countControl.length > 0) {
            countControl.find(".js-count-item").html(count.toString());
        }
    };
    CartManager.prototype.setSummOneProductInOrderPage = function (id, price, count) {
        var page1 = $(".js-order-page1");
        if (page1.length > 0) {
            page1.find(".js-summ-item[data-id=" + id + "]").html((price * count * this.cart.days).toString());
        }
    };
    CartManager.prototype.setCountAllProducts = function () {
        var page1 = $(".js-order-page1");
        if (page1.length > 0) {
            this.cart.listProducts.forEach(function (item) {
                page1.find(".js-count-item[data-id=" + item.productId + "]").html(item.count.toString());
            });
        }
    };
    CartManager.prototype.setSummAllProducts = function () {
        var _this = this;
        var page1 = $(".js-order-page1");
        if (page1.length > 0) {
            this.cart.listProducts.forEach(function (item) {
                var summ = item.count * item.price * _this.cart.days;
                page1.find(".js-summ-item[data-id=" + item.productId + "]").html(summ.toString());
            });
        }
    };
    CartManager.prototype.setCartSum = function () {
        if (this.cartSum.length > 0) {
            var summ = 0;
            this.cart.listProducts.forEach(function (item) {
                summ += (item.count * item.price);
            });
            summ = summ * this.cart.days;
            this.cartSum.html(summ.toString());
        }
    };
    CartManager.prototype.setProductsCount = function () {
        if (this.cartCount.length > 0) {
            var summ = 0;
            this.cart.listProducts.forEach(function (item) {
                summ += item.count;
            });
            this.cartCount.html(summ.toString());
        }
    };
    return CartManager;
}());
var cartManager;
$(function () {
    cartManager = new CartManager();
    cartManager.setCountAllProducts();
    cartManager.setSummAllProducts();
    if ($(".js-cart").length > 0) {
        cartManager.setProductsCount();
        $(document).on("click", ".js-cart", function (e) {
            var location = $(e.currentTarget).find("div").data("href") + "?ids=" + cartManager.cart.getProductIds();
            window.location.href = location;
        });
    }
    if ($(".js-addcart").length > 0) {
        $(document).on("click", ".js-addcart", function (e) {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            var itemPrice = $(e.currentTarget).data("price");
            var count = cartManager.cart.addToCart(itemId, itemMax, itemPrice);
            cartManager.setProductsCount();
            cartManager.setCountOneProductInOrderPage($(e.currentTarget), count);
            cartManager.setSummOneProductInOrderPage(itemId, itemPrice, count);
            cartManager.setCartSum();
        });
    }
    if ($(".js-removecart").length > 0) {
        $(document).on("click", ".js-removecart", function (e) {
            var itemId = $(e.currentTarget).data("id");
            var itemPrice = $(e.currentTarget).data("price");
            var count = cartManager.cart.removeFromCart(itemId);
            cartManager.setProductsCount();
            cartManager.setCountOneProductInOrderPage($(e.currentTarget), count);
            cartManager.setSummOneProductInOrderPage(itemId, itemPrice, count);
            cartManager.setCartSum();
        });
    }
    if ($(".js-order-summ").length > 0) {
        cartManager.setCartSum();
    }
});
