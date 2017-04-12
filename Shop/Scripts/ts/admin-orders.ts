class AdminOrderManager {
    constructor() { }

    applyOrderOneClickevent(event:any, id: number): void {
        $.post("/Order/ApplyOneClickOrder/", { id: id })
            .done(result => {
                $(event.target).closest("tr").html(result);
            });
    }

    showExistProducts(productId: number, dbId: number, orderId: number): void {

        $.post("/Order/ShowChemodanTracking/", { productId: productId })
            .done(result => {
                var text = `
<div class="js-order-parameters" data-id="${dbId}" data-orderid="${orderId}"></div>
                            `;
                message.showMessageWnd(text+result,null);
            });
    }

    saveCodeToOrder(event: any, code: string): void {
        var ordParams = $(event.target).closest(".js-message").find(".js-order-parameters");
        $(event.target).closest("table").find("td").removeClass("success");
        var dbId = ordParams.data("id");
        var orderId = ordParams.data("orderid");
        $.post("/Order/SaveProductToOrder/", { dbId: dbId, code: code, orderId: orderId })
            .done(result => {
                $(event.target).addClass("success");
                $(".js-edit-order-body").html(result);
            });
    }
}


var adminOrderManager: AdminOrderManager;

$(() => {
    adminOrderManager = new AdminOrderManager();
});
