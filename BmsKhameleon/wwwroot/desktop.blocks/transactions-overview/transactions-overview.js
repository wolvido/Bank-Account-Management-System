$(async function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };

    //deposit cash popup
    $(".transactions-overview__deposit-button").on("click", async function () {
        var response = await fetch(`/CreateDepositCashTransactionPartial`);
        var depositCashData = await response.text();
        $(".transactions-overview__deposit_overlay").html(depositCashData);

        //reset form validation
        $(".transactions-overview__deposit_overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__deposit_overlay form");
    });




});