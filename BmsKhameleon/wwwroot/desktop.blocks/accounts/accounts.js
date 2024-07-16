$(async function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };

    let overlayEdit = $(".accounts__overlay_modify");

    let editForm = $(".accounts__modify");

    let dateToday = new Date().toDateString().slice(4, 20);

    $(".accounts__item-button_edit").on("click", async function () {

        $(".accounts__modify-header").text("Modify Bank Account");

        overlayEdit.showFlex();
        editForm.showFlex();

        $(".accounts__modify-label_balance").text("Modify Initial Balance (Enrolled "+dateToday+")");

        let bankName = $(this).closest(".accounts__item").find(".accounts__item-bank").text();
        let accountNumber = $(this).closest(".accounts__item").find(".accounts__item-account").text();

        editForm.find(".accounts__modify-bank").val(bankName);
        editForm.find(".accounts__modify-account-number").val(accountNumber);
        editForm.find(".accounts__button_delete").show();

    });

    //add account
    $(".accounts__add-accounts").on("click", async function () {

        $(".accounts__modify-header").text("Add Bank Account");
        $(".accounts__modify-label_balance").text("Initial Balance to Enroll (" + dateToday + ")");

        overlayEdit.showFlex();
        editForm.showFlex();

        //clear all input fields when adding new account
        editForm.find(".accounts__modify-bank").val("");
        editForm.find(".accounts__modify-account-number").val("");
        editForm.find(".accounts__modify-balance").val("");
        editForm.find(".accounts__modify-form-group_joint").hide();
        editForm.find(".accounts__modify-payor").val("");
        editForm.find(".accounts__modify-branch").val("");

        editForm.find(".accounts__button_delete").hide();
    });

    //$(".accounts__modify-account-type").on("change", function () {

    //    if ($(this).val() == "Joint Account") {
    //        $(".accounts__modify-form-group_joint").showFlex();
    //    }
    //    else {
    //        $(".accounts__modify-form-group_joint").hide();
    //    }

    //});


});