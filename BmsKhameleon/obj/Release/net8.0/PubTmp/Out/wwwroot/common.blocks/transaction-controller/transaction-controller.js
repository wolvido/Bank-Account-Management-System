$(async function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };
    
    async function parseToISO(dateStr) {
        // Extract the date and time parts
        const [datePart, timePart] = dateStr.split(' ');

        // Split the date into day, month, and year
        const [day, month, year] = datePart.split('/');

        // Create a new Date object using the correct order (YYYY-MM-DD)
        const formattedDate = `${year}-${month}-${day}`;

        // Convert to a Date object
        const dateObj = new Date(formattedDate);

        // Return the ISO string without milliseconds
        return dateObj.toISOString().split('T')[0];
    }
    async function depositCash(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateDepositCashPartial/${accountId}/${date}`);
        let depositCashData = await response.text();
        $(".transaction-controller__overlay").html(depositCashData);

        //reset form validation
        $(".transaction-controller__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transaction-controller__overlay form");
    };
    async function withdrawCash(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateWithdrawCashPartial/${accountId}/${date}`);
        let withdrawCashData = await response.text();
        $(".transaction-controller__overlay").html(withdrawCashData);

        //reset form validation
        $(".transaction-controller__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transaction-controller__overlay form");
    }
    async function depositCheque(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateDepositChequePartial/${accountId}/${date}`);
        let depositChequeData = await response.text();
        $(".transaction-controller__overlay").html(depositChequeData);

        //reset form validation
        $(".transaction-controller__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transaction-controller__overlay form");
    };
    async function withdrawCheque(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateWithdrawChequePartial/${accountId}/${date}`);
        let withdrawChequeData = await response.text();
        $(".transaction-controller__overlay").html(withdrawChequeData);

        //reset form validation
        $(".transaction-controller__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transaction-controller__overlay form");
    };

    var accountId;
    var dateString;

    //deposit cash popup through button click
    $(".transaction-controller__button_deposit").on("click", async function () {
        accountId = this.getAttribute("data-accountId");
        dateString = this.getAttribute("data-date");
        await depositCash(accountId, dateString);
        $(".transaction-controller__overlay").showFlex();
    });

    //deposit cheque popup through radio
    $(document).on("change", ".transactions-overview__cheque-deposit-radio", async function () {
        if ($(".transactions-overview__cheque-deposit-radio").is(':checked')) {
            await depositCheque(accountId, dateString);
        }
    });

    //deposit cash popup through radio
    $(document).on("change", ".transactions-overview__cash-deposit-radio", async function () {
        if ($(".transactions-overview__cash-deposit-radio").is(':checked')) {
            await depositCash(accountId, dateString);
        }
    });

    //withdraw cash popup through button click
    $(".transaction-controller__button_withdraw").on("click", async function () {
        accountId = this.getAttribute("data-accountId");
        dateString = this.getAttribute("data-date");
        await withdrawCash(accountId, dateString);
        $(".transaction-controller__overlay").showFlex();
    });
    //withdraw cheque popup through radio
    $(document).on("change", ".transactions-overview__cheque-withdraw-radio", async function () {
        if ($(".transactions-overview__cheque-withdraw-radio").is(':checked')) {
            await withdrawCheque(accountId, dateString);
        }
    });
    //withdraw cash popup through radio
    $(document).on("change", ".transactions-overview__cash-withdraw-radio", async function () {
        if ($(".transactions-overview__cash-withdraw-radio").is(':checked')) {
            await withdrawCash(accountId, dateString);
        }
    });
});