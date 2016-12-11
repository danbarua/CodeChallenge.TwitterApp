namespace TwitterApp.Core.AcceptanceTests
{
    using System;
    using System.Threading.Tasks;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;

    public class TweetSharpAdapter : ITwitterApiFacade
    {
        public Task<TweetSearchResult> SearchTweets()
        {
            throw new NotImplementedException();
        }

        public Task PostTweet(string content)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTweet(long tweetId)
        {
            throw new NotImplementedException();
        }
    }
}
