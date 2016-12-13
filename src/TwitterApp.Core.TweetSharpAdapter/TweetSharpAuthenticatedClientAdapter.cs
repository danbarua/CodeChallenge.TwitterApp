namespace TwitterApp.Core.TweetSharpAdapter
{
    using System;
    using System.Threading.Tasks;

    using TweetSharp;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;

    public class TweetSharpAuthenticatedClientAdapter : ITwitterAuthenticatedClient
    {
        private readonly TwitterService service;

        private long? userId;

        public TweetSharpAuthenticatedClientAdapter(string consumerKey, string consumerSecret)
        {
            this.service = new TwitterService(consumerKey, consumerSecret);
        }

        public long? UserId => this.userId;

        public bool IsAuthenticated => this.userId.HasValue;

        public async Task<Uri> GetAuthorizationUri(string oauthCallBackUri)
        {
            var tokenResult = await this.service.GetRequestTokenAsync(oauthCallBackUri).ConfigureAwait(false);
            tokenResult.Response.ThrowIfFailed();
            var uri = this.service.GetAuthorizationUri(tokenResult.Value);
            return uri;
        }

        public async Task<long> Authenticate(string requestToken, string requestVerifier)
        {
            var oauthToken = new OAuthRequestToken { Token = requestToken };
            var accessTokenResult = await service.GetAccessTokenAsync(oauthToken, requestVerifier).ConfigureAwait(false);
            accessTokenResult.Response.ThrowIfFailed();

            this.service.AuthenticateWith(accessTokenResult.Value.Token, accessTokenResult.Value.TokenSecret);
            var result = await this.service.VerifyCredentialsAsync(new VerifyCredentialsOptions()).ConfigureAwait(false);
            result.Response.ThrowIfFailed();
            this.userId = result.Value.Id;
            return result.Value.Id;
        }

        public async Task<Tweet> PostTweet(string content)
        {
            this.AssertIsAuthenticated();
            var result =
                await this.service.SendTweetAsync(new SendTweetOptions() { Status = content }).ConfigureAwait(false);
            result.Response.ThrowIfFailed();

            var tweetResult = result.Value;
            return new Tweet(tweetResult.Id,
                tweetResult.Text,
                tweetResult.User.Id,
                tweetResult.User.ScreenName,
                tweetResult.CreatedDate);
        }

        public async Task DeleteTweet(long tweetId)
        {
            this.AssertIsAuthenticated();
            var result =
                await this.service.DeleteTweetAsync(new DeleteTweetOptions() { Id = tweetId }).ConfigureAwait(false);
            result.Response.ThrowIfFailed();
        }

        private void AssertIsAuthenticated()
        {
            if (!this.IsAuthenticated)
            {
                throw new InvalidOperationException("You must call .Authenticate() before attempting operations on behalf of a User.");
            }
        }
    }
}