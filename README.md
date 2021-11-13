# ATM-2


 If you want to use your favorite IDE, just open the **ATM Machine.sln** file in the root of the Git repository, and then select which project to run.

To start the web project:
 Requirements: **NodeJS** and **.NET 5**.
 
 If you have NodeJS installed navigate to **ATM-MachineWeb/ClientApp** in a terminal and run the command **npm install** to install the packages required for the web project to run.
 
 Once you have installed the packages, to run the project navigate to the **ATM-MachineWeb** folder and in your terminal use the command **dotnet run**. 
 
 Once it is running, go to **https://localhost:5001** to get to the website.
 

You might need to install dev certificates for HTTPS to run the project, if so, run the command **dotnet dev-certs https --trust** in your terminal of choice.
After you are done, you  can remove the dev certificates if you want by running the command **dotnet dev-certs https --clean**.

To start the console project, navigate with your terminal to the **ATM_Console** folder and run the **dotnet run** command.



