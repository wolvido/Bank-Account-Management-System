$(function () {

    $.fn.showFlex = function() {
        this.css('display', 'flex');
    }

    $('.overlay__cancel').on("click", function () {
        $('.overlay').hide();
    });

});