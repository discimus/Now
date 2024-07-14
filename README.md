
## What is this


It is a POC (Proof of Concept), a simple app to implement some concepts and verify a set of best practices. The following items are verified:

- Non-anemic entities
- Layered modules in a Clean Architecture design to decouple parts
- Unit tests for layers
- Unit tests for repositories using in-memory SQLite connection string

The main app, Now.Api, is a RESTful API with CRUD functionality.

![image](https://github.com/user-attachments/assets/adeb75d5-6f59-4cc2-a791-abdc50616767)


The database is a simple table named "tasks" in a SQLite database located in the following folders:

- Now.Api/Database
- Now.Samples/Database

![image](https://github.com/user-attachments/assets/9da8cb1d-f9bb-46e1-9552-0b6fa4fda077)
