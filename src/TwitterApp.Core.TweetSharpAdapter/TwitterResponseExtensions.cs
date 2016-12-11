namespace TwitterApp.Core.TweetSharpAdapter
{
    using System.Linq;

    using TweetSharp;

    using TwitterApp.Core.Ports;

    public static class TwitterResponseExtensions
    {
        public static bool Success(this TwitterResponse response)
        {
            return response.Error == null && response.InnerException == null;
        }

        public static void ThrowIfFailed(this TwitterResponse response)
        {
            if (!response.Success())
            {
                throw new TwitterException(
                    response.Errors.errors?.FirstOrDefault()?.Message
                        ?? response.InnerException.Message,
                    response.InnerException);
            }
        }
    }
}