I use Microsoft SQL Server (2019) as my main DB

For this project I created a small database (blog-engine)
and migration which you can find in ./API/Migrations 


To create a database with the necessary tables 
you must run this command in your console (CMD) or in Package Manager Console


dotnet ef database update --project API

(more info at: https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

And you have to install SQL Server on your computer (obviously 🙂)

Also you should look at ConnectionStrings inside  ./API/appsettings.json


I didn't do any unit tests or add comments 
because I thought they were redundant for such simple queries, 
but that doesn't mean I can't do them. 🙂

Hope I haven't missed anything.. Good luck!
