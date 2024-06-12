$(function () {

    let hiddenForm = $(".deposit__form-container_cheque");
    hiddenForm.detach();

    $(".deposit__control").on("click", function () {
        if ($(".deposit__control-radio_cash").is(":checked")) {

            hiddenForm.appendTo(".deposit__form");
            hiddenForm = $(".deposit__form-container_cheque");
            hiddenForm.detach();

            $(".deposit__notice-deposit-type strong").replaceWith("<strong>Cash</strong>")
        }
        else {
            hiddenForm.appendTo(".deposit__form");
            hiddenForm = $(".deposit__form-container_cash");
            hiddenForm.detach();

            $(".deposit__notice-deposit-type strong").replaceWith("<strong>Check</strong>")
        }
    });

});