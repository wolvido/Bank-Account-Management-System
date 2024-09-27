$(function () {

    $.fn.showFlex = function () {
        this.css('display', 'flex');
    };

    //calendar control
    let currentMonth = new Date().getMonth();
    let currentYear = new Date().getFullYear();

    let transactions = JSON.parse(window.transactions); //inject data here

    let transactionsMonth = transactions[0].Date.split('-')[1] * 1;
    let transactionsYear = transactions[0].Date.split('-')[0] * 1;

    $(".calendar__select-month").val(transactionsMonth); //set dropdown default to current month
    $(".calendar__select-year").val(transactionsYear); //set dropdown default to current year"
    async function calendarDay(day, adjacentMonths = false, balance = '0.00', withdrawal = '0.00', date = '', accountId = '') {
        let transactions =
            `
            <div class="calendar__date-transactions">
                <div class="calendar__date-balance">
                    ${Number(balance).toLocaleString('en-PH', {style:"currency", currency:"PHP"})}
                </div>

                <div class ="calendar__date-withrawal">
                    ${Number(withdrawal).toLocaleString('en-PH', {style:"currency", currency:"PHP"})}
                </div>
            </div>
            `;

        //faded day from previous and next month
        let fadedDesign;
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

        //highlight today on calendar
        let higlightToday = ''; 
        let today = new Date();
        today.setHours(8, 0, 0, 0);
        if (date == today.toISOString().split('.')[0]) {
            higlightToday = 'calendar__date-item_highlight';
        }

        let calendarDay = ''; // Default value if date or accountId is not provided

        // inject the day item to the calendar
        if (date !== '' && accountId !== '') {
            const transactionOverviewUrl = `/TransactionsOverview/${accountId}/${date}`; // form the URL

            // Construct calendarDay with the fetched URL
            calendarDay = `
            <a href="${transactionOverviewUrl}" class="calendar__date-item ${higlightToday}">
                <div class="calendar__date-day${fadedDesign}">
                    ${day}
                </div>
                ${transactions}
            </a>
            `;
        }
        else
        {
            calendarDay = `
            <div class="calendar__date-item">
                <div class="calendar__date-day${fadedDesign}">
                    ${day}
                </div>
                ${transactions}
            </div>
            `;
        }

        return calendarDay;  
    }

    async function populateCalendar(transactions) {
        let selectedMonth = $(".calendar__select-month").find(":selected").val() - 1;
        let selectedYear = $(".calendar__select-year").find(":selected").val();
        let daysInSelectedMonth = 32 - new Date(selectedYear, selectedMonth, 32).getDate();
        let transactionsInSelectedMonth = transactions.filter(transaction => new Date(transaction.Date).getMonth() == selectedMonth && new Date(transaction.Date).getFullYear() == selectedYear);
        let calendarContainer = $(".calendar__date-container");

        let previousMonth = selectedMonth - 1;
        let previousYear = selectedYear;
        //previous year if the month is january
        if (previousMonth < 0) {
            previousMonth = 11;
            previousYear = selectedYear - 1;
        };
        //previous month days
        let daysInPreviousMonth = 32 - new Date(previousYear, previousMonth, 32).getDate();

        for (day = daysInPreviousMonth - new Date(selectedYear, selectedMonth, 1).getDay() + 1; day < daysInPreviousMonth + 1; day++) {
            let transactionToday = transactionsInSelectedMonth.filter(transactionDay => new Date(transactionDay.Date).getDate() == day);

            calendarContainer.append(await calendarDay(day, true));
        }


        //current month days
        for (day = 1; day < daysInSelectedMonth + 1; day++) {

            if (transactionsInSelectedMonth.filter(transactionDay => new Date(transactionDay.Date).getDate() == day).length > 0) { //check if a transaction on this day exists

                let transactionToday = transactionsInSelectedMonth.filter(transactionDay => new Date(transactionDay.Date).getDate() == day);
                let dayBalance = transactionToday[0].TotalBalance;

                let dayWithdrawal = transactionToday[0].TotalWithdrawal;

                let transactionDate = transactionToday[0].Date;
                let transactionAccountId = transactionToday[0].AccountId;


                //display transaction for current day
                calendarContainer.append(await calendarDay(day, false, dayBalance, dayWithdrawal, transactionDate, transactionAccountId));
            }
            else {
                calendarContainer.append(await calendarDay(day));
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
            calendarContainer.append(await calendarDay(day, true) );
        };

    };

    function dePopulateCalendar(){
        $(".calendar__date-container").find("*").remove();
    };

    //reload link on date change
    function reloadWithRouteParameter(newParam) {
        let currentUrl = window.location.pathname; // Get the current path
        let newUrl = `${currentUrl}/${newParam}`; // Create the new route with the added parameter

        //count the number of forward slash in the new url
        let forwardSlashCount = (newUrl.match(/\//g) || []).length;

        if (forwardSlashCount > 3) { // If there are more there is already a date in the url
            let urlSplit = currentUrl.split('/'); // Split the url into an array
            urlSplit.pop(); // Remove the old date from the array
            currentUrl = urlSplit.join('/'); //combine the array back into a string
            newUrl = `${currentUrl}/${newParam}`; // Create the new route with the new date
        }

        // Reload the page with the new route
        window.location.href = newUrl;
    }

    //intial load calendar
    populateCalendar(transactions);

    //on date change reload calendar
    $(".calendar__select-date").find('*').on("change", function () {
        dePopulateCalendar();
        populateCalendar(transactions);
    });

    //on date change reload link with the new date
    $(".calendar__select-date").on("change", function () {
        let selectedMonth = $(".calendar__select-month").find(":selected").val();
        let selectedYear = $(".calendar__select-year").find(":selected").val();

        let date = new Date(selectedYear, selectedMonth, 1).toISOString();
        reloadWithRouteParameter(date); 
    });

    //account info control
    $(".calendar__account-info-button").on("click", function () {
        $(".calendar__overlay-account-info").showFlex();
    });


});