# Domain Driven Design

## What is DDD?

This project is based around the DDD concept and provides a framework to make **DDD** based development easier to implement. DDD is [defined in the Wikipedia](https://en.wikipedia.org/wiki/Domain-driven_design) as below:

> **Domain-driven design** (**DDD**) is an approach to software development for complex needs by connecting the implementation to an evolving model. The premise of domain-driven design is the following:
>
> - Placing the project's primary focus on the core domain and domain logic;
> - Basing complex designs on a model of the domain;
> - Initiating a creative collaboration between technical and domain experts to iteratively refine a conceptual model that addresses particular domain problems.

## Layers

This project follows DDD principles and patterns to achieve a layered application model which consists of four fundamental layers:

- **Presentation Layer**: Provides an interface to the user. Uses the *Application Layer* to achieve user interactions. e.g. MVC Layer, API Layer
- **Application Layer**: Mediates between the Presentation and Domain Layers. Orchestrates business objects to perform specific application tasks. Implements use cases as the application logic.
- **Domain Layer**: Includes business entities and their business rules. This is the heart of the application.
- **Infrastructure Layer**: Provides generic technical capabilities that support higher layers, for example persistence and services 

## Dependency Management

One of the key difference of a DDD application structure to a more tradition layered architecture is the dependencies between layers. In a layer application architecture  each layer communicates with the layer directly below it. In a DDD structure the Domain is the core of the application. This leads to some of the dependencies being inverted. This architecture is also similar to a ports and adapters architecture, also known as hexagonal architecture.

## Contents

* **Domain Layer**
  * [Entities ](Entities.md)
  * [Aggregate Roots](AggregateRoots.md)
  * Value Objects
  * [Repositories](Repositories.md)
  * Domain Services
  * Specifications
* **Application Layer**
  * [Application Services](Application-Services.md)
  * Data Transfer Objects (DTOs)
  * Unit of Work