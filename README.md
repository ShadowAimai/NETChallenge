# NETChallenge

# Installation
- Run migrations for the DB context in the Frontend project using efcore, or running script.sql in SQL Server, that can be found on the root folder, in a new database.
- Change the connection strings and service URIs to match your local configuration for all projects.
- In Microsoft Visual Studio, Under the solution's properties, select multiple startup projects, and make sure both Frontend and StockService projects are set to start

# How to use
- Open a browser and input https://localhost:44339/ or as it was configured locally.
- Make sure javascript has enough permissions to run
- Create a user by selecting "Register" on the login page or on the Sign Up tab on top.
- Enter your credentials in Login page
- Select one of the chats (Bezos, Musk or Arnault), then input a message in the textbox

# Unit testing
- Select "Run Tests" in Microsoft Visual Studio

# Notes
- It was not fully clear if "decoupled bot" meant a proper AI, so a project using .NET Bot Framework was also added
- Due to monetary constraints, this could not be implemented. However, this project can still be used by using an emulator.
- The emulator can be found here: https://github.com/Microsoft/BotFramework-Emulator/releases/tag/v4.5.2