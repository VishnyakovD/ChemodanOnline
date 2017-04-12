var AdminOrderManager = (function () {
    function AdminOrderManager() {
    }
    AdminOrderManager.prototype.applyOrderOneClickevent = function (event, id) {
        $.post("/Order/ApplyOneClickOrder/", { id: id })
            .done(function (result) {
            $(event.target).closest("tr").html(result);
        });
    };
    AdminOrderManager.prototype.showExistProducts = function (productId, dbId, orderId) {
        $.post("/Order/ShowChemodanTracking/", { productId: productId })
            .done(function (result) {
            var text = "\n<div class=\"js-order-parameters\" data-id=\"" + dbId + "\" data-orderid=\"" + orderId + "\"></div>\n                            ";
            message.showMessageWnd(text + result, null);
        });
    };
    AdminOrderManager.prototype.saveCodeToOrder = function (event, code) {
        var ordParams = $(event.target).closest(".js-message").find(".js-order-parameters");
        $(event.target).closest("table").find("td").removeClass("success");
        var dbId = ordParams.data("id");
        var orderId = ordParams.data("orderid");
        $.post("/Order/SaveProductToOrder/", { dbId: dbId, code: code, orderId: orderId })
            .done(function (result) {
            $(event.target).addClass("success");
            $(".js-edit-order-body").html(result);
        });
    };
    return AdminOrderManager;
}());
var adminOrderManager;
$(function () {
    adminOrderManager = new AdminOrderManager();
});
//# sourceMappingURL=admin-orders.js.map