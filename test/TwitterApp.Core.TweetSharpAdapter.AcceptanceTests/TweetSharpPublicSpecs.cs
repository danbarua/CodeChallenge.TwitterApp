namespace TwitterApp.Core.TweetSharpAdapter.AcceptanceTests
{
    using System.Threading.Tasks;

    using TwitterApp.Core.Ports;

    public class TweetSharpPublicSpecs : PublicClientSpecs
    {
        protected override async Task<ITwitterPublicClient> GetClient()
        {
            var fixture =
                await
                TweetSharpPublicClientAdapter.Create(consumerKey, consumerSecret, accessToken, accessTokenSecret)
                    .ConfigureAwait(false);

            return fixture;
        }
    }
}
