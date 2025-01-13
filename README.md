## Description
The *WordFinder* project searches for words within a character matrix. 
It started as a simple idea to test if words could be found correctly in a matrix. 
Over time, it evolved into a more organized and flexible solution.

First, I wrote some basic unit tests to ensure the core functionality of searching for words worked correctly. 
Then, I implemented methods to search for words horizontally and vertically. 
However, I realized that adding more search directions directly into the main class would make the code harder to maintain. 
To solve this, I used the *Strategy Pattern*, which allowed me to separate the logic for each search direction into its own class.
This made it easier to add or modify search logic without changing the main class and it will be good for other future search strategy.

As the project became more complex, I added logging to track what was happening during execution, 
making it easier to debug and understand the flow. I also expanded the tests to include more challenging cases, 
like empty word streams, large inputs, or when no words are found.

The validation logic for the matrix was initially mixed into the main class, which made it harder to read. 
To fix this, I used *FluentValidation* to handle common validations, like checking the size and structure of the matrix.
This kept the main class focused on its core job.

Finally, I introduced the *Factory Pattern* to handle the creation of objects like repositories and strategies. 
This made the code easier to configure and reduced repetition when setting up dependencies.

## Tests
The unit tests validate:
- That inputs like the matrix and word stream are correct.
- That the search methods find words as expected.
- Edge cases, like empty inputs, oversized matrices, and unmatched words.

## Concepts Used
- Strategy Pattern
- Factory Pattern
- Dependency Injection (DI)
- Asynchronous Programming
- FluentValidation
- Unit Testing
- Logging

## Author
Jorge Santilli

