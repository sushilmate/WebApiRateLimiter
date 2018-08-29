# Introduction
This project is to limit the rate of hit a web service can get, we can decide the number of hits of particular service in a stipulated time.

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
    This project is web api, you can host in IIS, or self hosted server.
2.	Software dependencies
    IDE: Visula Studio 2017 Version 15.5.6
    Framework: .Net Core 2.1 C#
    Package: Nuget
3.	Latest releases
    No Releases.
4.	API references
    https://en.wikipedia.org/wiki/.NET_Core


# Build and Test
TODO: Describe and show how to build your code and run the tests. 
    Make sure you download the code from repo or clone the repo, this project is developed C# .NET Core framework so you need to have .Net core SDK installed on your system
    All the packages used in the project has been downloaded from nuget so your Visual Studio should hage package source as https://api.nuget.org/v3/index.json
    Once you open the project in visual studio, just build the project, this will restore the packages from nuget & all projects inside solution will build one by one.
    We have created 3 projects in the solutions.
    1)  WebApiRateLimiter - We have hosted web api, accessed mocked data, & provided limited access to the services.
    2)  WebApiRateLimiter.Demo - Console application to test the web api, this project has dependancies on above project so if you want to run this project you need to select multiple projects to run in solution & add above project while running this project.
    3)  WebApiRateLimiter.Tests - This projects has end to end integration test cases.

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 
    I'm happy to recieve to recive your development contribution in this project.
