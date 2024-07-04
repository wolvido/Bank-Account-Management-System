$(async function() {

    $(".accounts__item-button_edit").on("click", async function () {

        $(".overlay").css("display", "flex");

        let accountNumber = $(this).closest(".accounts__item").find(".accounts__item-account").text();

        console.log(accountNumber);

        $(".overlay__content").text(accountNumber);
       
    })


});