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
});
//# sourceMappingURL=controls.js.map