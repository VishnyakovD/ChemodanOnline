var TopMenu = (function () {
    function TopMenu() {
        this.menu = $(".js-top-menu-body-mobile");
    }
    TopMenu.prototype.Show = function () {
        this.menu.toggleClass("toggled");
        // this.menu.slideDown("slow");
    };
    TopMenu.prototype.Hide = function () {
        this.menu.removeClass("toggled");
    };
    return TopMenu;
}());
var topMenu;
$(function () {
    topMenu = new TopMenu();
    $(document).on("click", ".js-click-top-menu-btn", function (e) {
        if (!topMenu.menu.hasClass("toggled")) {
            topMenu.Show();
        }
        else {
            topMenu.Hide();
        }
    });
});
//# sourceMappingURL=menu.js.map