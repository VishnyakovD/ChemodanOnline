class AdminOrderManager {
    constructor() { }

    applyOrderOneClickevent(event:any, id: number): void {
        $.post("/Order/ApplyOneClickOrder/", { id: id })
            .done(result => {
                $(event.target).closest("tr").html(result);
            });
    }
}


var adminOrderManager: AdminOrderManager;

$(() => {
    adminOrderManager = new AdminOrderManager();
});
