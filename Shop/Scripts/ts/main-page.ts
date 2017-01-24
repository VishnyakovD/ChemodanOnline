﻿class MainPage {
    caruselItem: JQuery;
    infoBlockItem: JQuery;

    constructor() {
        this.caruselItem = $(".js-carusel-item");
        this.infoBlockItem = $(".js-info-block-item");
    }

    GetInfoBlock(id:number, type:number):void {
        $.post('/Admin/ShowInfoBlockItem/', { id: id, type: type }).done((data) => {

            switch (type) {
                case 0:
                    this.caruselItem.html(data);
                    break;
                case 1:
                    this.infoBlockItem.html(data);
                    break;
            }
            
        });
    }
}

var mainPage: MainPage;

$(() => {
    mainPage = new MainPage();

    $(document).on("click", ".js-show-info-block-item", (e) => {
        mainPage.GetInfoBlock($(e.currentTarget).data("id"), $(e.currentTarget).data("type"));
    });



});