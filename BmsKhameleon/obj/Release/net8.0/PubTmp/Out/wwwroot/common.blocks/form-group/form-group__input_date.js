$(function () {
    //custom input date design
    $(document).on("change", ".form-group__input_date", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-MM-DD")
                .format(this.getAttribute("data-date-format"))
        )

    }).trigger("change");
});
