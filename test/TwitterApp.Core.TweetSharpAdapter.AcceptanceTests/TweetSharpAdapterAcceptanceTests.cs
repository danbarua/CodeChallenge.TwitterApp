namespace TwitterApp.Core.TweetSharpAdapter.AcceptanceTests
{
    using System.Configuration;
    using System.Threading.Tasks;

    using TwitterApp.Core.Ports;

    public class TweetSharpAdapterAcceptanceTests : TwitterApiFacadeAcceptanceTests
    {
        protected override async Task<ITwitterApiFacade> GetFixture()
        {
            var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            var accessToken = ConfigurationManager.AppSettings["AccessToken"];
            var accessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];

            var fixture =
                await
                TweetSharpAdapter.Create(consumerKey, consumerSecret, accessToken, accessTokenSecret)
                    .ConfigureAwait(false);
            return fixture;
        }
    }
}
