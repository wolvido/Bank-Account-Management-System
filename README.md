# FundPulse Tracker
A full stack web application that offers a comprehensive financial management solution designed around a calendar-based interface. Helps clients track financial transactions across multiple accounts via a calendar based interface and a transaction overview interface.

### Purpose
Many business owners and individuals struggle with managing multiple bank accounts and the scattered transaction records associated with them. FundPulse was created to consolidate this data into a clear and user-friendly interface, making financial tracking simple and organized.  

### Features:
- Multiple bank account management.
![accounts](https://github.com/user-attachments/assets/c11c7284-8a33-455a-899a-5b1fe4add84a)


- A calendar interface that displays the balance and withdraws of every day of the month.
![calendar](https://github.com/user-attachments/assets/a6016fd3-dff0-4cec-b650-1f3efec567dd)


- A transaction overview interface for a given date, allowing users to view and edit all transactions for that day and providing a snapshot of their financial activity.
![transactions for the day](https://github.com/user-attachments/assets/7a6ec8b8-8c1e-4033-8ee2-52fb7b8edaeb)

- User accounts; signin, signout, and signup.
![authentication](https://github.com/user-attachments/assets/e60f879d-0afa-410e-ad38-b56c335034ea)

### Local Build
Dotnet needs to be installed first, please refer to https://dotnet.microsoft.com/en-us/download

Build Commands:
```bash
git clone https://github.com/wolvido/FundPulse-Tracker.git
```
- After cloning or downloading the sample you must first setup your local sql database. Please create a local SQL server instance.
- After you have created a local SQL server instance, rewrite the default connection strings in `FundPulse-Tracker/BmsKhameleon/appsettings.Development.json`  to point to the local SQL Server instance.
  - eg. `"DefaultConnection": "YourLocalDatabaseConnectionString".`

After a local database has been setup, and it's connection string established in appsettings.Development.json, proceed with bash commands:
```bash
cd FundPulse-Tracker

dotnet restore

dotnet build

dotnet tool install --global dotnet-ef

cd bmskhameleon

dotnet tool restore

dotnet ef database update --context AccountDbContext

dotnet ef database update --context IdentityDbContext

dotnet run
```
- Then, check the assigned localhost in the console (eg. http://localhost:5299) and paste in the browser.
- use the default username: BMSAdminDefault241 and password: BMSKeyDefault33.
- click the main logo to navigate to the bank accounts page.
