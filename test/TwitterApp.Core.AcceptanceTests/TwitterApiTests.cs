namespace TwitterApp.Core.TweetSharpAdapter
{
    using System.Configuration;

    public abstract class TwitterApiTests
    {
        protected string consumerKey;

        protected string consumerSecret;

        protected string accessToken;

        protected string accessTokenSecret;

        protected TwitterApiTests()
        {
            this.consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            this.consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            this.accessToken = ConfigurationManager.AppSettings["AccessToken"];
            this.accessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];
        }
    }
}