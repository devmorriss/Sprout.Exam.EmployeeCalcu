Readme

How to use:
1. Once cloned, do a dotnet restore, clean, and build.
2. I added a migration just in case you forgot to restore the database.bak file in ms sql. so you can run dotnet run straight.
3. You need to register and create an account yourself to login if you didnt restore the db backup.

.NET version used:
.NET 5

Summary of tools used:

AutoMapper <br>
MediatR <br>
FluentValidation <br>
Moq <br>
xUnit <br>

Pattern used:

CQRS <br>
Factory
<br><br>
Unit Testing Included
<br><br>
If we are going to deploy this on production, and an improvement is to be done, these are the things that should be done:

1. I would change the calculation of the salary for Regular, I think it is not correct (my own calculation is included in this project). <br> ![image](https://github.com/devmorriss/Sprout.Exam.EmployeeCalcu/assets/68768091/a3b94bb8-1e30-4f5a-bfda-09529ea61bb5)
2. Considering this is an API and SPA application, we should create them individually, and not as a bundle. with this, there would be a seperate port for SPA app and the API app. That way it wouldnt be as coupled as they were created.
3. If no. 2 is done, the folder structure would be better than the current (sample image of good folder structuring) <br>
![image](https://github.com/devmorriss/Sprout.Exam.EmployeeCalcu/assets/68768091/9773df17-e990-4a6e-b50f-8c9763ef6ef7)
4. Custom result for both error message and API message to fetch data accordingly.
5. I would use TypeScript rather than JavaScript to reduce errors on data fetching since most of the response is of type Any.
6. the generate method authService.getAccessToken() is being accessed by mostly all the js files, it would be better to have a single file for this that whenever you communicate with the api, it would get added;
7. Custom error pages for error status codes.
8. There was an error when app was first launched when in development environment(json fetch data error), I fixed it by returning a text 'try to login again' to refrain from crashing.
