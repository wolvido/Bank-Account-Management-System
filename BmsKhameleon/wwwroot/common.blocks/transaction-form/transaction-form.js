$(function () {

    $.fn.showFlex = function() {
        this.css('display','flex');
    }

    if ($(".transaction-form__radio-cash").is(":checked")) {
        $(".transaction-form__fieldset_cheque").hide().find('input, textarea').prop('disabled', true);
    }


    $(".transaction-form__control").on("click", function () {

        if ($(".transaction-form__radio-cash").is(":checked")) {


            $(this).siblings(".transaction-form__fieldset_cheque").hide().find('input, textarea').prop('disabled', true);

            $(this).siblings(".transaction-form__fieldset_cash").find('input, textarea').prop('disabled', false);
            $(this).siblings(".transaction-form__fieldset_cash").showFlex();

            $(".transaction-form__notice-transaction-type strong").replaceWith("<strong>Cash</strong>");
        }
        else {

            $(this).siblings(".transaction-form__fieldset_cash").hide().find('input, textarea').prop('disabled', true);

            $(this).siblings(".transaction-form__fieldset_cheque").find('input, textarea').prop('disabled', false);
            $(this).siblings(".transaction-form__fieldset_cheque").showFlex();


            $(".transaction-form__notice-transaction-type strong").replaceWith("<strong>Check</strong>");
        }

    });
});