@needsTestRPC
Feature: ContractMultiplication
	In order to avoid silly mistakes
	As a ethereum user
	I want to multiply a number

Scenario: Multiplication by 8
Given I have deployed a multiplication contract with multipler of 8
	When I call multiply using 8
	Then the multiplication result should be 64


Scenario Outline: Multiplication
	Given I have deployed a multiplication contract with multipler of <initialMultiplier>
	When I call multiply using <multiplier>
	Then the multiplication result should be <multiplicationResult>

	Examples:
	| initialMultiplier | multiplier | multiplicationResult |
	| 7                 | 7          | 49                   |
	| 3                 | 2          | 6                    |
	| 3                 | 3          | 9                    |