$(function () {

    $.fn.showFlex = function() {
        this.css('display','flex');
    }

    //calendar control
    let currentMonth = new Date().getMonth();
    let currentYear = new Date().getFullYear();

    $(".calendar__select-month").val(currentMonth + 1); //set dropdown default to current month

    let transactions = JSON.parse(window.transactions);

    function calendarDay(day, adjacentMonths = false, balance = '0.00', withdrawal = '0.00') {
        let fadedDesign;
        let transactions =
            `
            <div class="calendar__date-transactions">
                <div class="calendar__date-balance">
                    `+ balance+`
                </div>

                <div class ="calendar__date-withrawal">
                    `+ withdrawal+`
                </div>
            </div>
            `;

        if (adjacentMonths == true) {
            fadedDesign = " calendar__date-day_faded";
            transactions =
                `
            <div class="calendar__date-transactions">
                <div class="calendar__date-balance">
                    <br>
                </div>

                <div class ="calendar__date-withrawal">
                    <br>
                </div>
            </div>
            `;
        }
        else {
            fadedDesign = " ";
        }

        let calendarDay =
            `
            <a class="calendar__date-item">

                <div class="calendar__date-day`+ fadedDesign + `">
                `+ day + `
                </div>

                `+ transactions + `

            </a>
            `;

        return calendarDay;
    }

    function populateCalendar(transactions) {
        let selectedMonth = $(".calendar__select-month").find(":selected").val() - 1;
        let selectedYear = $(".calendar__select-year").find(":selected").val();
        let daysInSelectedMonth = 32 - new Date(selectedYear, selectedMonth, 32).getDate();
        let transactionsInSelectedMonth = transactions.filter(transaction => new Date(transaction.date).getMonth() == selectedMonth && new Date(transaction.date).getFullYear() == selectedYear);
        let calendarContainer = $(".calendar__date-container");


        //previous month days
        let previousMonth = selectedMonth - 1;
        let previousYear = selectedYear;

        if (previousMonth < 0) {
            previousMonth = 11;
            previousYear = selectedYear - 1;
        };

        let daysInPreviousMonth = 32 - new Date(previousYear, previousMonth, 32).getDate();

        for (day = daysInPreviousMonth - new Date(selectedYear, selectedMonth, 1).getDay() + 1; day < daysInPreviousMonth + 1; day++) {
            calendarContainer.append(calendarDay(day, true));
        }

        
        let transactionToday; //transaction for the day when the loop is running

        //current month days
        for (day = 1; day < daysInSelectedMonth + 1; day++) {

            if (transactionsInSelectedMonth.filter(transactionDay => new Date(transactionDay.date).getDay() == day).length > 0) { //check if a transaction on this day exists

                transactionToday = transactionsInSelectedMonth.filter(transactionDay => new Date(transactionDay.date).getDay() == day);
                let dayBalance = transactionToday[0].balance;

                let dayWithdrawal = transactionToday[0].withdrawal;

                calendarContainer.append(calendarDay(day, false, dayBalance, dayWithdrawal));
            }
            else {
                calendarContainer.append(calendarDay(day));
            }

        }

        //next month days
        let nextMonth = selectedMonth + 1;
        let nextYear = selectedYear;

        if (nextMonth > 11) {
            nextMonth = 0;
            nextYear = selectedYear + 1;
        }

        let daysInNextMonth = 32 - new Date(nextYear, nextMonth, 32).getDate();

        for (day = 1; calendarContainer.children().length < 42; day++) {
            calendarContainer.append(calendarDay(day, true));
        };

    };

    function dePopulateCalendar(){
        $(".calendar__date-container").find("*").remove();
    };

    populateCalendar(transactions);

    $(".calendar__select-date").find('*').on("change", function () {

        dePopulateCalendar();
        populateCalendar(transactions);

    });

    //popup control
    $(".calendar__add-deposit").on("click", function () {
        $(".calendar__overlay").showFlex();
    });



});