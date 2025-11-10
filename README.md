# Introduction  
Welcome to **PharmConnect WHMS (Warehouse Inventory Management System)**, a solution meticulously crafted to streamline and optimize your inventory operations.  

Now powered by **ASP.NET Core 9.0**, the latest cutting-edge technology from Microsoft, PC WHMS is faster than ever and showcases the future of modern web development. With its fully decoupled **headless API** architecture, PC WHMS enables seamless integration between the back end and front end, offering unparalleled flexibility and performance.  

The back end is built using **Clean Architecture**, **CQRS**, **MediatR**, and the **Repository Pattern**, ensuring maintainability and scalability for enterprise-grade applications. On the front end, **ASP.NET Core Razor Pages** and **Vue.js** come together to create a dynamic and user-friendly interface.  

## Key Features  
PC WHMS provides a comprehensive suite of capabilities:  
- **Sales, Purchase, Delivery, and Goods Receive**  
- **Transfer, Adjustment, Return, and Scrapping**  
- **Stock Count and Detailed Reporting Functionalities**  

# Monolithic Clean Architecture  

PC WHMS is built using a **Monolithic Clean Architecture** approach, ensuring a structured and simplified development process. By keeping all components within a single codebase, dependency management is streamlined, eliminating the risk of a **dependency nightmare**. This approach consolidates all dependencies in one place, ensuring compatibility and coherence across the entire system.  

Additionally, it simplifies **deployment**, as all code resides in a single repository with a well-optimized pipeline, reducing complexity. The **cohesive project structure** provides a clear and consistent source code pattern, making it easier for developers to understand and maintain the system. With the combination of Clean Architecture, CQRS, and MediatR, PC WHMS delivers a **scalable, maintainable, and enterprise-ready solution**.

# Technical Features
- **ASP.NET Core 9.0 Headless API** (Back End)
  - Clean Architecture
  - CQRS with MediatR
  - Repository Pattern
  - Entity Framework Core (EF Core) for data access
  - AutoMapper for object mapping
  - FluentValidation for input validation
  - Serilog for logging
  - Support for file uploads and downloads (images/documents)
  - Secure authentication and authorization with ASP.NET Identity + JWT
- **ASP.NET Core Razor Pages with a Simple & Modern UI** (Front End)  
  - **Effortless** dynamic interactivity using Vue.js **without any build system**  
  - **Ready-to-use** industry-leading Syncfusion UI components (free community edition)  
  - **Lightweight and straightforward** API communication with Axios  
  - **Easy-to-customize** responsive UI powered by the AdminLTE template  

# Functional Features

- **Customer Management**
  - Customer Group, Category, Details, and Contacts
- **Sales Management**
  - Sales Order, Sales Return, Sales Reports
- **Vendor Management**
  - Vendor Group, Category, Details, and Contacts
- **Purchase Management**
  - Purchase Order, Purchase Return, Purchase Reports
- **Warehouse Operations**
  - Unit Measure, Product Group, Products
  - Delivery Order, Goods Receive
  - Transfers, Adjustments, Scrapping, Stock Counts
- **Reporting**
  - Transaction Report, Stock Report, Movement Report
- **System Settings**
  - Company Settings, Tax Configuration, User Management
  - Number Sequence for systematic tracking
- **Analytics and Logs**
  - Error Logs, Analytic Logs
- **Authentication & Membership**
  - Secure user authentication and role-based access control

# Run The Project: Visual Studio  

Getting started is **easy**! Thanks to **Monolithic Clean Architecture**, everything is structured and streamlined.
Plus, even though this project uses a **modern JavaScript framework like Vue.js**, it **does not require a build system** — just use Visual Studio to run and build effortlessly.

1. Open the project using Visual Studio.  
2. Update the connection string in `appsettings.json` to match your SQL Server database.  
3. Clean and build the solution:  
   - Right-click the solution > Clean  
   - Right-click the solution > Build  
4. Run the project:  
   - Click the green "play" button in the Visual Studio toolbar.  

> **Note**: The database will be created automatically if it does not exist.  