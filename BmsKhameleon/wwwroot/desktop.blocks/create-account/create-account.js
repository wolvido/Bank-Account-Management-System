$(function() {

    let originalHeight = $(".create-account__form-container").height();
    let formGroupHeight = $(".create-account__form-group").height();

    $(".create-account__form-group_joint").hide();
    $(".create-account__form-container").height(originalHeight - formGroupHeight);

    $(".create-account__form-input_account-type").on("change", function () {

        let accountType = $(".create-account__form-input_account-type").val();

        if (accountType === "Joint Account") {
            $(".create-account__form-group_joint").show();
            $(".create-account__form-container").height(originalHeight);
        }
        else {
            $(".create-account__form-group_joint").hide();
            $(".create-account__form-container").height(originalHeight - formGroupHeight);
        }
        
    });



});