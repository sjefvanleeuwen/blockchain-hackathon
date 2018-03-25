@needsTestRPC
Feature: Self reliance
	Citizens that are part of the social care act need products and services to be self reliant
	For this they receive endorsements from district nurses and their township 

Scenario: Obtain product endorsment
	Given "Richard" gets an indication for the social care act from "Bob"
	And "Eline" gives an endorsement for product "Electric Bike"
	When "Jahir" asks if the endorsement is valid
	Then the result should be "True"
