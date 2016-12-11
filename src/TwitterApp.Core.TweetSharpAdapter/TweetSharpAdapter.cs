namespace TwitterApp.Core.TweetSharpAdapter
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using TweetSharp;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;

    public class TweetSharpAdapter : ITwitterApiFacade
    {
        private TwitterService service;

        public TweetSharpAdapter(TwitterService service)
        {
            this.service = service;
        }

        public async Task<TweetSearchResult> SearchTweets(string query, int count = 15, long? maxId = null)
        {
            var search = await this.service.SearchAsync(
                new SearchOptions()
                    {
                        Q = query,
                        Count = count,
                        MaxId = maxId
                    }).ConfigureAwait(false);

            search.Response.ThrowIfFailed();

            return new TweetSearchResult()
                       {
                           Tweets =
                               search.Value.Statuses.Select(
                                   x => new Tweet(x.Id, x.Text, x.Author.ScreenName, x.CreatedDate)).ToArray(),
                           MinId = search.Value.Statuses.Min(x => x.Id)
                       };
        }

        public Task PostTweet(string content)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTweet(long tweetId)
        {
            throw new NotImplementedException();
        }

        public static Task<TweetSharpAdapter> Create(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(accessToken, accessTokenSecret);
            return Task.FromResult(new TweetSharpAdapter(service));
        }
    }
}
