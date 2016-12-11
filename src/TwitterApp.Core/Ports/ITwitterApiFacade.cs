namespace TwitterApp.Core.Ports
{
    using System.Threading.Tasks;

    using TwitterApp.Core.Models;

    public interface ITwitterApiFacade
    {
        Task<TweetSearchResult> SearchTweets();

        Task PostTweet(string content);

        Task DeleteTweet(long tweetId);
    }
}
