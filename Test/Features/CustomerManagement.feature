Feature: Manage Customers in System

    Scenario: Customers get created successfully
        When I create customer with following details
          | FirstName | LastName   | DateOfBirth | PhoneNumber | Email               | BankAccountNumber   |
          | Shahab    | Bahojb     | 2000-01-01  | 09381524341 | bahojb.sb@gmail.com | 1234-5678-9123-3456 |
          | TestName  | TestFamily | 2001-01-01  | 09381524342 | test.sb@gmail.com   | 1238-5678-9123-3456 |
        Then the customers are created successfully

    Scenario: Customers get deleted successfully
        Given the following customers are in the system
          | Id | FirstName | LastName   | DateOfBirth | PhoneNumber | Email               | BankAccountNumber   |
          | 1  | Shahab    | Bahojb     | 2000-01-01  | 09381524341 | bahojb.sb@gmail.com | 1234-5678-9123-3456 |
          | 2  | TestName  | TestFamily | 2001-01-01  | 09381524342 | test.sb@gmail.com   | 1238-5678-9123-3456 |
        When I thise customers get deleted
        Then the customers are deleted successfully