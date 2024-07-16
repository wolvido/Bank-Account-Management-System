$(function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    }

    let deleteForm = $(".popup_delete");

    let editForm = $(".popup_modify");

    $(".popup__show-delete-button").on("click", async function () {

        editForm.hide();
        deleteForm.showFlex();
    });

    deleteForm.find("*button").on("click", function () {
        deleteForm.hide();
    });



});