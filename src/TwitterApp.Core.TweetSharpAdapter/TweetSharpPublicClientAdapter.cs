namespace TwitterApp.Core.TweetSharpAdapter
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using TweetSharp;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;

    public class TweetSharpPublicClientAdapter : ITwitterPublicClient
    {
        private TwitterService service;

        public TweetSharpPublicClientAdapter(TwitterService service)
        {
            this.service = service;
        }

        public async Task<TweetSearchResult> SearchTweets(string query, int count = 15, long? maxId = null)
        {
            var search =
                await
                this.service.SearchAsync(new SearchOptions() { Q = query, Count = count, MaxId = maxId })
                    .ConfigureAwait(false);

            search.Response.ThrowIfFailed();

            return new TweetSearchResult()
                       {
                           Tweets =
                               search.Value.Statuses.Select(
                                   tweetResult =>
                                   new Tweet(
                                       tweetResult.Id,
                                       tweetResult.Text,
                                       tweetResult.User.Id,
                                       tweetResult.User.ScreenName,
                                       tweetResult.CreatedDate)).ToArray(),
                           MinId = search.Value.Statuses.Any() ? search.Value.Statuses.Min(x => x.Id) : (long?)null
                       };
        }

        public static Task<TweetSharpPublicClientAdapter> Create(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(accessToken, accessTokenSecret);
            return Task.FromResult(new TweetSharpPublicClientAdapter(service));
        }
    }
}
