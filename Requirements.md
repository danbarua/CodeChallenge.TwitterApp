##Requirements:
Twitter App
Create a C# application (preferably a Web App), that is able to interact with Twitter and provide the
following services:

###Search for Tweets

https://dev.twitter.com/rest/reference/get/search/tweets

As a user of the application you should be able to search for tweets given a keyword. Twitter also
provides searches based on the parameters. These are optional for
you to include in your app.

###Post Tweets on your account

https://dev.twitter.com/rest/reference/post/statuses/update

As a user of the application, you should be able to post tweets to your timeline. For the purposes of this
exercise, it is sufficient to support text-only tweets.

###Delete Tweets on your account

https://dev.twitter.com/rest/reference/post/statuses/destroy/id

As a user of the application, you should be able to delete tweets from your timeline given a tweet id.

####Resources and Hints

- You can either implement the OAuth flow or use a hard-coded application token -
https://dev.twitter.com/oauth/overview/application-owner-access-tokens

- Twitter uses OAuth 1.0a as a means of authenticating and authorising requests -
https://dev.twitter.com/oauth/overview/authorizing-requests

- We recommend using a debugging tool like Postman (https://www.getpostman.com) to help
with development of the application.

- Please use this opportunity to showcase your coding skills through a well structured, well
tested application.