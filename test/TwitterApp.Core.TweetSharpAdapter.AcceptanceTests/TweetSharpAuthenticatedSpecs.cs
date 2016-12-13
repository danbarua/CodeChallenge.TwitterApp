namespace TwitterApp.Core.TweetSharpAdapter.AcceptanceTests
{
    using System.Threading.Tasks;

    using TwitterApp.Core.Ports;

    public class TweetSharpAuthenticatedSpecs : AuthenticatedClientSpecs
    {
        protected override Task<ITwitterAuthenticatedClient> GetClient()
        {
            ITwitterAuthenticatedClient fixture = new TweetSharpAuthenticatedClientAdapter(consumerKey, consumerSecret);
            return Task.FromResult(fixture);
        }
    }
}