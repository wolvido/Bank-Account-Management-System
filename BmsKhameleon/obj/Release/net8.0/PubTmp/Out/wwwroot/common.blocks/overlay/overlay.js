$(function () {

    $.fn.showFlex = function() {
        this.css('display', 'flex');
    }

    $('.overlay').on("click",'.overlay__cancel', function () {
        $('.overlay').hide();
    });

    //prevent double submission on submit
    $(document).on("submit","form", function () {
        $(this).find('input[type="submit"]').prop('disabled', true);
    });

});