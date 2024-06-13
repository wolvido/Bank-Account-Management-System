$(function () {

    let hiddenForm = $(".withdraw__form-container_cash");
    hiddenForm.detach();

    $(".withdraw__control").on("click", function () {
        if ($(".withdraw__control-radio_cheque").is(":checked")) {

            hiddenForm.appendTo(".withdraw__form");
            hiddenForm = $(".withdraw__form-container_cash");
            hiddenForm.detach();



            $(".withdraw__notice-message").replaceWith("<span class='withdraw__notice-message'>You are about to issue a Check — dated on </span>");

        }
        else {
            hiddenForm.appendTo(".withdraw__form");
            hiddenForm = $(".withdraw__form-container_cheque");
            hiddenForm.detach();

            $(".withdraw__notice-message").replaceWith( "<span class='withdraw__notice-message'>You are about to record a withdrawal of Cash on </span>");

        }
    });

});