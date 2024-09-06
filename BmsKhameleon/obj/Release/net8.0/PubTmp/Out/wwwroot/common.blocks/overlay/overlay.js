$(function () {

    $.fn.showFlex = function() {
        this.css('display', 'flex');
    }

    $('.overlay').on("click",'.overlay__cancel', function () {
        $('.overlay').hide();
    });

});