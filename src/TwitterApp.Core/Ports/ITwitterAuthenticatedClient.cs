namespace TwitterApp.Core.Ports
{
    using System;
    using System.Threading.Tasks;

    using TwitterApp.Core.Models;

    public interface ITwitterAuthenticatedClient
    {
        long? UserId { get; }

        Task<Uri> GetAuthorizationUri(string oauthCallBackUri);

        Task<long> Authenticate(string requestToken, string requestVerifier);

        Task<Tweet> PostTweet(string content);

        Task DeleteTweet(long tweetId);
    }
}