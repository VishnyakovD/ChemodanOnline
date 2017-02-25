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
        var date = new Date();
        this.listProducts = [];
        this.dateFrom = new Date(date.setHours(0, 0, 0, 0));
        this.dateTo = new Date(date.setHours(23, 59, 59, 99));
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
                $("#ServerMessage").html("товар добавлен в корзину");
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
    Cart.prototype.removeFromCart = function (id, removeAll) {
        var countResult = 0;
        var item = this.listProducts.filter(function (item) { return item.productId === id; });
        if (item.length > 0) {
            item[0].count -= 1;
            countResult = item[0].count;
            if (removeAll || item[0].count < 1) {
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
        var days = Math.ceil(((t - f) / 86400000));
        if (days < 1) {
            days = 1;
        }
        this.days = days;
    };
    Cart.prototype.setDatesFromAndTo = function (from, to) {
        this.dateFrom = new Date(new Date(from).setHours(0, 0, 0, 0));
        this.dateTo = new Date(new Date(to).setHours(23, 59, 59, 99));
        this.setDays();
        this.setCartCookie();
    };
    return Cart;
}());
var CartManager = (function () {
    function CartManager() {
        this.cart = new Cart();
        this.cartCount = $(".js-count");
        this.cartSum = $(".js-order-summ");
    }
    CartManager.prototype.removeCartLineInOrderPage = function (curent) {
        var line = curent.closest(".js-cart-line");
        if (line.length > 0) {
            line.remove();
        }
    };
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
    CartManager.prototype.setDateFrom = function (event) {
        var date = new Date(event.target.value);
        this.cart.dateFrom = new Date(date.setHours(0, 0, 0, 0));
        this.cart.setDays();
        this.setCartSum();
        this.setCountAllProducts();
        this.setSummAllProducts();
    };
    CartManager.prototype.setDateTo = function (event) {
        var date = new Date(event.target.value);
        this.cart.dateTo = new Date(date.setHours(23, 59, 59, 99));
        this.cart.setDays();
        this.setCartSum();
        this.setCountAllProducts();
        this.setSummAllProducts();
    };
    return CartManager;
}());
var cartManager;
$(function () {
    cartManager = new CartManager();
    if ($(".js-cart").length > 0) {
        cartManager.setProductsCount();
        $(document).on("click", ".js-cart", function (e) {
            if (cartManager.cart.listProducts.length > 0) {
                window.location.href = $(e.currentTarget).find("div").data("href") + "?ids=" + cartManager.cart.getProductIds();
            }
            else {
                $("#ServerMessage").html("ваша корзина пуста");
            }
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
            var itemRemoveAll = $(e.currentTarget).data("remove-all");
            console.log(itemRemoveAll);
            var count = 0;
            if (itemRemoveAll == "true") {
                count = cartManager.cart.removeFromCart(itemId, true);
            }
            else {
                count = cartManager.cart.removeFromCart(itemId, false);
            }
            if (count === 0) {
                cartManager.removeCartLineInOrderPage($(e.currentTarget));
            }
            cartManager.setProductsCount();
            cartManager.setCountOneProductInOrderPage($(e.currentTarget), count);
            cartManager.setSummOneProductInOrderPage(itemId, itemPrice, count);
            cartManager.setCartSum();
        });
    }
    if ($(".js-confirm-order").length > 0) {
        $(document).on("click", ".js-confirm-order", function (e) {
            //ClientLastName
            //ClientFirstName
            //ClientEmail
            //ClientPhone
            //City
            //Home
            //TypeStreet
            //Level
            //Street
            //Flat
            //DeliveryType
            //PaymentType
            //
            //
            //
        });
    }
    if ($(".js-card-dates").length > 0) {
        var dates = $(".js-card-dates");
        cartManager.cart.setDatesFromAndTo(dates.find("input[name=From]").val(), dates.find("input[name=To]").val());
    }
    if ($(".js-order-summ").length > 0) {
        cartManager.setCartSum();
    }
    cartManager.setCountAllProducts();
    cartManager.setSummAllProducts();
});
//# sourceMappingURL=cart.js.map