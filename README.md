# CodeChallenge.TwitterApp

Requirements:
- .Net 4.6.1
- Asp.Net

Issues:
- Code quality is... hackathon grade
- ..as are the tests
 - Integration testing with Twitter API requires OAuth access tokens
 - When attempting to Post and Delete a Tweet on behalf of a user it will
   spawn a Twitter authorization page in a browser window, 
   requiring user interaction before the test can proceed.
   This won't work on a CI server!
 - There are no system level integration or browser automation tests
   These are too time consuming to set up in the context of a code challenge
 - Nancy web app tests use mocks and are coupled to internal implementation details
- User experience is not great
- Some shortcuts taken in the design of the web app in order to ship
 - eg. instead of using Post-Redirect-Get, Posting a Tweet returns the View
 - Post `/tweet/delete/{id}` is not restful at all
 - No error handling
 - No request validation (watch it blow up when you submit an empty query)
 - No security (XSRF, XSS, etc)
 - Assumes it will run on a single server, uses singleton in-mem user cache
 
