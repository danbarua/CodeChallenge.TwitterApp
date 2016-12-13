namespace TwitterApp.Web.Nancy.Tests.Tweet
{
    using System;

    using FakeItEasy;

    using FluentAssertions;

    using global::Nancy;
    using global::Nancy.Testing;

    using Ploeh.AutoFixture;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;
    using TwitterApp.Nancy.AspNet.Auth;
    using TwitterApp.Nancy.AspNet.Tweet;

    using Xunit;

    public class TweetModuleTests
    {
        [Fact]
        public void Unauthenticated_Get_should_return_403()
        {
            //arrange
            var twitterUserTracker = new TwitterUserTracker();

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<TweetModule>();
                    with.Dependency<ITwitterUserTracker>(twitterUserTracker);
                });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Get("/tweet");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void Authenticated_Get_should_return_Status_View()
        {
            //arrange
            var twitterApi = A.Fake<ITwitterAuthenticatedClient>();
            var twitterUserTracker = A.Fake<ITwitterUserTracker>();
            const long TwitterUserId = 123L;
            var twitterUser = new TwitterUser(TwitterUserId);

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<TweetModule>();
                    with.Dependency<ITwitterUserTracker>(twitterUserTracker);
                    with.RequestStartup(
                        (_, __, context) =>
                            {
                                context.CurrentUser = twitterUser;
                            });
                });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Get("/tweet");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.GetViewName().Should().Be("Status");
        }

        [Fact]
        public void Authenticated_Post_Should_Submit_Tweet_and_return_to_populated_Status_View()
        {
            //arrange
            var fixture = new Fixture();
            var twitterApi = A.Fake<ITwitterAuthenticatedClient>();
            var twitterUserTracker = A.Fake<ITwitterUserTracker>();
            const long TwitterUserId = 123L;
            var twitterUser = new TwitterUser(TwitterUserId);
            const string Content = "#DreamJob";

            var returnedTweet = 
                new Tweet(
                    fixture.Create<long>(),
                    Content,
                    TwitterUserId,
                    fixture.Create<string>(),
                    fixture.Create<DateTime>());

            A.CallTo(
                () =>
                twitterUserTracker.GetAuthenticatedTwitterClientForUser(A<long>.That.Matches(x => x == TwitterUserId)))
                .Returns(twitterApi);

            A.CallTo(() => twitterApi.PostTweet(A<string>.That.Matches(x => x == Content))).Returns(returnedTweet);

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<TweetModule>();
                    with.Dependency<ITwitterUserTracker>(twitterUserTracker);
                    with.RequestStartup(
                        (_, __, context) =>
                        {
                            context.CurrentUser = twitterUser;
                        });
                });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Post("/tweet", req =>
            {
                req.FormValue("Content", Content);
            });

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.GetViewName().Should().Be("Status");

            var viewModel = response.GetModel<Tweet>();
            viewModel.Should().NotBeNull();
            viewModel.ShouldBeEquivalentTo(returnedTweet);
        }

        [Fact]
        public void Post_Delete_should_Delete_the_Tweet_and_redirect_to_Status_View()
        {
            //arrange
            var fixture = new Fixture();
            var twitterApi = A.Fake<ITwitterAuthenticatedClient>();
            var twitterUserTracker = A.Fake<ITwitterUserTracker>();
            const long TwitterUserId = 123L;
            var twitterUser = new TwitterUser(TwitterUserId);
            const string Content = "#DreamJob";

            var returnedTweet =
                new Tweet(
                    fixture.Create<long>(),
                    Content,
                    TwitterUserId,
                    fixture.Create<string>(),
                    fixture.Create<DateTime>());

            A.CallTo(
                () =>
                twitterUserTracker.GetAuthenticatedTwitterClientForUser(A<long>.That.Matches(x => x == TwitterUserId)))
                .Returns(twitterApi);

            A.CallTo(() => twitterApi.PostTweet(A<string>.That.Matches(x => x == Content))).Returns(returnedTweet);

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<TweetModule>();
                    with.Dependency<ITwitterUserTracker>(twitterUserTracker);
                    with.RequestStartup(
                        (_, __, context) =>
                        {
                            context.CurrentUser = twitterUser;
                        });
                });

            var browser = new Browser(bootstrapper);

            //act - create tweet then delete it
            var response = browser.Post(
                "/tweet",
                req =>
                    {
                        req.FormValue("Content", Content);
                    })
                    .Then.Post($"/tweet/delete/{returnedTweet.Id}");

            //assert
            response.ShouldHaveRedirectedTo("/tweet?message=Tweet%20Deleted");
            A.CallTo(() => twitterApi.DeleteTweet(A<long>.That.Matches(x => x == returnedTweet.Id)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
