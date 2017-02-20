class RadioButton {

    setRadio() {
        
    }
}


$(() => {

    var radioButtons = $(".radio-btns");
    if (radioButtons.length>0) {
        $(document).on("click", ".js-set-ratio", (e) => {
            $(e.currentTarget).closest(".radio-btns").find(".selected").removeClass("selected");
            $(e.currentTarget).addClass("selected");
        });

    }
});