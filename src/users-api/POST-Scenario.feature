Scenario: 201 User created by POST request
    Given POST request is emitted
    Given the request body has a DateOfBirth
    Given the request body has an Email
    Given the request body has a FirstName
    When I emit a POST HTTP request
    Then a 201 POST HTTP response is returned
    And a response id is returned

Scenario: 400 User not created by POST request - invalid DateOfBirth
    Given POST request is emitted
    Given the request body has an invalid DateOfBirth
    Given the request body has an Email
    Given the request body has a FirstName
    When I emit a POST HTTP request
    Then a 400 HTTP response is returned
    And no response id is returned

Scenario: 400 User not created by POST request - email already exists
    Given POST request is emitted
    Given the request body has an valid DateOfBirth
    Given the request body has an existing Email
    Given the request body has a FirstName
    When I emit a POST HTTP request
    Then a 400 HTTP response is returned
    And no response id is returned