$(function () {
    //form control
    let hiddenForm = $(".transaction-form__fieldset_cheque");
    hiddenForm.detach();

    $(".transaction-form__control").on("click", function () {
        if ($(".transaction-form__radio-cash").is(":checked")) {

            hiddenForm.insertAfter(".transaction-form__control");

            hiddenForm = $(".transaction-form__fieldset_cheque");
            hiddenForm.detach();

            $(".transaction-form__notice-transaction-type strong").replaceWith("<strong>Cash</strong>");
        }
        else {
            hiddenForm.insertAfter(".transaction-form__control");

            hiddenForm = $(".transaction-form__fieldset_cash");
            hiddenForm.detach();

            $(".transaction-form__notice-transaction-type strong").replaceWith("<strong>Check</strong>");
        }
    });
});