namespace TwitterApp.Nancy.AspNet.Tweet
{
    using global::Nancy;
    using global::Nancy.Security;

    using TwitterApp.Nancy.AspNet.Auth;

    public class TweetModule : NancyModule
    {
        public TweetModule(TwitterUserTracker userTracker) : base("/tweet")
        {
            this.RequiresAuthentication();

            this.Get["/"] = _ => this.View["Status"];

            this.Post["/", true] = async (_, __) =>
                {
                    var twitterApi = userTracker.GetAuthenticatedTwitterClientForUser(this.GetCurrentUser().TwitterUserId);
                    var tweet = await twitterApi.PostTweet(this.Request.Form.Content);
                    this.ViewBag.Message = "Tweet Sent";

                    // should be post-redirect-get
                    return this.View["Status", tweet];
                };

            this.Post["/delete/{id:long}", true] = async (_, __) =>
                {
                    var twitterApi = userTracker.GetAuthenticatedTwitterClientForUser(this.GetCurrentUser().TwitterUserId);
                    await twitterApi.DeleteTweet(_.id);
                    return Response.AsRedirect("/tweet");
                };
        }

        private TwitterUser GetCurrentUser()
        {
            return this.Context.CurrentUser as TwitterUser;
        }
    }
}