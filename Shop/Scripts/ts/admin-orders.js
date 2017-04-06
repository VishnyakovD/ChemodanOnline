var AdminOrderManager = (function () {
    function AdminOrderManager() {
    }
    AdminOrderManager.prototype.applyOrderOneClickevent = function (event, id) {
        $.post("/Order/ApplyOneClickOrder/", { id: id })
            .done(function (result) {
            $(event.target).closest("tr").html(result);
        });
    };
    return AdminOrderManager;
}());
var adminOrderManager;
$(function () {
    adminOrderManager = new AdminOrderManager();
});
//# sourceMappingURL=admin-orders.js.map