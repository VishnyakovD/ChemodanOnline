var TopMenu = (function () {
    function TopMenu() {
        this.menu = $(".js-top-menu-body-mobile");
    }
    TopMenu.prototype.show = function () {
        this.menu.toggleClass("toggled");
        // this.menu.slideDown("slow");
    };
    TopMenu.prototype.hide = function () {
        this.menu.removeClass("toggled");
    };
    return TopMenu;
}());
var topMenu;
$(function () {
    topMenu = new TopMenu();
    $(document).on("click", ".js-click-top-menu-btn", function (e) {
        if (!topMenu.menu.hasClass("toggled")) {
            topMenu.show();
        }
        else {
            topMenu.hide();
        }
        console.log("dfdf");
    });
});
//# sourceMappingURL=menu.js.map