namespace TwitterApp.Core.Ports
{
    using System.Threading.Tasks;

    using TwitterApp.Core.Models;

    public interface ITwitterApiFacade
    {
        Task<TweetSearchResult> SearchTweets(string query, int count = 15, long? maxId = null);

        Task PostTweet(string content);

        Task DeleteTweet(long tweetId);
    }
}
