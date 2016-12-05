class TopMenu {

    menu: JQuery;
    constructor() {
        this.menu = $(".js-top-menu-body-mobile");
    }

    show() {
        this.menu.toggleClass("toggled");
       // this.menu.slideDown("slow");
    }

    hide() {
        this.menu.removeClass("toggled");
    }
}

var topMenu: TopMenu;

$(() => {
    topMenu = new TopMenu();

    $(document).on("click", ".js-click-top-menu-btn", (e) => {
        if (!topMenu.menu.hasClass("toggled")) {
            topMenu.show();
        } else {
            topMenu.hide();  
        }
        console.log("dfdf");
    });
   

});