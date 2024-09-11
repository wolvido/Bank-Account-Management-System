$(async function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };
    async function parseToISO(dateStr) {
        // Extract the date and time parts
        const [datePart] = dateStr.split(' ');

        // Split the date into day, month, and year
        const [day, month, year] = datePart.split('/').map(Number);

        // Create the date using Date.UTC, adjusting the month to zero-indexed
        const dateObj = new Date(Date.UTC(year, month - 1, day));

        // Check if the date is valid
        if (isNaN(dateObj.getTime())) {
            throw new Error('Invalid date value');
        }

        // Return the ISO string without milliseconds
        return dateObj.toISOString().split('T')[0];
    }
    async function depositCash(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateDepositCashPartial/${accountId}/${date}`);
        let depositCashData = await response.text();
        $(".transactions-overview__overlay").html(depositCashData);

        //reset form validation
        $(".transactions-overview__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__overlay form");
    };
    async function withdrawCash(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateWithdrawCashPartial/${accountId}/${date}`);
        let withdrawCashData = await response.text();
        $(".transactions-overview__overlay").html(withdrawCashData);

        //reset form validation
        $(".transactions-overview__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__overlay form");
    }
    async function depositCheque(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateDepositChequePartial/${accountId}/${date}`);
        let depositChequeData = await response.text();
        $(".transactions-overview__overlay").html(depositChequeData);

        //reset form validation
        $(".transactions-overview__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__overlay form");
    };
    async function withdrawCheque(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CreateWithdrawChequePartial/${accountId}/${date}`);
        let withdrawChequeData = await response.text();
        $(".transactions-overview__overlay").html(withdrawChequeData);

        //reset form validation
        $(".transactions-overview__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__overlay form");
    };
    async function loadWithdrawalsTable(accountId, dateString) {
        let date = await parseToISO(dateString);

        //grab main view withdrawals table and populate the withdrawals section
        let response = await fetch(`/WithdrawalsTable/${accountId}/${date}`);
        let withdrawTable = await response.text();
        $(".transactions-overview__withdrawals-section").html(withdrawTable);

        //grab the total withdraw and populate the total withdraw field on main view
        let cashTotalText = $(".transactions-overview__withdrawals-total-value_cash").text();
        let chequeTotalText = $(".transactions-overview__withdrawals-total-value_cheque").text();
        let cashTotal = cashTotalText.match(/\d+(\.\d+)?/)[0];
        let chequeTotal = chequeTotalText.match(/\d+(\.\d+)?/)[0];
        let totalWithdrawals = (parseFloat(cashTotal) + parseFloat(chequeTotal)).toFixed(2);
        await populateTotalWithdrawals(totalWithdrawals);
    }
    async function loadCashDepositsTable(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/CashDepositsTable/${accountId}/${date}`);
        let depositTable = await response.text();
        $(".transactions-overview__deposits-cash").html(depositTable);
    }
    async function loadChequeDepositsTable(accountId, dateString) {
        let date = await parseToISO(dateString);

        let response = await fetch(`/ChequeDepositsTable/${accountId}/${date}`);
        let depositTable = await response.text();
        $(".transactions-overview__deposits-cheque").html(depositTable);
    }
    async function loadDepositsTable(accountId, dateString) {
        await loadCashDepositsTable(accountId, dateString);
        await loadChequeDepositsTable(accountId, dateString);

        //grab the total deposit and populate the total deposit field on main view
        let cashTotalText = $(".transactions-overview__deposits-total-value_cash").text();
        let chequeTotalText = $(".transactions-overview__deposits-total-value_cheque").text();
        let cashTotal = cashTotalText.match(/\d+(\.\d+)?/)[0];
        let chequeTotal = chequeTotalText.match(/\d+(\.\d+)?/)[0];
        let totalDeposits = (parseFloat(cashTotal) + parseFloat(chequeTotal)).toFixed(2);
        await populateTotalDeposits(totalDeposits);

    }
    async function populateTotalDeposits(amount) {
        $(".transactions-overview__balance-value_deposit").text(amount);
    }
    async function populateTotalWithdrawals(amount) {
        $(".transactions-overview__balance-value_withdraw").text(amount);
    }
    async function updateTransaction(transactionId) {
        var response = await fetch(`/UpdateTransactionPartial/${transactionId}`);
        var updateTransactionData = await response.text();
        $(".transactions-overview__overlay").html(updateTransactionData);

        //reset form validation
        $(".transactions-overview__overlay form").data("validator", null);
        $.validator.unobtrusive.parse(".transactions-overview__overlay form");
    }
    async function backToCalendarLink(accountId, dateString) {
        let date = await parseToISO(dateString);
        $(".transactions-overview__back-button").prop("href", `/Calendar/${accountId}/${date}`);
    }

    let accountId = $("main").attr('data-accountId');
    let dateString = $("main").attr('data-date');

    //generate link to go back to calendar
    await backToCalendarLink(accountId, dateString);

    //load withdrawals table
    await loadWithdrawalsTable(accountId, dateString);

    //load deposits table
    await loadDepositsTable(accountId, dateString);

    //deposit cash popup through button click
    $(".transactions-overview__deposit-button").on("click", async function () {
        await depositCash(accountId, dateString);
        $(".transactions-overview__overlay").showFlex();
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
    $(".transactions-overview__withdraw-button").on("click", async function () {
        await withdrawCash(accountId, dateString);
        $(".transactions-overview__overlay").showFlex();
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

    //edit transaction
    $(document).on("click", ".transaction-overview__button_edit", async function () {
        var transactionId = this.getAttribute('data-transactionId');
        await updateTransaction(transactionId);
        $(".transactions-overview__overlay").showFlex();
    });

    //update date notice
    $(document).on("change", ".transactions-overview__input-date", async function () {
        let newDate = $(".transactions-overview__input-date").val();

        let oldDate = $(".transactions-overview__notice-date").text();

        console.log(newDate);
        console.log(oldDate);
        let formattedNewDate = moment(newDate, "YYYY-MM-DD").format(this.getAttribute("data-date-format"));

        $(".transactions-overview__notice-date").text(formattedNewDate);

    });
});