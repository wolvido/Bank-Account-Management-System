$(function () {
    $(document).on("change", ".form-group__date-input", function () {

        $(".form-group__date-display").text(
            moment($(this).val(), "YYYY-MM-DD").format($(this).attr("data-date-format"))
        )

    }).trigger("change");
});
