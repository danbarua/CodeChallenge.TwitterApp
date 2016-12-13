namespace TwitterApp.Core.Ports
{
    using System.Threading.Tasks;

    using TwitterApp.Core.Models;

    public interface ITwitterPublicClient
    {
        Task<TweetSearchResult> SearchTweets(string query, int count = 15, long? maxId = null);
    }
}
