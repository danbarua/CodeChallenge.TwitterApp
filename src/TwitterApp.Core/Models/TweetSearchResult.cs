namespace TwitterApp.Core.Models
{
    public class TweetSearchResult
    {
        public Tweet[] Tweets { get; set; }

        public long? MinId { get; set; }
    }
}