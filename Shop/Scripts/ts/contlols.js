var RadioButton = (function () {
    function RadioButton() {
    }
    RadioButton.prototype.setRadio = function () {
    };
    return RadioButton;
}());
$(function () {
    var radioButtons = $(".radio-btns");
    if (radioButtons.length > 0) {
        $(document).on("click", ".js-set-ratio", function (e) {
            $(e.currentTarget).closest(".radio-btns").removeClass("selected");
        });
    }
});
