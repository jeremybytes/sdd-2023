# Get Comfortable with .NET 7 and the CLI  
Demo Walkthrough 

By following this walkthrough, you can re-create the projects in this repository: [https://github.com/jeremybytes/sdd-2023/tree/main/01-dotnet-cli](https://github.com/jeremybytes/sdd-2023/tree/main/01-dotnet-cli).

**Level**: Introductory  

**Target**: C# developers who have been working with unfamiliar with the CLI (command-line interface) that .NET 7.0 provides. 

**Required Software**:  
* .NET 7.0 SDK  
[https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)  
* Visual Studio Code  
[https://code.visualstudio.com/download](https://code.visualstudio.com/download)  
(You can also use Visual Studio 2022 Community Edition)
* C# Extension for Visual Studio Code  
[https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)  
(This will give you code completion and lots of other help/refactoring in Visual Studio Code).

**Additional Files:**  
The following files will be required during the walkthrough. These files are in the [Starting Files folder](/StartingFiles/) of the repository:

* [snippets.txt](/StartingFiles/snippets.txt)  
(code snippets to pasted into the project)
* [CSVPeopleProvider.cs](/StartingFiles/CSVPeopleProvider.cs)  
(data provider to show dependency injection)
* [People.txt](/StartingFiles/People.txt)  
(data for the CSVPeopleProvider)

At the end of the walkthrough, we will have a working web service, unit tests for the service, and a console application that calls the service. In addition, we will look at the built-in dependency injection container in ASP.NET Core.

You can use Visual Studio Code or Visual Studio to edit the code files (I will use a mixture of the two). And PowerShell or cmd.exe will work for the command line. The instructions here show PowerShell commands (which may be slightly different from cmd.exe).

Code to be typed at the command prompt will appear like this:

```
PS C:\dotnetCLI> command
```

In addition, code blocks will be shown for the projects themselves.

## Hello dotnet

We'll start by opening PowerShell to the root folder that we use for this project (the location and name of the root folder is up to you).

The "dotnet" command will be used throughout this walkthrough. This is available and automatically in your path when you install the .NET Core SDK.

```
PS C:\dotnetCLI> dotnet

Usage: dotnet [options]
Usage: dotnet [path-to-application]

Options:
  -h|--help         Display help.
  --info            Display .NET information.
  --list-sdks       Display the installed SDKs.
  --list-runtimes   Display the installed runtimes.

path-to-application:
  The path to an application .dll file to execute.
```

By typing a command, you get a list of options. To get further details on any command, just add "-h".

```
PS C:\dotnetCLI> dotnet -h
Usage: dotnet [runtime-options] [path-to-application] [arguments]

Execute a .NET application.

runtime-options:
  --additionalprobingpath <path>   Path containing probing policy and assemblies to probe for.
  --additional-deps <path>         Path to additional deps.json file.
  --depsfile                       Path to <application>.deps.json file.
  --fx-version <version>           Version of the installed Shared Framework to use to run the application.
  --roll-forward <setting>         Roll forward to framework version  (LatestPatch, Minor, LatestMinor, Major, LatestMajor, Disable).
  --runtimeconfig                  Path to <application>.runtimeconfig.json file.
[...Plus a lot more...]
```

To see what version of .NET Core is installed, use "--version"

```
PS C:\dotnetCLI> dotnet --version
7.0.202
```

This walkthrough was built with version 7.0.202.

## Web Service

We'll start by creating a web service. 

### Creating the Project
First, create a new folder and navigate to it.

```
PS C:\dotnetCLI> mkdir person-api
```
```
PS C:\dotnetCLI> cd .\person-api\
PS C:\dotnetCLI\person-api>
```

Use "dotnet new" to see a list of installed templates:

```
PS C:\dotnetCLI\person-api> dotnet new
The 'dotnet new' command creates a .NET project based on a template.

Common templates are:
Template Name         Short Name    Language    Tags
--------------------  ------------  ----------  -------------------
ASP.NET Core Web App  webapp,razor  [C#]        Web/MVC/Razor Pages
Blazor Server App     blazorserver  [C#]        Web/Blazor
Class Library         classlib      [C#],F#,VB  Common/Library
Console App           console       [C#],F#,VB  Common/Console
Windows Forms App     winforms      [C#],VB     Common/WinForms
WPF Application       wpf           [C#],VB     Common/WPF

An example would be:
   dotnet new console

Display template options with:
   dotnet new console -h
Display all installed templates with:
   dotnet new list
Display templates available on NuGet.org with:
   dotnet new search web
```

> This is not a complete list of installed templates, just the most commonly used ones. To get a full list, use "dotnet new list" (as noted in the output).

Notice that many command arguments have 2 options: a single dash (-h) option and a double dash (--help) option. These are equivalent.

To create a web service, we will use the "webapi" template. (This shows up on the full list of installed templates).

But before we create the project, let's look at a few of the options for the template (by using "-h" for help):

```
PS C:\dotnetCLI\person-api> dotnet new webapi -h
ASP.NET Core Web API (C#)
Author: Microsoft
Description: A project template for creating an ASP.NET Core application with an example Controller for a RESTful HTTP service. This template can also be used for ASP.NET Core MVC Views and Controllers.

Usage:
  dotnet new webapi [options] [template options]

Options:
[some options removed from this list]
  -n, --name <name>       
     The name for the output being created. If no name is specified, the name of the output
     directory is used.
  -lang, --language <C#>  
     Specifies the template language to instantiate.

Template options:
[most options are not shown for this list]
  --no-https
      Whether to turn off HTTPS. This option only applies if IndividualB2C,
      SingleOrg, or MultiOrg aren't used for --auth.
      Type: bool
      Default: false
  -minimal, --use-minimal-apis
      Whether to use minimal APIs instead of controllers.
      Type: bool
      Default: false
  --use-program-main
      Whether to generate an explicit Program class and Main method instead of
      top-level statements.
      Type: bool
      Default: false

To see help for other template languages (F#), use --language option:
   dotnet new webapi -h --language F#
```

There are a number of options that are not shown here that are also available. Some of the important ones for creating a service.

* --no-https  
With https enabled, http calls are automatically redirected to the https endpoint. You may wan to turn off this behavior when building test services on a machine that does not have an HTTPS certificate installed.  

* -minimal  
When this is not set (the default), API controllers are created. This may be what you're used to if you have created web api projects in the past. When "minimal" is set, controllers are not created, and endpoints are put directly into the Program.cs file.

* --use-program-main  
This gives the options to turn off top-level statements. Personally, I am not a fan of top-level statements since real world applications are generally complex enough to require multiple files and classes, so I prefer to have a Program class as well.

Let's create a new webapi project without https, with controller APIs, and with a program class:

```
PS C:\dotnetCLI\person-api> dotnet new webapi --no-https --use-program-main
The template "ASP.NET Core Web API" was created successfully.

Processing post-creation actions...
Restoring C:\dotnetCLI\person-api\person-api.csproj:
  Determining projects to restore...
  Restored C:\dotnetCLI\person-api\person-api.csproj (in 258 ms).
Restore succeeded.
```

Here is a directory listing to show the files that are generated by the template:

```
PS C:\dotnetCLI\person-api> ls

    Directory: C:\dotnetCLI\person-api

Mode        Length Name
----        ------ ----
d-----             Controllers
d-----             obj
d-----             Properties
-a----         127 appsettings.Development.json
-a----         151 appsettings.json
-a----         455 person-api.csproj
-a----         770 Program.cs
-a----         259 WeatherForecast.cs
```

Note that the project is named "person-api.csproj" (the same as the folder). We will see how to change this default value a bit later.

### Building and Running the Sample Service
From here, we can build the project.

```
PS C:\dotnetCLI\person-api> dotnet build
MSBuild version 17.4.1+9a89d02ff for .NET
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api -> C:\dotnetCLI\person-api\bin\Debug\net7.0\person-api.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:03.49
```

The service is self-hosting, so we can run the project directly.

```
PS C:\dotnetCLI\person-api> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5224
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dotnetCLI\person-api
```

Note that the host is listening on localhost port 5224. This port is randomly generated when the project is created, and we will change it to a specific port later on.

Now we can navigate to the service in the browser. An easy way to do this is use "ctrl+click" on the URL in the command window. This will open a browser to localhost with the correct port selected.

The default service requires a path, so add "/weatherforecast" in the address bar.

Here's a link with the port that was generated for my project:  
[http://localhost:5224/weatherforecast](http://localhost:5224/weatherforecast)

> To stop the service, press "ctrl+c".

Here is some sample data from the service:

```json
[{"date":"2023-05-16","temperatureC":-20,"temperatureF":-3,"summary":"Scorching"},{"date":"2023-05-17","temperatureC":43,"temperatureF":109,"summary":"Mild"},{"date":"2023-05-18","temperatureC":27,"temperatureF":80,"summary":"Chilly"},{"date":"2023-05-19","temperatureC":18,"temperatureF":64,"summary":"Sweltering"},{"date":"2023-05-20","temperatureC":43,"temperatureF":109,"summary":"Bracing"}]
```

This is randomly generated dummy data that is part of the webapi template.

Stop the service using "ctrl+c".

```
      Application is shutting down...
PS C:\dotnetCLI\person-api>  
```

### Looking at the Code
Now let's open the project to look at some code. Visual Studio Code is a great option for this. Type the following on the command line:

```
PS C:\dotnetCLI\person-api> code .
```

This will open the current folder in Visual Studio Code. With ASP.NET Core, we do not need to open a solution file or a project file (right now, we do not even have a solution file). Instead, we can open a folder to show the contents.

> If you are using Visual Studio 2022, you can do this as well. Just start Visual Studio 2022 and choose the "Open a local folder" option.

Inside Visual Studio Code, you will be prompted to "Add required assets". If you choose "Yes", this will create a new ".vscode" folder that has some setting files in it. These are used for debugging and running from within Visual Studio code. You can say "Yes" to add them, but we will not use them during this walkthrough.

### Updating the Service Code
With a working service, we can now go in and change it to run our code.

In Visual Studio Code, add a new folder to the project called "Models". To add a folder, put your cursor in the Explorer window (this is the one that shows the files). In the folder header (where it says "PERSON-API"), you will see a set of icons. One of these is "New Folder".  

The "Models" folder should be at the root of the project folder (as a sibling to "Controllers").  

*As an alternate, you can add a new folder from the command prompt or File Explorer. The new folder will automatically show up as part of the project in Visual Studio Code.*

### Adding a Person Class
From here, add a new file to the Models folder called "Person.cs". To add a file, click on the Models folder, and then choose the "New File" icon.

Visual Studio Code creates an empty file. Start by adding the namespace:

```csharp
namespace person_api;
```

> Note that the default namespace for the project is the same as the project name, except the dashes have been replaced by underscores.

Now locate the "snippets.txt" file and copy the "Person" record into the file. (Or just copy the code from this code block.)

```csharp
namespace person_api;

public record Person(int Id, string GivenName, string FamilyName,
    DateTime StartDate, int Rating, string FormatString = "")
{
    public override string ToString()
    {
        if (string.IsNullOrEmpty(FormatString))
            return $"{GivenName} {FamilyName}";
        return string.Format(FormatString, GivenName, FamilyName);
    }
}
```

*If the indentation is off when you paste in the code, use the keyboard shortcut "shift+alt+f" to format the file.*

### Adding a Data Provider
The Person record is the data type that for the service. To supply some data, we will add a data provider. Add a new file to the Models folder named "HardCodedPeopleProvider.cs".

As above, we will add the namespace plus the class declaration.

```csharp
namespace person_api;

public class HardCodedPeopleProvider
{
    
}
```

Then copy in the "GetPeople" and "GetPerson" methods from the "snippets.txt" file (or from the following code block).

```csharp
namespace person_api;

public class HardCodedPeopleProvider
{
    public static List<Person> GetPeople()
    {
        List<Person> p = new()
        {
            new Person(1, "John", "Koenig", new DateTime(1975, 10, 17), 6, ""),
            new Person(2, "Dylan", "Hunt", new DateTime(2000, 10, 2), 8, ""),
            new Person(3, "Leela", "Turanga", new DateTime(1999, 3, 28), 8, "{1} {0}"),
            new Person(4, "John", "Crichton", new DateTime(1999, 3, 19), 7, ""),
            new Person(5, "Dave", "Lister", new DateTime(1988, 2, 15), 9, ""),
            new Person(6, "Laura", "Roslin", new DateTime(2003, 12, 8), 6, ""),
            new Person(7, "John", "Sheridan", new DateTime(1994, 1, 26), 6, ""),
            new Person(8, "Dante", "Montana", new DateTime(2000, 11, 1), 5, ""),
            new Person(9, "Isaac", "Gampu", new DateTime(1977, 9, 10), 4, ""),
        };
        return p;
    }

    public Person GetPerson(int id)
    {
        return GetPeople().First(p => p.Id == id);
    }
}
```

This gives us the data that we need for now. The next step is to update the endpoints to provide our data.

### Updating the Controller

The next step is to update the controller to use our new data.

Inside the "Controllers" folder is the "WeatherForecastController.cs" file. We'll rename this file. To do this, click on the file, press F2, and change the name to "PeopleController.cs".

Open the "PeopleController.cs" file. Here we will rename the class, remove unneeded code, and update the "Get" method.

Here is the result:

```csharp
[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    [HttpGet(Name = "GetPeople")]
    public IEnumerable<Person> Get()
    {

    }
}
```

Be sure to update the class name (PeopleController) and the return type of the "Get" method ("Person" instead of "WeatherForeacast").

To fill in the functionality, we will use the HardCodedPeopleProvider that we created earlier. Here's the completed code:  

```csharp
[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    HardCodedPeopleProvider provider = new();

    [HttpGet(Name = "GetPeople")]
    public IEnumerable<Person> Get()
    {
        return provider.GetPeople();
    }
}
```

Note: the "Name" is used for Swagger (which is included by default). This is a way for us to test services in the browser, but we will not be looking at this today.

Finally, we will add a "Get" method that takes an ID parameter so that we can return a single item.  

```csharp
    [HttpGet("{id}", Name = "GetPerson")]
    public Person Get(int id)
    {
        return provider.GetPerson(id);
    }
```

Save all the files.

Back at the command prompt, build the service. (We'll do an explicit build here just to see if there are any build problems we need to fix before running the service.)  

```
PS C:\dotnetCLI\person-api> dotnet build
MSBuild version 17.4.1+9a89d02ff for .NET
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api -> C:\dotnetCLI\person-api\bin\Debug\net7.0\person-api.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:03.13
```

Assuming you have a successful build, run the service.  

```
PS C:\dotnetCLI\person-api> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5224
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dotnetCLI\person-api
```

Navigate to http://localhost:5224/people (substituting the appropriate port number).

You should get the following data in the browser:

```json
[{"id":1,"givenName":"John","familyName":"Koenig","startDate":"1975-10-17T00:00:00","rating":6,"formatString":""},{"id":2,"givenName":"Dylan","familyName":"Hunt","startDate":"2000-10-02T00:00:00","rating":8,"formatString":""},{"id":3,"givenName":"Leela","familyName":"Turanga","startDate":"1999-03-28T00:00:00","rating":8,"formatString":"{1} {0}"},{"id":4,"givenName":"John","familyName":"Crichton","startDate":"1999-03-19T00:00:00","rating":7,"formatString":""},{"id":5,"givenName":"Dave","familyName":"Lister","startDate":"1988-02-15T00:00:00","rating":9,"formatString":""},{"id":6,"givenName":"Laura","familyName":"Roslin","startDate":"2003-12-08T00:00:00","rating":6,"formatString":""},{"id":7,"givenName":"John","familyName":"Sheridan","startDate":"1994-01-26T00:00:00","rating":6,"formatString":""},{"id":8,"givenName":"Dante","familyName":"Montana","startDate":"2000-11-01T00:00:00","rating":5,"formatString":""},{"id":9,"givenName":"Isaac","familyName":"Gampu","startDate":"1977-09-10T00:00:00","rating":4,"formatString":""}]
```

Also, try the other endpoint by adding an ID of 3 to the URL.  

http://localhost:5224/people/3  
Remember to use the appropriate port.

The output will be as follows:

```json
{"id":3,"givenName":"Leela","familyName":"Turanga","startDate":"1999-03-28T00:00:00","rating":8,"formatString":"{1} {0}"}
```

### Clean Up  
Now that we have our endpoints in place, we can clean up the unneeded weather forecast code that came with the template.

* Delete the "WeatherForecast.cs" file from the project folder.

### Changing the Port
One last change is that we will change the port that is used for the service. This is something that I do with my localhost projects to eliminate possible collisions if I have multiple services running at the same time.

To change the port, we'll update the launch settings.  

In Visual Studio Code, expand the "Properties" folder in Explorer, and open "launchSettings.json".  

Here are the profiles that were generated for me:  

```json
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5224",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
```

Notice the "applicationUrl" setting. We will change this for the "http" profile.

> As a side note, I generally delete the IIS Express profile since I don't use this with ASP.NET Core. If you decide to delete this profile, make sure that you have the right number of closing braces at the end; it can be difficult to get that right.  

Use the following for the "http" profile:  

```json
"applicationUrl": "http://localhost:9874",
```


Back on the command line, stop and restart the service.

```
      Application is shutting down...
PS C:\dotnetCLI\person-api> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dotnetCLI\person-api
```

We now see that the host is listening on port 9874.

If we click "Refresh" in the browser (or navigate to the old address), we get a "connection refused" error.

[http://localhost:5245/people](http://localhost:5245/people)

If we update the URL to use the new port, then we have the data that we expect.

[http://localhost:9874/people](http://localhost:9874/people)

```json
[{"id":1,"givenName":"John","familyName":"Koenig","startDate":"1975-10-17T00:00:00","rating":6,"formatString":""},{"id":2,"givenName":"Dylan","familyName":"Hunt","startDate":"2000-10-02T00:00:00","rating":8,"formatString":""},{"id":3,"givenName":"Leela","familyName":"Turanga","startDate":"1999-03-28T00:00:00","rating":8,"formatString":"{1} {0}"},{"id":4,"givenName":"John","familyName":"Crichton","startDate":"1999-03-19T00:00:00","rating":7,"formatString":""},{"id":5,"givenName":"Dave","familyName":"Lister","startDate":"1988-02-15T00:00:00","rating":9,"formatString":""},{"id":6,"givenName":"Laura","familyName":"Roslin","startDate":"2003-12-08T00:00:00","rating":6,"formatString":""},{"id":7,"givenName":"John","familyName":"Sheridan","startDate":"1994-01-26T00:00:00","rating":6,"formatString":""},{"id":8,"givenName":"Dante","familyName":"Montana","startDate":"2000-11-01T00:00:00","rating":5,"formatString":""},{"id":9,"givenName":"Isaac","familyName":"Gampu","startDate":"1977-09-10T00:00:00","rating":4,"formatString":""}]
```

And that's our working service.

> Be sure to shut down the service before continuing to the unit tests.

## Unit Tests

Next we'll create a unit test project and add some tests for our code in the web service.

### Creating the Project
Open a new PowerShell (or cmd.exe) tab/window at the root of the project. If you are using Terminal, you can right-click on the current tab and choose "Duplicate Tab" to open a new tab at the same location.

```
PS C:\dotnetCLI>
```

Create a folder for the unit test project named "person-api-tests" and navigate to that folder.

```
PS C:\dotnetCLI> mkdir person-api-tests
PS C:\dotnetCLI> cd .\person-api-tests\
PS C:\dotnetCLI\person-api-tests>
```

If we type "dotnet new list", we can see several unit test project options in the list.

```
PS C:\dotnetCLI\person-api> dotnet new list
These templates matched your input:

Template Name            Short Name    Tags
-----------------------  ------------  -------------------------------------
MSTest Test Project      mstest        Test/MSTest
NUnit 3 Test Item        nunit-test    Test/NUnit
NUnit 3 Test Project     nunit         Test/NUnit
xUnit Test Project       xunit         Test/xUnit
```

For our sample, we'll use NUnit.

```
PS C:\dotnetCLI\person-api-tests> dotnet new nunit
The template "NUnit 3 Test Project" was created successfully.

Processing post-creation actions...
Restoring C:\dotnetCLI\person-api-tests\person-api-tests.csproj:
  Determining projects to restore...
  Restored C:\dotnetCLI\person-api-tests\person-api-tests.csproj (in 483 ms).
Restore succeeded.
```

Open the project folder in Visual Studio Code.

```
PS C:\dotnetCLI\person-api-tests> code .
```

The "UnitTest1.cs" file has a sample test.

```csharp
namespace person_api_tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
```

### Running the Test
To run the test, we use "dotnet test" at the command prompt.

```
PS C:\dotnetCLI\person-api-tests> dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api-tests -> C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll
Test run for C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.4.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 27 ms - person-api-tests.dll (net7.0)
```

Just like with "dotnet run", we do not need to do an explicit build to run the tests. The project will be built automatically if required.

### Adding a Project Reference
Since we want to test the controller from the web service, we need to add a reference to that project. We can do this from the command line using "dotnet add reference ...".

```
PS C:\dotnetCLI\person-api-tests> dotnet add reference ..\person-api\person-api.csproj
Reference `..\person-api\person-api.csproj` added to the project.
```

### Creating the First Test
Back in Visual Studio Code, we will update the placeholder test with our own.

First, rename the file from "UnitTest1.cs" to "PeopleController.cs". (To rename a file in Visual Studio Code, click on the file name and press F2).

Inside the file, we will rename the class from "Tests" to "PeopleControllerTests".

> Note: I like to rename the files and classes for tests because many test runners (include Visual Studio 2022) have options to group by file or class name.

```csharp
public class PeopleControllerTests
```

We will use the setup method to create an instance of the controller that our tests will use. For this, we'll add a class-level field for the controller and "new" it up in the Setup method.

```csharp
public class PeopleControllerTests
{
    PeopleController controller;

    [SetUp]
    public void Setup()
    {
        controller = new PeopleController();
    }
    ...
}
```

*Note: You can use "ctrl+." to bring in the "using" statement for the PeopleController class. You can do the same for the Person class* 

*Or you can manually add the namespaces to the top of the file:*

```csharp
using person_api;
using person_api.Controllers;
```

By creating the controller in the Setup method, we ensure that we have a fresh controller for each test in our test run.

Next, we'll remove "Test1" and add something a little more useful.

```csharp
[Test]
public void GetPeople_ReturnsAllItems()
{
    IEnumerable<Person> actual = controller.Get();
    Assert.That(actual.Count, Is.EqualTo(9));
}
```

(You will need to bring in some using statements, but I'll assume that you are used to doing that now.)

Back at the command prompt, we'll run our new test.

```
PS C:\dotnetCLI\person-api-tests> dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api -> C:\dotnetCLI\person-api\bin\Debug\net7.0\person-api.dll
  person-api-tests -> C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll
Test run for C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.4.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 41 ms - person-api-tests.dll (net7.0)
```

The first test is passing. This test is not very useful because it is tied to the "HardCodedPeopleProvider" class that is part of the web service project. We'll separate this out later on when we look at dependency injection.

### Adding 2 More Tests
Next we will add a couple of tests for the "Get(int id)" method.

```csharp
[Test]
public void GetPerson_WithValidId_ReturnsPerson()
{
    Person? actual = controller.Get(2);
    Assert.That(actual?.Id, Is.EqualTo(2));
}

[Test]
public void GetPerson_WithInvaliId_ReturnsNull()
{
    Person? actual = controller.Get(-10);
    Assert.That(actual, Is.Null);
}
```

Now run the tests.

```
PS C:\dotnetCLI\person-api-tests> dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api -> C:\dotnetCLI\person-api\bin\Debug\net7.0\person-api.dll
  person-api-tests -> C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll
Test run for C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.4.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
  Failed GetPerson_WithInvaliId_ReturnsNull [24 ms]
  Error Message:
   System.InvalidOperationException : Sequence contains no matching element
  Stack Trace:
     at System.Linq.ThrowHelper.ThrowNoMatchException()
   at System.Linq.Enumerable.First[TSource](IEnumerable`1 source, Func`2 predicate)
   at person_api.HardCodedPeopleProvider.GetPerson(Int32 id) in C:\dotnetCLI\person-api\Models\HardCodedPeopleProvider.cs:line 24
   at person_api.Controllers.PeopleController.Get(Int32 id) in C:\dotnetCLI\person-api\Controllers\PeopleController.cs:line 20
   at person_api_tests.PeopleControllerTests.GetPerson_WithInvaliId_ReturnsNull() in C:\dotnetCLI\person-api-tests\PeopleControllerTests.cs:line 33
   at System.RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)
   at System.Reflection.MethodInvoker.Invoke(Object obj, IntPtr* args, BindingFlags invokeAttr)

Failed!  - Failed:     1, Passed:     2, Skipped:     0, Total:     3, Duration: 68 ms - person-api-tests.dll (net7.0)
```

In this case, the last test fails. That is because the web service throws an exception (InvalidOperationException) rather than returning null.

The test fails because of the way the HardCodedPeopleProvider is coded. As a reminder, here is the "GetPerson" method from that class:

```csharp
public Person GetPerson(int id)
{
    return GetPeople().First(p => p.Id == id);
}
```

The "First" method looks for the first match it can find. But if it finds no matches at all, it will throw an exception.

Let's change this to "FirstOrDefault":  

```csharp
public Person? GetPerson(int id)
{
    return GetPeople().FirstOrDefault(p => p.Id == id);
}
```

If the ID is not found, then the default for the record is returned: null.

We'll make one small change to the  "Get(int id)" method in the PeopleController class.

```csharp
[HttpGet("{id}", Name = "GetPerson")]
public Person? Get(int id)
{
    return provider.GetPerson(id);
}
```

A "?" has been added to the return type. This indicates that this method can return a null.

> *Even though the service method returns null, the service itself will return an HTTP 204 response (No Content). This indicates that the requested record could not be found.*

Now if we re-run the tests, we see that they all pass.

```
PS C:\dotnetCLI\person-api-tests> dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api -> C:\dotnetCLI\person-api\bin\Debug\net7.0\person-api.dll
  person-api-tests -> C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll
Test run for C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.4.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     3, Skipped:     0, Total:     3, Duration: 67 ms - person-api-tests.dll (net7.0)
```

> *Note: we do not have to explicitly re-build the web service project because it is referenced by the test project. So it will get re-built automatically as needed.*

Our tests still need better isolation so that we can have more control over the provider during testing. But this gets us started. We will make things a bit more robust later on.

## Console Application

As a third project, we will build a console application that calls the web service.

### Creating the Project
Open a new PowerShell (or cmd.exe) tab/window at the root of the project. If you are using Terminal, you can right-click on the current tab and choose "Duplicate Tab" to open a new tab at the same location.

```
PS C:\dotnetCLI>
```

Create a folder for the console project named "person-console" and navigate to that folder.

```
PS C:\dotnetCLI> mkdir person-console
PS C:\dotnetCLI> cd .\person-console\
PS C:\dotnetCLI\person-console>
```

Next create a new console application with "dotnet new console --use-program-main". As with our webapi project, we will not use top-level statements here since we will have multiple files/classes as part of the project.

```
PS C:\dotnetCLI\person-console> dotnet new console --use-program-main
The template "Console App" was created successfully.

Processing post-creation actions...
Restoring C:\dotnetCLI\person-console\person-console.csproj:
  Determining projects to restore...
  Restored C:\dotnetCLI\person-console\person-console.csproj (in 73 ms).
Restore succeeded.
```

### Running the Application
Run the application.

```
PS C:\dotnetCLI\person-console> dotnet run
Hello, World!
```

We have a working console application. Unfortunately, it takes the fun out of creating a new application because "Hello, World!" has already been added.

Open the project folder in Visual Studio Code

```
PS C:\dotnetCLI\person-console> code .
```

Open the "Program.cs" file.

```csharp
namespace person_console;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
```

This is what is included in the default template.

### Calling the Service
To call the service, we will add a copy of the "Person" class and create a class that knows how to talk to the service.

Using File Explorer (or the method of your choice), copy the "Person.cs" file from the "person-api" project into the root folder of the "person-console" project.

*One thing to notice when you do this: the file automatically shows up in Visual Studio Code.* 

> **For those who have been doing .NET for a while:** .NET 7 uses a different project system than .NET Framework. By default, the project includes any files that are in the project folder. There is no need to explicitly add them. (We can explicitly exclude them if we need to).

Open "Person.cs" in Visual Studio Code and change the namespace to match the console application: person_console.

```csharp
namespace person_console;

public record Person(int Id, string GivenName, string FamilyName,
    DateTime StartDate, int Rating, string FormatString = "")
{
    public override string ToString()
    {
        if (string.IsNullOrEmpty(FormatString))
            return $"{GivenName} {FamilyName}";
        return string.Format(FormatString, GivenName, FamilyName);
    }
}
```

Create a new file / class named "PersonReader.cs" / "PersonReader".

```csharp
namespace person_console;

public class PersonReader
{
    
}
```

Add a field of type "HttpClient".

```csharp
public class PersonReader
{
    private HttpClient client = new();
}
```

Copy the contents of the PersonReader class from the "snippets.txt" file.

```csharp
public class PersonReader
{
    HttpClient client =
        new() { BaseAddress = new Uri("http://localhost:9874") };
    public async Task<List<Person>> GetPeopleAsync()
    {
        HttpResponseMessage response =
            await client.GetAsync("people");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Unable to retrieve People. Status code {response.StatusCode}");

        var stringResult =
            await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Person>>(stringResult);
        if (result is null)
            throw new JsonException("Unable to deserialize List<Person> object (json object may be empty)");
        return result;
    }
}
```

> This code needs the Newtonsoft.Json package from NuGet. (We're using Newtonsoft.Json specifically so we can see how to add NuGet packages. Starting with .NET Core 3.0, System.Text.Json is part of the standard SDK. This package will work fine here (although the syntax and capabilities are a little bit different)). 

### Adding a NuGet Package
To add the package, we'll go back to the command prompt and use "dotnet add package".

```
PS C:\dotnetCLI\person-console> dotnet add package Newtonsoft.Json
  Determining projects to restore...
  Writing C:\Users\jerem\AppData\Local\Temp\tmpB40C.tmp
info : X.509 certificate chain validation will use the default trust store selected by .NET.
info : X.509 certificate chain validation will use the default trust store selected by .NET.
info : Adding PackageReference for package 'Newtonsoft.Json' into project 'C:\dotnetCLI\person-console\person-console.csproj'.
info :   GET https://api.nuget.org/v3/registration5-gz-semver2/newtonsoft.json/index.json
info :   OK https://api.nuget.org/v3/registration5-gz-semver2/newtonsoft.json/index.json 392ms
info : Restoring packages for C:\dotnetCLI\person-console\person-console.csproj...
info :   GET https://api.nuget.org/v3-flatcontainer/newtonsoft.json/index.json
info :   OK https://api.nuget.org/v3-flatcontainer/newtonsoft.json/index.json 154ms
info :   GET https://api.nuget.org/v3-flatcontainer/newtonsoft.json/13.0.2/newtonsoft.json.13.0.2.nupkg
info :   OK https://api.nuget.org/v3-flatcontainer/newtonsoft.json/13.0.2/newtonsoft.json.13.0.2.nupkg 45ms
info : Installed Newtonsoft.Json 13.0.2 from https://api.nuget.org/v3/index.json with content hash R2pZ3B0UjeyHShm9vG+Tu0EBb2lC8b0dFzV9gVn50ofHXh9Smjk6kTn7A/FdAsC8B5cKib1OnGYOXxRBz5XQDg==.
info : Package 'Newtonsoft.Json' is compatible with all the specified frameworks in project 'C:\dotnetCLI\person-console\person-console.csproj'.
info : PackageReference for package 'Newtonsoft.Json' version '13.0.2' added to file 'C:\dotnetCLI\person-console\person-console.csproj'.
info : Writing assets file to disk. Path: C:\dotnetCLI\person-console\obj\project.assets.json
log  : Restored C:\dotnetCLI\person-console\person-console.csproj (in 1.12 sec).
```

If you use "ctrl+." on "JsonConvert" in the code file, it will now give the option to add "using Newtonsoft.Json".

### Updating the Program
Now that we have the data object and data reader set up, we need to call them from the Program class.

Open "Program.cs" and update the Main method as follows.

```csharp
class Program
{
    static void Main(string[] args)
    {
        var reader = new PersonReader();
        var people = await reader.GetPeopleAsync();
        foreach(var person in people)
            Console.WriteLine(person);

        Console.WriteLine("===============");
    }
}
```

This creates a data reader, awaits the "GetAsync" method, then loops through the results to output to the console.

This code will not compile at this point. Since we are using "await", we need to make the method "async". Fortunately, .NET Core supports an "async Main" method in console applications (since C# 7.1).

```csharp
class Program
{
    static async Task Main(string[] args)
    {
        var reader = new PersonReader();
        var people = await reader.GetPeopleAsync();
        foreach(var person in people)
            Console.WriteLine(person);

        Console.WriteLine("===============");
    }
}
```

The Main method needs to be "async Task" rather than "async void". "async void" is not allowed here.

### Running the Application
With everything in place, we can now run the application. First, go back to the PowerShell window with the service folder and start the service using "dotnet run".

```
PS C:\dotnetCLI\person-api> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dotnetCLI\person-api
```

Then run the console application from the PowerShell window that is open to the console project.

```
PS C:\dotnetCLI\person-console> dotnet run
John Koenig
Dylan Hunt
Turanga Leela
John Crichton
Dave Lister
Laura Roslin
John Sheridan
Dante Montana
Isaac Gampu
===============
```

This displays the data from the web service.

Everything works!

## Solution

Now that we have 3 projects, we will create a solution for them. We'll do this from the command line as well.

### Creating the Solution
Open a new PowerShell (or cmd.exe) tab/window at the root of the project. If you are using Terminal, you can right-click on the current tab and choose "Duplicate Tab" to open a new tab at the same location.

```
PS C:\dotnetCLI>
```

Use "dotnet new sln" to create a solution.

```
PS C:\dotnetCLI> dotnet new sln -n "dotnet7CLI"
The template "Solution File" was created successfully.
```

By using the "-n" argument, we can name the solution whatever we want. This way we can override the default naming if we want something different than the root folder.

```
PS C:\dotnetCLI> ls


    Directory: C:\dotnetCLI


Mode      Length Name
----      ------ ----
d-----           person-api
d-----           person-api-tests
d-----           person-console
-a----       441 dotnet7CLI.sln
```

### Adding Projects to the Solution
To add projects, we use "dotnet sln add".

The following adds the web service project to the solution.

```
PS C:\dotnetCLI> dotnet sln add .\person-api\person-api.csproj
Project `person-api\person-api.csproj` added to the solution.
```

This may seem like a lot of work, but because of auto-completion on the command line, it does not take very long to type. Just type the first few letters of the folder or file and hit "Tab".

We'll do the same for the other 2 projects.

```
PS C:\dotnetCLI> dotnet sln add .\person-api-tests\person-api-tests.csproj
Project `person-api-tests\person-api-tests.csproj` added to the solution.
PS C:\dotnetCLI> dotnet sln add .\person-console\person-console.csproj
Project `person-console\person-console.csproj` added to the solution.
```

Now that we have a populated solution file, we can open the file in Visual Studio 2022.

> Note: I'm using Visual Studio 2022 because we will be updating all 3 projects together, and the tooling is a little bit better for that. Visual Studio 2022 is not required; you could continue using Visual Studio Code to complete the following steps.

With Visual Studio installed, we can type the name of the file to automatically open it.

```
PS C:\dotnetCLI> .\dotnet7CLI.sln
```

In Visual Studio, we see all 3 projects, and we can run the tests in the test explorer.

When using Visual Studio, I'll often create the projects and classes inside the IDE. But it is good to know that we can do this all from the command line in case we need to. Also, if we understand how to do things from the command line, we have a better understanding of what Visual Studio does behind the scenes.

## Dependency Injection

As a last stop, we will take a quick look at the built-in dependency injection container that comes with ASP.NET Core. This allows us to quickly swap out dependencies to make our code more flexible and easier to test and maintain.

For our application, we will inject the data provider into the controller for the service. Then our controller will not need to know about any specific data provider, and we can change it in the service startup. It will also allow us to mock out the data provider for our unit tests so that we have consistent data and behavior.

As a first step, stop the web service (if it is running).

```
      Application is shutting down...
PS C:\dotnetCLI\person-api>
```

### Creating an Abstraction
Before changing out the data provider, we will need to create an abstraction that we can use. In this case, it will be an interface. Then the controller will reference the interface methods instead of the methods on a concrete type.

Visual Studio Code has an "Extract Interface" shortcut, but it does not quite do what I would like it to for this application. So I'll use Visual Studio instead.

In Visual Studio, open the "HardCodedPeopleProvider.cs" file and click on the class name.

Then use "ctrl+." to bring up the built-in refactoring tools. Select "Extract interface...".

In the popup box, change the name of the interface to "IPeopleProvider" and click "OK".

This will create a new file (IPeopleProvider.cs) with the following code.

```csharp
namespace person_api;

public interface IPeopleProvider
{
    List<Person> GetPeople();
    Person? GetPerson(int id);
}
```

*If you do not have Visual Studio, you can create the "IPeopleProvider.cs" file manually and add the code listed above.*

This interface is an abstraction that represents any class that includes these 2 methods.

In addition to creating a new file, Visual Studio updated the "HardCodedPeopleProvider" class to denote that it implements the new interface.

```csharp
    public class HardCodedPeopleProvider : IPeopleProvider
    {
        ...
    }
```

*If you add the interface manually, you will need to add the ': IPeopleProvider' to the class yourself.*

### Updating the Controller
Now that we have the interface, we can update the controller to use the interface for the field.

Here is an update to the "PeopleController" class.

```csharp
public class PeopleController : ControllerBase
{
    IPeopleProvider provider;
    ...
}
```

We have removed the "new()", so we need to assign to this field somewhere. For this, we will create a constructor that will set the field.

```csharp
public class PeopleController : ControllerBase
{
    IPeopleProvider provider;

    public PeopleController(IPeopleProvider provider)
    {
        this.provider = provider;
    }
    ...
}
```

Make sure all of the files are saved.

With this code in place, the controller is no longer responsible for the provider. Whoever creates the controller is responsible for providing an already-instantiated provider (through the constructor parameter).

The body of the constructor sets the private field based on the parameter coming in.

*If you are not familiar with dependency injection, you can take a look at the materials available here: [DI Why: Getting a Grip on Dependency Injection](http://www.jeremybytes.com/Demos.aspx#DI).*

### Running the Application
At this point, we can build and run the application. We'll go back to the command line for this.

When we run "dotnet build" we get a successful build.

```
PS C:\dotnetCLI\person-api> dotnet build
MSBuild version 17.4.1+9a89d02ff for .NET
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api -> C:\dotnetCLI\person-api\bin\Debug\net7.0\person-api.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:03.43
```

And if we use "dotnet run" the service starts successfully.

```
PS C:\dotnetCLI\person-api> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dotnetCLI\person-api
```

But if we navigate to the service in the browser, we get a runtime error.

[http://localhost:9874/people](http://localhost:9874/people)

If we go back to PowerShell, it lists the error. Look for the "Fail" in the output.

```
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.InvalidOperationException: Unable to resolve service for type 'person_api.IPeopleProvider' while attempting to activate 'person_api.Controllers.PeopleController'.
```

An exception is thrown when trying to create an instance of "PeopleController". The error tells us that the system was unable to resolve "IPeopleProvder".

Although we coded the controller to use the interface, we still need to tell the dependency injection container how to map that abstraction to a concrete type.

### Mapping the Interface in the Dependency Injection Container
We can set up this mapping in the "Program.cs" file.

As to where we add this mapping, the default template includes a helpful comment:

```csharp
// Add services to the container.
```

```
// Add services to the container.
builder.Services.AddSingleton<IPeopleProvider, HardCodedPeopleProvider>();
```

The "AddSingleton" method specifies that anywhere we need an "IPeopleProvider", the system should use a "HardCodedPeopleProvider".

"AddSingleton" will only create a single instance of the HardCodedPeopleProvider regardless of how many times we need one. Other options include "AddTransient" and "AddScoped". A discussion of scopes is outside the scope of this walkthrough.

### Running the Service
Now if we stop and restart the service, we can navigate to the service successfully.

```
      Application is shutting down...
PS C:\dotnetCLI\person-api> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dotnetCLI\person-api
```

[http://localhost:9874/people](http://localhost:9874/people)

```json
[{"id":1,"givenName":"John","familyName":"Koenig","startDate":"1975-10-17T00:00:00","rating":6,"formatString":""},{"id":2,"givenName":"Dylan","familyName":"Hunt","startDate":"2000-10-02T00:00:00","rating":8,"formatString":""},{"id":3,"givenName":"Leela","familyName":"Turanga","startDate":"1999-03-28T00:00:00","rating":8,"formatString":"{1} {0}"},{"id":4,"givenName":"John","familyName":"Crichton","startDate":"1999-03-19T00:00:00","rating":7,"formatString":""},{"id":5,"givenName":"Dave","familyName":"Lister","startDate":"1988-02-15T00:00:00","rating":9,"formatString":""},{"id":6,"givenName":"Laura","familyName":"Roslin","startDate":"2003-12-08T00:00:00","rating":6,"formatString":""},{"id":7,"givenName":"John","familyName":"Sheridan","startDate":"1994-01-26T00:00:00","rating":6,"formatString":""},{"id":8,"givenName":"Dante","familyName":"Montana","startDate":"2000-11-01T00:00:00","rating":5,"formatString":""},{"id":9,"givenName":"Isaac","familyName":"Gampu","startDate":"1977-09-10T00:00:00","rating":4,"formatString":""}]
```

### Setting Up Another Provider
Let's set up another data provider so that we can see how this works.

We will use already-created code for this. In addition to the "snippets.txt" file, there are "CSVPeopleProvider.cs" and "People.txt" files in the [Starting Files folder](./StartingFiles/) of the repository.

Copy "CSVPeopleProvider.cs" into the "Models" folder of the web service.

Copy "People.txt" into the root folder of the web service.

As noted above, after copying the files into the appropriate folders, they automatically show up as part of the web service project, whether we use Visual Studio 2022 or Visual Studio Code.

"CSVPeopleProvider" implements the "IPeopleProvider" interface.

```csharp
public class CSVPeopleProvider : IPeopleProvider
```

The class reads data from a text file on the file system (People.txt) and parses it into C# Person objects.

One more thing we need to do is set the "People.txt" file so that it is copied to the output folder of the project.

In Visual Studio, right-click on the "People.txt" file and select "Properties". Change the "Copy to Output Directory" setting to "Copy always".

If you are using Visual Studio Code, you can manually change the "people-api.csproj" file to add this setting. Here is the completed project file. Note the "ItemGroup" section:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

	<PropertyGroup>
		<TargetFramework>net7.0</TargetFramework>
		<Nullable>enable</Nullable>
		<ImplicitUsings>enable</ImplicitUsings>
		<RootNamespace>person_api</RootNamespace>
	</PropertyGroup>

	<ItemGroup>
		<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="7.0.2" />
		<PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
	</ItemGroup>

	<ItemGroup>
		<None Update="People.txt">
			<CopyToOutputDirectory>Always</CopyToOutputDirectory>
		</None>
	</ItemGroup>

</Project>
```

### Updating Configuration
Now that we have the code for the new data provider, we can change the configuration in the "Startup.cs" file.

```csharp
// Add services to the container.
builder.Services.AddSingleton<IPeopleProvider, CSVPeopleProvider>();
```

### Running the Service
Next, stop and restart the service.

```
      Application is shutting down...
PS C:\dotnetCLI\person-api> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dotnetCLI\person-api
```

To show the updated service in action, go back to the PowerShell window for the console application. Then run the application.

```
PS C:\dotnetCLI\person-console> dotnet run
John Koenig
Dylan Hunt
Turanga Leela
John Crichton
Dave Lister
Laura Roslin
John Sheridan
Dante Montana
Isaac Gampu
**Jeremy Awesome**
===============
```

The text file has an extra record: Jeremy Awesome. So we can tell by looking at the data that the service is now getting data from the text file instead of using the hard-coded data provider.

### Fixing Broken Unit Tests
Since we changed the constructor for the controller, our unit tests no longer build.

The problem is in the Setup method of the "PeopleControllerTests" class.

```csharp
public class PeopleControllerTests
{
    PeopleController controller;

    [SetUp]
    public void Setup()
    {
        controller = new PeopleController();
    }
    ...
}
```

We need a data provider to pass as a parameter to the PeopleController constructor. We could use a mocking framework. But to keep things easier for those who are not familiar with mocking, we will use a fake object.

### Adding a Fake Data Reader
To create a fake data reader for testing, we will start with the HardCodedPeopleProvider that we already have. This will save us a lot of typing.

Copy the "HardCodedPeopleProvider.cs" file from the web service project folder into the unit test project folder.

After copying the file, rename it to "FakePeopleProvider.cs".

Open the file in Visual Studio (or Visual Studio Code) and change the name of the class to "FakePeopleProvider". We will leave the rest of the class the same. In addition, we'll change the namespace to "person_api_tests".

```csharp
using person_api;

namespace person_api_tests;

public class FakePeopleProvider : IPeopleProvider
{
    public List<Person> GetPeople() ...

    public Person? GetPerson(int id) ...
}
```

This gives us a separate class that we can use for testing. Now we have more control over the test behavior.

### Updating the Tests
To update the tests, update the "Setup" method to use the "FakePeopleProvider".

```csharp
[SetUp]
public void Setup()
{
    var provider = new FakePeopleProvider();
    controller = new PeopleController(provider);
}
```

Since the "Setup" method runs before each test, all of our tests are now using the fake data provider.

If we re-run the tests, we'll find that they all pass.

```
PS C:\dotnetCLI\person-api-tests> dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  person-api -> C:\dotnetCLI\person-api\bin\Debug\net7.0\person-api.dll
  person-api-tests -> C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-
  tests.dll
Test run for C:\dotnetCLI\person-api-tests\bin\Debug\net7.0\person-api-tests.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.4.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     3, Skipped:     0, Total:     3, Duration: 15 ms - person-api-tests.dll (net7.0)
```

Wrap Up
--------
We've seen how to use .NET 7 and the command-line interface (CLI) to build a web service, unit tests, and a console application.

In addition, we've seen how to use the built-in dependency injection container. This allows us to change out the data provider with a few small changes and also helps us isolate our code for better control over our unit tests.

Happy Coding!
