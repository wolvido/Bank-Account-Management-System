$(async function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };

    let overlayEdit = $(".accounts__overlay_modify");

    let overlayCreate = $(".accounts__overlay_create");

    let editForm = $(".accounts__modify");

    $(".accounts__item-button_edit").on("click", async function () {

        overlayEdit.showFlex();
        editForm.showFlex();

        editForm.find(".accounts__button_delete").show();
    });

    //add account
    $(".accounts__add-accounts").on("click", async function () {

        overlayCreate.showFlex();
        editForm.showFlex();

        editForm.find(".accounts__button_delete").hide();
    });

});