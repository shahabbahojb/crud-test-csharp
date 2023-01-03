Feature: Manage Customers in System

    Scenario: Customers get created successfully
        When I create customer with following details
          | FirstName | LastName   | DateOfBirth | PhoneNumber | Email               | BankAccountNumber   |
          | Shahab    | Bahojb     | 2000-01-01  | 09381524341 | bahojb.sb@gmail.com | 1234-5678-9123-3456 |
          | TestName  | TestFamily | 2001-01-01  | 09381524342 | test.sb@gmail.com   | 1238-5678-9123-3456 |
        Then the customers are created successfully
        
        
    Scenario:  Get validation error
        When I create customer with invalid input
          | FirstName | LastName   | DateOfBirth | PhoneNumber | Email               | BankAccountNumber   |
          | Shahab    | Bahojb     | 2000-01-01  | 09381524341 | fakeemail           | 1234-5678-9123-3456 |
        Then Get Validation Error

    Scenario: Customers get deleted successfully
        Given the following customers are in the system
          | Id | FirstName | LastName   | DateOfBirth | PhoneNumber | Email               | BankAccountNumber   |
          | 1  | Shahab    | Bahojb     | 2000-01-01  | 09381524341 | bahojb.sb@gmail.com | 1234-5678-9123-3456 |
          | 2  | TestName  | TestFamily | 2001-01-01  | 09381524342 | test.sb@gmail.com   | 1238-5678-9123-3456 |
        When I these customers get deleted
        Then the customers are deleted successfully
        
        
    Scenario: Customers List works successfully
        When I get customers list
        Then the customers list data is correct
        
    Scenario: Edit Customer works successfully
        Given This Customer is in the system
        When I update customer with these data
          | Id | FirstName | LastName | DateOfBirth | PhoneNumber | Email               | BankAccountNumber   |
          | 1  | Shahab New Name    | Bahojb Update   | 2000-01-20  | 09381524342 | newgmail.sb@gmail.com | 1234-5678-9123-3785 |
        Then the customers updates correctly