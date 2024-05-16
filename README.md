Issues with previous code:

Lack of defensive coding techniques- null checks, try catch blocks

plain text queries are at risk of dependency injection





changes:

added a unique constraint to the Person.Name. Added error handling/logging to the Person/Post.

added null check to create astronautduty method to ensure the duty is for a person that exists.

added an 'IsCurrent' bool to the AstronautDuty model to determine if the duty is current or not.

Added logic to prevent the user from entering multiple current duties to the same astronaught + error handling/logging

added logic to ensure current duties do not have an end date

added logic to esure a Person's Previous Duty End Date is set to the day before the New Astronaut Duty Start Date when a new Astronaut Duty is received for a Person

added logic to ensure A Person's Career End Date is one day before the Retired Duty Start Date

added tests folder with proposed unit tests to test the core functionality of the API

Created a web ui with react to access the project
