$(function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    }

    $(document).on("click", ".popup__show-delete-button", function () {
        $(".popup_modify").hide();
        $(".popup_delete").showFlex();
    });

    $(".popup_delete").find("*button").on("click", function () {

        this.hide();

    });

});