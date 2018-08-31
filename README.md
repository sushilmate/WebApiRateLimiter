# Introduction
This project is to limit the rate of hit a web service can get, we can decide the number of hits of particular service in a stipulated time.

# Getting Started
1.	Installation process
    This project is web api, you can host in IIS, or self hosted server.
2.	Software dependencies
    IDE: Visula Studio 2017 Version 15.5.6 <br>
    Framework: .Net Core 2.1 C# https://www.microsoft.com/net/download/dotnet-core/2.1<br>
    Package Manager: Nuget API https://www.nuget.org/api/v2
3.	API references
    https://en.wikipedia.org/wiki/.NET_Core


# Build and Test
    Make sure you download the code from repo or clone the repo, this project is developed C# .NET Core framework so you need to have .Net core SDK installed on your system
    All the packages used in the project has been downloaded from nuget so your Visual Studio should hage package source as https://api.nuget.org/v3/index.json
    Once you open the project in visual studio, just build the project, this will restore the packages from nuget & all projects inside solution will build one by one.
    We have created 3 projects in the solutions.

    1)  WebApiRateLimiter - We have hosted web api, accessed mocked data, & provided limited access to the services.

    2)  WebApiRateLimiter.Demo - Console application to test the web api, this project has dependancies on above project so if you want to run this project you need to select multiple projects to run in solution & add above project while running this project.

    3)  WebApiRateLimiter.Tests - This projects has end to end integration test cases.

# Project Detais
    I have used the .NET technology stack to implement this feature, I have build this project with latest framework/versions of the .NET.
    
    I have started with C# .NET Core 2.1 version to implement this web api, keeping in mind code should be easy to scale if we bring more serivces to this project, it should be readable & kept DRY wherever possible.
    As mock data was provided so I decided to keep data in the project for simplicity rather than putting in to Database, implemented Repository pattern for reading the Data from the CSV, it super easy if you decide to bring databse in this project, you can easily switch to database. Repository provides abstraction & reduce repoetitive code even it helps us to write unit test cases.
    In Data folder of the solution we have DbContext with mock data, repositoy with interfaces, entity model & viewmodels (poco) which also bring the abstraction.
    
    In the project I have used to AutoMapper library to implement mapping between model to viewmodel & vice versa. this reduced to plenty of code.
    
    In Project I have kept API rate limited settings in the configuration so we can easily configure as per choice & dynamically application reload those changes without re running the app.
    The gist of this project is to limit the API uses for that we have used Memory cache at server side to keep the API uses in the memory.MemoryCache is singlton & shared across different users.
    
    I have also used swagger for UI purpose, with the help of swagger you can easily invokes API from browser. when you run the project swagger UI gets loaded in the browser & shows all the services from the app.
    
    There are different ways to implement rate limiting on APIs, I have started with the approch to have rate limitting with the help of attribute filters which gets trigger very later part of the ASP.NET Web api pipeline.
    Later I realized if want to decide on APIs should be served or not 

