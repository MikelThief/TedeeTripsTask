# TedeeTripsTask
This is solution to the recruitment task requested by Tedee.

## Word on architecture
The solution uses three-layer (onion) approach (Application with extracted composition root + API, Domain, Infrastructure).
Tests are compacted into a single test project for simplicity.

There are two types of interactions: queries and commands. These establish CQS.
Queries are not subject to decision making, hence they are entirely handler in Application layer.

Queries related to similar subject were aggregated within a single handler, as the space taken by handling code only is tiny. This is to avoid duplication of boilerplate handler setup that is the same for all similar queries. In bigger scale scnarios, those could be separated.

Commands, on contrary, are handled with the use of the domain model contained within Domain layer and Application layer.
The decision making is appropriately splitted between entities and valueobjects. The only exception from adherence to domain purity is trip name uniqueness. This could be extracted into dedicated domain service, but the concept's simplicity fits well into ordinary processing pipeline without having any noticable downsides.

Processing within domain model strongly relies on functional programming features available in C#. There's also strong use of the type system which allows compiler to work with doubled-effort ensuring correctness of the program in design-time/compile-time. Railway-oriented programming also makes the code much easier to read and understand, because if statements are almost non-existing.

# Word on tests
There are few tests, which cover most important business requirements. They are designed in a way that can let programmers unfamiliar with the code know what the service is expected to do and how it performs in common scenarios. Thus tests act like documentation showing, for example, the lifetime of the entity.
Some unit tests are provided to show how convienient pure domain model testing is. Tests related to entities were omitted (becuase they are not much different in its look), but the invariant are also covered in integration tests.
