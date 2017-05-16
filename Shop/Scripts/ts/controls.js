$(function () {
    var radioButtons = $(".radio-btns");
    if (radioButtons.length > 0) {
        $(document).on("click", ".js-set-ratio", function (e) {
            var current = $(e.currentTarget);
            var radioBtns = current.closest(".radio-btns");
            radioBtns.find(".selected").removeClass("selected");
            current.addClass("selected");
            radioBtns.find("input").val(current.data("id"));
        });
    }
    var closePanelButtons = $(".js-close-panel-admin");
    if (closePanelButtons.length > 0) {
        $(document).on("click", ".js-close-panel-admin", function (e) {
            var current = $(e.currentTarget);
            var pan = current.closest(".js-panel");
            pan.find(".js-panel-closed").addClass("panel-closed");
        });
    }
});
function showPanel(event) {
    var current = $(event.currentTarget);
    var pan = current.closest(".js-panel");
    pan.find(".js-panel-closed").removeClass("panel-closed");
}
