
This project is organized into three main components: **Bootstraper**, **Modules**, and **Shared**. Below is a brief explanation of each:

## Bootstraper

The `bootstraper` folder contains the startup logic for the application. 
It is responsible for initializing dependencies, configuring services, and setting up the application's environment before execution. 
This may include dependency injection setup, configuration loading, and application entry points. 
It contain API project and it is repsonsible for exposing the endpoints for our modules
API will serve as a gateway to our project. Program.cs

## Modules

The `modules` folder contains the core features of the application, organized into separate, self-contained units. 
It is organized using DDD and vertical slice Arhitecture
Each module typically represents a distinct business domain or functionality, such as user management, authentication, or reporting. 
Modules encapsulate their own services, controllers, and data access logic, promoting modularity and maintainability.
It contains events folder for communication between the modules

## Shared

The `shared` folder contains reusable components, utilities, and resources that are used across multiple modules. 
This may include common helper functions, data models, constants, and interfaces. 
The shared folder helps reduce code duplication and ensures consistency throughout the project.
It contains common contracts and messaging

---


