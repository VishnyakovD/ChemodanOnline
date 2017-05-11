var InputErrorItem = (function () {
    function InputErrorItem(inputName, inputError) {
        this.inputName = inputName;
        this.inputError = inputError;
    }
    return InputErrorItem;
}());
var CartItem = (function () {
    function CartItem(productId, count, price, zalog) {
        this.productId = productId;
        this.count = count;
        this.price = price;
        this.zalog = zalog;
    }
    return CartItem;
}());
var Cart = (function () {
    function Cart() {
        var date = new Date();
        this.listProducts = [];
        this.from = new Date(date.setHours(0, 0, 0, 0));
        this.to = new Date(date.setHours(23, 59, 59, 99));
        this.setDays();
        this.getCartCookie();
    }
    Cart.prototype.addToCart = function (id, maxCount, price, zalog) {
        var countResult;
        var item = this.listProducts.filter(function (it) { return it.productId === id; });
        if (item.length > 0) {
            if (item[0].count < maxCount) {
                item[0].count += 1;
                countResult = item[0].count;
                message.showMessage("товар добавлен в корзину");
            }
            else {
                countResult = item[0].count;
                message.showMessage("нельзя добавить");
            }
        }
        else {
            this.listProducts.push(new CartItem(id, 1, price, zalog));
            message.showMessage("товар добавлен в корзину");
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
        if (cartCook.from != null) {
            this.from = new Date(cartCook.from.toString());
        }
        if (cartCook.to != null) {
            this.to = new Date(cartCook.to.toString());
        }
    };
    Cart.prototype.getProductIds = function () {
        if (this.listProducts.length < 1) {
            return [];
        }
        return this.listProducts.map(function (item) { return item.productId; });
    };
    Cart.prototype.setDays = function () {
        var t = this.to.getTime();
        var f = this.from.getTime();
        var days = Math.ceil(((t - f) / 86400000));
        if (days < 1) {
            days = 1;
        }
        this.days = days;
    };
    Cart.prototype.setDatesFromAndTo = function (from, to) {
        this.from = new Date(new Date(from).setHours(0, 0, 0, 0));
        this.to = new Date(new Date(to).setHours(23, 59, 59, 99));
        this.setDays();
        this.setCartCookie();
    };
    Cart.prototype.validate = function (datesOnly) {
        var errors = [];
        if (datesOnly) {
            if (this.from.toString() === "Invalid Date" || this.from.setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0)) {
                errors.push(new InputErrorItem("From", ""));
            }
            var dateFrom = new Date(this.from.toDateString());
            if (this.to.toString() === "Invalid Date" || this.to.setHours(23, 59, 59, 99) <= dateFrom.setHours(23, 59, 59, 99)) {
                errors.push(new InputErrorItem("To", ""));
            }
            return errors;
        }
        if (this.clientEmail === "" || !this.clientEmail.match(/^(([^<>=/*()\[\]\\.,;:\s@"]+(\.[^<>=/*()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)) {
            errors.push(new InputErrorItem("ClientEmail", ""));
        }
        if (this.clientFirstName === "") {
            errors.push(new InputErrorItem("ClientFirstName", ""));
        }
        if (this.clientLastName === "") {
            errors.push(new InputErrorItem("ClientLastName", ""));
        }
        if (this.clientPhone === "" || !this.clientPhone.match(/^[0]{1}\d{9}$/)) {
            errors.push(new InputErrorItem("ClientPhone", ""));
        }
        if (this.paymentType < 1) {
            errors.push(new InputErrorItem("PaymentType", ""));
        }
        if (this.deliveryType === 2 || this.deliveryType === 3) {
            //errors.push(new InputErrorItem("DeliveryType", ""));
            if (this.city === "") {
                errors.push(new InputErrorItem("City", ""));
            }
            if (this.flat === "") {
                errors.push(new InputErrorItem("Flat", ""));
            }
            if (this.home === "") {
                errors.push(new InputErrorItem("Home", ""));
            }
            if (this.typeStreet === "") {
                errors.push(new InputErrorItem("TypeStreet", ""));
            }
            if (this.level === "") {
                errors.push(new InputErrorItem("Level", ""));
            }
            if (this.street === "") {
                errors.push(new InputErrorItem("Street", ""));
            }
        }
        return errors;
    };
    return Cart;
}());
var CartManager = (function () {
    function CartManager() {
        this.cart = new Cart();
        this.cartCount = $(".js-count");
        this.cartSum = $(".js-order-summ");
        this.cartSumZalog = $(".js-order-summ-zalog");
        this.isReadContract = false;
        this.payOnlineItem = {};
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
    CartManager.prototype.setCartSumZalog = function () {
        if (this.cartSum.length > 0) {
            var summ = 0;
            this.cart.listProducts.forEach(function (item) {
                summ += (item.count * item.zalog);
            });
            this.cart.zalog = summ;
            this.cartSumZalog.html(summ.toString());
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
        this.cart.from = new Date(date.setHours(0, 0, 0, 0));
        this.cart.setDays();
        this.setCartSum();
        this.setCartSumZalog();
        this.setCountAllProducts();
        this.setSummAllProducts();
        //  $(".js-card-dates input[name=From]").val(`${this.cart.from.toDateString()}`);
    };
    CartManager.prototype.setDateTo = function (event) {
        var date = new Date(event.target.value);
        this.cart.to = new Date(date.setHours(23, 59, 59, 99));
        this.cart.setDays();
        this.setCartSum();
        this.setCartSumZalog();
        this.setCountAllProducts();
        this.setSummAllProducts();
        //  $(".js-card-dates input[name=To]").val(this.cart.to.toString());
    };
    CartManager.prototype.validateCart = function (datesOnly) {
        var errors = this.cart.validate(datesOnly);
        var orderPages = $(".js-order-pages");
        orderPages.find(".error").removeClass("error");
        errors.forEach(function (item) {
            orderPages.find("[name=" + item.inputName + "]").addClass("error");
        });
        if (errors.length > 0) {
            orderPages.find(".error").first().focus();
            return false;
        }
        return true;
    };
    CartManager.prototype.visibilityAddres = function (visible) {
        var orderPage = $(".js-order-pages");
        if (visible) {
            orderPage.find(".js-addres").removeClass("hide");
        }
        else {
            orderPage.find(".js-addres").addClass("hide");
        }
    };
    CartManager.prototype.setReadContract = function (event) {
        this.isReadContract = $(event.currentTarget).prop("checked");
        if (this.isReadContract) {
            $(".js-confirm-order").removeClass("disabled");
        }
        else {
            $(".js-confirm-order").addClass("disabled");
        }
    };
    CartManager.prototype.pay = function (elem) {
        var payOnline = new Wayforpay();
        if (elem !== null) {
            cartManager.payOnlineItem = JSON.parse(elem);
        }
        payOnline.run({
            merchantAccount: cartManager.payOnlineItem.merchantAccount,
            merchantDomainName: cartManager.payOnlineItem.merchantDomainName,
            authorizationType: cartManager.payOnlineItem.authorizationType,
            merchantSignature: cartManager.payOnlineItem.merchantSignature,
            orderReference: cartManager.payOnlineItem.orderReference,
            orderDate: cartManager.payOnlineItem.orderDate,
            amount: cartManager.payOnlineItem.amount,
            currency: cartManager.payOnlineItem.currency,
            productName: cartManager.payOnlineItem.productName,
            productPrice: cartManager.payOnlineItem.productPrice,
            productCount: cartManager.payOnlineItem.productCount,
            clientFirstName: cartManager.payOnlineItem.clientFirstName,
            clientLastName: cartManager.payOnlineItem.clientLastName,
            clientEmail: cartManager.payOnlineItem.clientEmail,
            clientPhone: cartManager.payOnlineItem.clientPhone,
            holdTimeout: cartManager.payOnlineItem.holdTimeout,
            merchantTransactionType: cartManager.payOnlineItem.merchantTransactionType,
            paymentSystems: "card",
            language: "RU"
        }, function (response) {
            if (response.reasonCode == 1100) {
                var date = new Date();
                var stringDate = date.toLocaleDateString() + " " + date.toLocaleTimeString();
                $.post("/Order/PaidOrder/", { num: cartManager.payOnlineItem.orderReference, paymentId: response.authCode, date: stringDate })
                    .done(function (result) {
                    console.log("order paid : " + result);
                    message.showMessageWnd("Заказ создан " + cartManager.payOnlineItem.orderReference, "/");
                    $(".js-order-pages").html("");
                    $(".js-order-pages").css("height", "50vh");
                });
            }
        }, function (response) {
            console.log("declined");
            console.log(response);
        }, function (response) {
            console.log("pending or in processing");
            console.log(response);
        });
    };
    return CartManager;
}());
var cartManager;
var orderPage1;
var orderPage2;
$(function () {
    cartManager = new CartManager();
    if ($(".js-cart").length > 0) {
        cartManager.setProductsCount();
        $(document).on("click", ".js-cart", function (e) {
            if (cartManager.cart.listProducts.length > 0) {
                window.location.href = $(e.currentTarget).find("div").data("href") + "?ids=" + cartManager.cart.getProductIds();
            }
            else {
                message.showMessage("ваша корзина пуста");
            }
        });
    }
    if ($(".js-addcart").length > 0) {
        $(document).on("click", ".js-addcart", function (e) {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            var itemPrice = $(e.currentTarget).data("price");
            var itemZalog = $(e.currentTarget).data("zalog");
            var count = cartManager.cart.addToCart(itemId, itemMax, itemPrice, itemZalog);
            cartManager.setProductsCount();
            cartManager.setCountOneProductInOrderPage($(e.currentTarget), count);
            cartManager.setSummOneProductInOrderPage(itemId, itemPrice, count);
            cartManager.setCartSum();
            cartManager.setCartSumZalog();
        });
    }
    if ($(".js-removecart").length > 0) {
        $(document).on("click", ".js-removecart", function (e) {
            var itemId = $(e.currentTarget).data("id");
            var itemPrice = $(e.currentTarget).data("price");
            var itemRemoveAll = $(e.currentTarget).data("remove-all");
            var count = 0;
            if (itemRemoveAll) {
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
            cartManager.setCartSumZalog();
        });
    }
    if ($(".js-confirm-order").length > 0) {
        $(document).on("click", ".js-confirm-order", function (e) {
            var orderPage = $(".js-order-pages");
            if (cartManager.isReadContract === false) {
                return;
            }
            var date = new Date();
            cartManager.cart.clientLastName = orderPage.find("[name=ClientLastName]").val();
            cartManager.cart.clientFirstName = orderPage.find("[name=ClientFirstName]").val();
            cartManager.cart.clientEmail = orderPage.find("[name=ClientEmail]").val();
            cartManager.cart.clientPhone = orderPage.find("[name=ClientPhone]").val();
            cartManager.cart.city = orderPage.find("[name=City]").val();
            cartManager.cart.home = orderPage.find("[name=Home]").val();
            cartManager.cart.typeStreet = orderPage.find("[name=TypeStreet]").val();
            cartManager.cart.level = orderPage.find("[name=Level]").val();
            cartManager.cart.street = orderPage.find("[name=Street]").val();
            cartManager.cart.flat = orderPage.find("[name=Flat]").val();
            cartManager.cart.deliveryType = parseInt(orderPage.find("[name=DeliveryType]").val());
            cartManager.cart.paymentType = parseInt(orderPage.find("[name=PaymentType]").val());
            cartManager.cart.createDate = date.toLocaleDateString() + " " + date.toLocaleTimeString();
            if (cartManager.validateCart(false)) {
                var orderPages = $(".js-order-pages");
                //
                $.post("/Order/CreateOrder/", { order: JSON.stringify(cartManager.cart) })
                    .done(function (result) {
                    $.removeCookie('cart', { path: '/' });
                    if (cartManager.cart.paymentType !== 2) {
                        message.showMessageWnd(result, "/");
                        orderPages.html("");
                    }
                    else {
                        orderPages.find(".js-confirm-order").remove();
                        orderPages.find(".js-repay-order").removeClass("hide");
                        cartManager.pay(result);
                    }
                });
            }
        });
    }
    if ($(".js-delivery-type").length > 0) {
        $(document).on("click", ".js-delivery-type", function (e) {
            if ($(e.currentTarget).data("id") === 2 || $(e.currentTarget).data("id") === 3) {
                cartManager.visibilityAddres(true);
            }
            else {
                cartManager.visibilityAddres(false);
            }
        });
    }
    if ($(".js-card-dates").length > 0) {
        var dates = $(".js-card-dates");
        cartManager.cart.setDatesFromAndTo(dates.find("input[name=From]").val(), dates.find("input[name=To]").val());
    }
    if ($(".js-order-summ").length > 0) {
        cartManager.setCartSum();
        cartManager.setCartSumZalog();
    }
    cartManager.setCountAllProducts();
    cartManager.setSummAllProducts();
    orderPage1 = $(".js-order-page1");
    if (orderPage1.length > 0) {
        orderPage2 = $(".js-order-page2");
        $(document).on("click", ".js-pre-confirm-order", function (e) {
            if (cartManager.validateCart(true)) {
                orderPage1.addClass("hide");
                orderPage2.removeClass("hide");
            }
        });
        $(document).on("click", ".js-repay-order", function (e) {
            $.post("/Order/RepayOrder/", { order: cartManager.payOnlineItem.id })
                .done(function (result) { cartManager.pay(result); });
        });
    }
});
//# sourceMappingURL=cart.js.map