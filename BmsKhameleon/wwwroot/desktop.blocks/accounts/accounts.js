$(async function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };

    let overlayEdit = $(".accounts__overlay_modify");
    let overlayCreate = $(".accounts__overlay_create");
    let editForm = $(".accounts__modify");

    //update account
    $(".accounts__item-button_edit").on("click", async function () {
        var accountId = this.getAttribute('data-accountid');

        //fetch and apply the partial view
        var response = await fetch(`/UpdateBankAccountPartial/${accountId}`);
        var updateAccountData = await response.text();
        $(".accounts__overlay_modify").html(updateAccountData);

        //reset form validation
        $(".accounts__overlay_modify form").data("validator", null);
        $.validator.unobtrusive.parse(".accounts__overlay_modify form");

        overlayEdit.showFlex();
        editForm.showFlex();

        editForm.find(".accounts__button_delete").show();
    });

    //update account button prevent default
    $(".accounts__item-button").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
    });

    //add account
    $(".accounts__add-accounts").on("click", async function () {
        overlayCreate.showFlex();
        editForm.showFlex();

        editForm.find(".accounts__button_delete").hide();
    });

    //delete account
    $(document).on("click", ".accounts__button_delete", async function () {
        $(".popup_modify").hide();
        $(".popup_delete").showFlex();
    });

});