$(function () {
    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };

    String.prototype.removeWhiteSpace = function () {
        return this.replace(/\s/g, '');
    };

    // deposit/withdraw popup control
    function showDepositPopup(depositOverlay, withdrawOverlay) {
        depositOverlay.showFlex();
        withdrawOverlay.hide();
    };
    function showWithdrawPopup(depositOverlay, withdrawOverlay) {
        withdrawOverlay.showFlex();
        depositOverlay.hide();
    }
    function clearInputFields() {
        var today = moment().format('YYYY-MM-DD');
        $(".transaction-controller__fieldset input").val("");
        $(".transaction-controller__fieldset textarea").val("");

        $(".transaction-controller__cash-date-input").val(today);
        $(".transaction-controller__cheque-date-input").val(today);

        console.log(today);

        $(".transaction-controller__fieldset input").trigger("change");
    };
    function addTransactionPopup(depositButton, withdrawButton, depositOverlay, withdrawOverlay, showDepositPopup, showWithdrawPopup) {
        depositButton.on("click", function () {
            showDepositPopup(depositOverlay, withdrawOverlay);
            clearInputFields();
            console.log("deposit button clicked");
        });

        withdrawButton.on("click", function () {
            showWithdrawPopup(depositOverlay, withdrawOverlay);
            clearInputFields();
            console.log("withdraw button clicked");
        });
    };

    let depositButton = $(".transaction-controller__button_deposit");
    let withdrawButton = $(".transaction-controller__button_withdraw");
    let depositOverlay = $(".transaction-controller__overlay_deposit");
    let withdrawOverlay = $(".transaction-controller__overlay_withdraw");

    addTransactionPopup(depositButton, withdrawButton, depositOverlay, withdrawOverlay, showDepositPopup, showWithdrawPopup);


    // cheque/cash switch control
    let depositCashForm = $(".transaction-controller__fieldset_deposit.transaction-controller__fieldset_cash");
    let depositChequeForm = $(".transaction-controller__fieldset_deposit.transaction-controller__fieldset_cheque");
    let depositRadioControl = $(".transaction-controller__radio-controls_deposit");

    let withdrawCashForm = $(".transaction-controller__fieldset_withdraw.transaction-controller__fieldset_cash");
    let withdrawChequeForm = $(".transaction-controller__fieldset_withdraw.transaction-controller__fieldset_cheque");
    let withdrawRadioControl = $(".transaction-controller__radio-controls_withdraw");
    function formSwitchControl(chequeForm, cashForm, radioControl) {

        chequeForm.detach();
        radioControl.on("change", function () {
            if (radioControl.find($(".transaction-controller__radio_cash")).is(':checked')) {

                $(".transaction-controller__transaction-medium strong").text("Cash");

                chequeForm.detach();

                cashForm.insertAfter(radioControl);
                clearInputFields();
            }
            else {

                $(".transaction-controller__transaction-medium strong").text("Check");

                cashForm.detach();

                chequeForm.insertAfter(radioControl);
                clearInputFields();
            }
        });

    };

    formSwitchControl(depositChequeForm, depositCashForm, depositRadioControl);
    formSwitchControl(withdrawChequeForm, withdrawCashForm, withdrawRadioControl);


    // edit form transaction controller
    function formDataHandler(sources, destinations, cashTransactionType = null) {
        //will handle the data transfer from table item to the edit form

        //text to radio selection
        if (cashTransactionType !== null) {
            let cashTransactionTypeValue = cashTransactionType.text().removeWhiteSpace();
            console.log(cashTransactionTypeValue);

            $(`.transaction-controller__radio_${cashTransactionTypeValue}`).prop('checked', true);
            $(`.transaction-controller__radio_${cashTransactionTypeValue}`).trigger("change");
        };

        if (Array.isArray(sources) && Array.isArray(destinations) && sources.length === destinations.length) { //check if the sources and destinations are same length
            for (let i = 0; i < sources.length; i++) {
                if (destinations[i] && sources[i]) { //check if the sources and destinations are not null
                    destinations[i].val(sources[i].text().removeWhiteSpace()); //transfer the data
                    destinations[i].trigger('change'); //UI won't update without trigerring the event
                }
            }
        } else {
            console.error("Sources and destinations must be arrays and of the same length.");
            return;
        };

    };
    $(".transaction-controller__button_edit").on("click", function () {

        let transactionType = $(this).closest(".transaction-controller__item").find(".transaction-controller__field-transaction-type"); //withdraw or deposit
        let currencyType = $(this).closest(".transaction-controller__item").find(".transaction-controller__field-currency-type"); //cash or cheque
        let amount = $(this).closest(".transaction-controller__item").find(".transaction-controller__field-amount");
        let date = $(this).closest(".transaction-controller__item").find(".transaction-controller__field-date");
        let note = $(this).closest(".transaction-controller__item").find(".transaction-controller__field-note");
        let payee = $(this).closest(".transaction-controller__item").find(".transaction-controller__field-payee");
        let bankName = $(this).closest(".transaction-controller__item").find(".transaction-controller__field-bank-name");

        let cashTransactionType;
        let radioCurrencyType;

        let amountInput;
        let dateInput;
        let noteInput;
        let chequeNumberInput;
        let payeeInput;
        let bankNameInput;

        if (transactionType.text().includes('deposit')) {
            showDepositPopup(depositOverlay, withdrawOverlay);
        }
        else {
            showWithdrawPopup(depositOverlay, withdrawOverlay);
        };

        if (currencyType.text().includes('Cash') ) {
            radioCurrencyType = $(`.transaction-controller__radio_cash`);
            radioCurrencyType.prop('checked', true);
            radioCurrencyType.trigger('change'); //UI won't update without trigerring the event

            cashTransactionType = $(this).closest(".transaction-controller__item").find(".transaction-controller__cash-transaction");

            amountInput = $(".transaction-controller__cash-amount-input");
            dateInput = $(".transaction-controller__cash-date-input");
            noteInput = $(".transaction-controller__cash-note-input");
           
            formDataHandler([amount, note, date], [amountInput, noteInput, dateInput], cashTransactionType);
        }
        else {
            radioCurrencyType = $(`.transaction-controller__radio_cheque`);
            radioCurrencyType.prop('checked', true);
            radioCurrencyType.trigger('change');

            amountInput = $(".transaction-controller__cheque-amount-input");
            bankNameInput = $(".transaction-controller__cheque-bank-name-input");
            dateInput = $(".transaction-controller__cheque-date-input");
            noteInput = $(".transaction-controller__cheque-note-input");
            chequeNumberInput = $(".transaction-controller__cheque-number-input");
            payeeInput = $(".transaction-controller__cheque-payee-input");

            formDataHandler([amount, bankName, date, note, currencyType, payee], [amountInput, bankNameInput, dateInput, noteInput, chequeNumberInput, payeeInput]);
        };



 
    });


});