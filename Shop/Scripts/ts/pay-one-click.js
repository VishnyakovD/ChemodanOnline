var PayOneClick = (function () {
    function PayOneClick() {
    }
    PayOneClick.prototype.payOneClick = function (e) {
        var phone = $(e.currentTarget).closest(".js-pay-one-click").find("input");
        if (phone.val() === "" || !phone.val().match(/^[0]{1}\d{9}$/)) {
            message.showMessage("Не верно указан номер телефона. Правильный формат : 0ХХ ХХХ ХХ ХХ", "new");
            return;
        }
        var productId = $(e.currentTarget).data("product-id");
        var date = new Date();
        var stringDate = date.toLocaleDateString() + " " + date.toLocaleTimeString();
        $.post("/Order/PayOneClick/", { phone: phone.val(), productId: productId, createDate: stringDate })
            .done(function (result) {
            phone.val("");
            $.removeCookie('cart', { path: '/' });
            message.showMessageWnd(result, "/");
        });
    };
    return PayOneClick;
}());
//# sourceMappingURL=pay-one-click.js.map