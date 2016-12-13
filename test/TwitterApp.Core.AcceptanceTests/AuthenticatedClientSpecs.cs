namespace TwitterApp.Core.TweetSharpAdapter
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Nancy;
    using Nancy.Hosting.Self;

    using TwitterApp.Core.AcceptanceTests;
    using TwitterApp.Core.Ports;

    using Xunit;

    public abstract class AuthenticatedClientSpecs : TwitterApiTests, IDisposable
    {
        private readonly NancyHost host;

        protected abstract Task<ITwitterAuthenticatedClient> GetClient();

        private const string OAuthInterceptorUri = "http://localhost:5000/";

        protected AuthenticatedClientSpecs()
            : base()
        {
            host = new Nancy.Hosting.Self.NancyHost(new Uri(OAuthInterceptorUri), new DefaultNancyBootstrapper(), new HostConfiguration()
                       { UrlReservations = new UrlReservations() { CreateAutomatically = true}
                       });
            this.host.Start();
        }

        [Fact]
        public async Task Can_get_callback_uri()
        {
            var client = await this.GetClient();
            var uri = await client.GetAuthorizationUri(string.Concat(OAuthInterceptorUri, HorribleOAuthHacks.OAuthCallBack));
            uri.ToString().Should().StartWith("https://api.twitter.com/oauth/authorize?oauth_token=");
        }

        [Fact]
        public async Task Attempting_to_Post_without_authenticating_throws_InvalidOperationException()
        {
            var client = await this.GetClient();
            var ex = await Record.ExceptionAsync(() => client.PostTweet("test"));
            ex.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task Attempting_to_Delete_without_authenticating_throws_InvalidOperationException()
        {
            var client = await this.GetClient();
            var ex = await Record.ExceptionAsync(() => client.DeleteTweet(123));
            ex.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task Can_Post_and_delete_a_Tweet_when_authenticated()
        {
            var client = await this.GetClient();
            await this.Authenticate(client);

            client.UserId.Should().NotBeNull();

            var tweet = await client.PostTweet("test");

            tweet.AuthorUserId.Should().Be(client.UserId, "expect Tweet Id to be initialized");

            await client.DeleteTweet(tweet.Id);
        }

        public void Dispose()
        {
            this.host?.Dispose();
            HorribleOAuthHacks.Reset();
        }

        private async Task Authenticate(ITwitterAuthenticatedClient client)
        {
            // shared mutable state - won't be able to run tests in parallel
            HorribleOAuthHacks.Reset();
            var uri = await client.GetAuthorizationUri(string.Concat(OAuthInterceptorUri, "/", HorribleOAuthHacks.OAuthCallBack));

            //requires user interaction to authorize the app - brittle
            Process.Start(uri.ToString());

            //wait until the user has completed the oauth flow - might time out or hang
            await Wait.Until(() => HorribleOAuthHacks.IsOAuthSigninCompleted);

            //authenticate the service
            await client.Authenticate(HorribleOAuthHacks.OAuthToken, HorribleOAuthHacks.OAuthVerifier);
        }
    }
}