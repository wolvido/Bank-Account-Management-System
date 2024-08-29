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
        $(".transactions-overview__deposit_overlay").html(depositCashData);

        //reset form validation
        $(".transactions-overview__deposit_overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__deposit_overlay form");
    };
    async function withdrawCash(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateWithdrawCashPartial/${accountId}/${date}`);
        let withdrawCashData = await response.text();
        $(".transactions-overview__withdraw_overlay").html(withdrawCashData);

        //reset form validation
        $(".transactions-overview__withdraw_overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__withdraw_overlay form");
    }
    async function depositCheque(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateDepositChequePartial/${accountId}/${date}`);
        let depositChequeData = await response.text();
        $(".transactions-overview__deposit_overlay").html(depositChequeData);

        //reset form validation
        $(".transactions-overview__deposit_overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__deposit_overlay form");
    };
    async function withdrawCheque(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateWithdrawChequePartial/${accountId}/${date}`);
        let withdrawChequeData = await response.text();
        $(".transactions-overview__withdraw_overlay").html(withdrawChequeData);

        //reset form validation
        $(".transactions-overview__withdraw_overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__withdraw_overlay form");
    };

    let accountId;
    let dateString

    //deposit cash popup through button
    $(".transactions-overview__deposit-button").on("click", async function () {
        accountId = this.getAttribute('data-accountId');
        dateString = this.getAttribute('data-date');

        await depositCash(accountId, dateString);
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

    //withdraw cash popup through button
    $(".transactions-overview__withdraw-button").on("click", async function () {
        accountId = this.getAttribute('data-accountId');
        dateString = this.getAttribute('data-date');

        await withdrawCash(accountId, dateString);
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