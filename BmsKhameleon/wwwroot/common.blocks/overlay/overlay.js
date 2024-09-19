$(function () {

    $.fn.showFlex = function() {
        this.css('display', 'flex');
    }

    $('.overlay').on("click",'.overlay__cancel', function () {
        $('.overlay').hide();
    });

    //close overlay on submission
    $(document).on("submit","form", function () {
        $('.overlay').hide();
    });

});