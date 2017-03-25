var PayOneClick = (function () {
    function PayOneClick() {
    }
    PayOneClick.prototype.payOneClick = function (e) {
        var phone = $(e.currentTarget).closest(".js-pay-one-click").find("input");
        var productId = $(e.currentTarget).data("product-id");
        var date = new Date();
        var epochTicks = 621355968000000000;
        var ticksPerMillisecond = 10000;
        var datetime = epochTicks + (date.getTime() * ticksPerMillisecond);
        $.post("/Order/PayOneClick/", { phone: phone.val(), productId: productId, createDate: datetime })
            .done(function (result) {
            message.showMessage(result);
            phone.val("");
            //если заказ создан нужно почистить куки и показать всплывающее окно с крестиком
        });
    };
    return PayOneClick;
}());
var payOneClick;
$(function () {
    if ($(".js-pay-one-click").length > 0) {
        payOneClick = new PayOneClick();
    }
});
//# sourceMappingURL=pay-one-click.js.map