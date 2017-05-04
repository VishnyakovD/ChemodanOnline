$(function () {
    if ($(".js-tabs-block").length > 0) {
        $(document).on("click", ".js-product-tab-title", function (e) {
            var tabBlock = $(e.currentTarget).closest(".js-tabs-block");
            tabBlock.find(".js-product-tab-title").removeClass("active");
            $(e.currentTarget).addClass("active");
            tabBlock.find(".js-product-tab-content").addClass("hide");
            tabBlock.find(".js-product-tab-content[data-id=\"" + $(e.currentTarget).data("id") + "\"]").removeClass("hide");
        });
    }
});
//# sourceMappingURL=card.js.map