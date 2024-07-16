$(function () {
    //custom input date design
	$(".form-group__input_date").on("change", function () {
    this.setAttribute(
        "data-date",
        moment(this.value, "YYYY-MM-DD")
        .format( this.getAttribute("data-date-format") )
    )
    }).trigger("change")
});
