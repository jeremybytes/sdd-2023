# Software Design & Development (SDD 2023) Presentations  

All of the slides and code samples for Jeremy's presentations at SDD 2023 (15-19 May, 2023).  

## Topics

**Get Comfortable with .NET 7 and the CLI**  
*Coding Level: 4*  
*Advanced Level: 2*  

Command-line interfaces (CLI) can be very powerful. The same is true in the .NET world. So let’s get comfortable with creating, running, and testing applications using the command-line interface. We’ll create a self-hosted web service and then write an application to use that service. Unit tests will make sure things work along the way. Whether you’re new to .NET 7 or have been using .NET 7 with Visual Studio, this session will help you get up-to-speed in this powerful environment. Attendees will need to be familiar with C#.  

* Slides: [SLIDES-dotnet-cli.pdf](./01-dotnet-cli/SLIDES-dotnet-cli.pdf)  
* Code: [01-dotnet-cli/](./01-dotnet-cli/)

---

**DI Why? Getting a Grip on Dependency Injection**  
*Coding Level: 4*  
*Advanced Level: 2*  

Many of our modern frameworks have Dependency Injection (DI) built in. But how do you use that effectively? We need to look at what DI is and why we want to use it. We’ll look at the problems caused by tight coupling. Then we’ll use some DI patterns such as constructor injection and property injection to break that tight coupling. We’ll see how loosely-coupled applications are easier to extend and test. With a better understanding of the basic patterns, we’ll remove the magic behind DI containers so that we can use the tools appropriately in our code. Attendees will need some familiarity with C# (at least six months or so), but no experience with dependency injection is necessary.  

* Slides: [SLIDES-dependency-injection.pdf](./02-dependency-injection/SLIDES-dependency-injection.pdf)  
* Code: [02-dependency-injection/](./02-dependency-injection/)

---

**Safer Code: Nullability and Null Operators in C#**  
*Coding Level: 4*  
*Advanced Level: 2*  

New projects in C# have nullable reference types enabled by default. This helps make the intent of our code more clear, and we can catch potential null references before they happen. But things can get confusing, particularly when migrating existing projects. Today we will look at the safeguards that nullability provides as well as the problems we still need to watch out for ourselves. In addition, we will learn about the various null operators in C# (including null conditional, null coalescing, and null forgiving operators). These can make our code more expressive and safe. Attendees will need some familiarity with C# (at least six months or so).  

* Slides: [SLIDES-nullability.pdf](./03-nullability/SLIDES-nullability.pdf)  
* Code: [03-nullability/](./03-nullability/)

---

**Get Func-y: Delegates in C#**  
*Coding Level: 4*  
*Advanced Level: 2*  

Delegates are the gateway to functional programming. So lets understand delegates and how we can change the way we program by using functions as parameters, variables, and properties. In addition, we’ll see how the built in delegate types, Func and Action, are waiting to make our lives easier. We’ll see how delegates can add elegance, extensibility, and safety to our programming.  

* Slides: [SLIDES-delegates.pdf](./04-delegates/SLIDES-delegates.pdf)
* Code: [04-delegates/](./04-delegates/)

---

**Better Parallel Code with C# Channels**  
*Coding Level: 4*  
*Advanced Level: 3*  

Producer/consumer problems show up in a lot of programming scenarios, including data processing and machine learning. Channels give us a thread-safe way to communicate between producers and consumers, and we can run them all concurrently. In this presentation, we will explore channels by comparing parallel tasks with continuations to using a producer/consumer model. In the end, we’ll have another tool in our toolbox to help us with concurrent programming. Attendees will need to have some familiarity with C# (six months or so), experience using Task and continuations would be helpful but is not required.

* Slides: [SLIDES-channels.pdf](./05-channels/SLIDES-channels.pdf)
* Code: [05-channels/](./05-channels/)

---