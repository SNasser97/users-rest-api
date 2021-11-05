Feature: Users Query

    Scenario: Request Successful - Empty array
        Given query made to endpoint with /users
        When a request is made to the endpoint /users
        Then a 200 OK is returned
        And an empty array is returned
