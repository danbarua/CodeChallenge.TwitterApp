using System;

namespace TwitterApp.Web.Nancy.Tests.Auth
{

    using FakeItEasy;

    using FluentAssertions;

    using global::Nancy;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.Testing;

    using Ploeh.AutoFixture;

    using TwitterApp.Core.Ports;
    using TwitterApp.Nancy.AspNet.Auth;

    using Xunit;

    public class AuthModuleTests
    {
        [Fact]
        public void Get_Auth_Sign_In_Populates_Twitter_Auth_Link()
        {
            //arrange
            var twitterApi = A.Fake<ITwitterAuthenticatedClient>();
            var callBackUrl = new Uri("http://api.faketwitter.com/?foo=bar");
            A.CallTo(() => twitterApi.GetAuthorizationUri(A<string>._)).Returns(callBackUrl);

            var twitterUserTracker = new TwitterUserTracker();

            var bootstrapper = new TestBootstrapper(
                with =>
                    {
                        with.Module<AuthModule>();
                        with.Dependency<ITwitterAuthenticatedClient>(twitterApi);
                        with.Dependency<ITwitterUserTracker>(twitterUserTracker);
                    });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Get("/auth/signin");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.GetViewName().Should().Be("SignIn");
            var authUrl = (Uri)(response.Context.ViewBag.AuthUrl);
            authUrl.ShouldBeEquivalentTo(callBackUrl);
        }

        [Fact]
        public void Get_OAUth_CallBack_with_access_token_should_sign_the_user_in()
        {
            //arrange
            var fixture = new Fixture();
            var oauth_token = fixture.Create<string>();
            var oauth_verifier = fixture.Create<string>();
            var twitterUserId = fixture.Create<long>();
            
            var twitterApi = A.Fake<ITwitterAuthenticatedClient>();
            var twitterUserTracker = A.Fake<ITwitterUserTracker>();

            A.CallTo(
                () =>
                    twitterApi.Authenticate(
                        A<string>.That.Matches(x => x == oauth_token),
                        A<string>.That.Matches(x => x == oauth_verifier))).Invokes(
                _ =>
                {
                    A.CallTo(() => twitterApi.UserId).Returns(twitterUserId);
                })
                .Returns(twitterUserId);
                

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<AuthModule>();
                    with.Dependency<ITwitterAuthenticatedClient>(twitterApi);
                    with.Dependency<ITwitterUserTracker>(twitterUserTracker);
                    with.RequestStartup((container, pipelines, __) =>
                    {
                        var formsAuthConfiguration =
                            new FormsAuthenticationConfiguration()
                            {
                                RedirectUrl = "~/Auth/SignIn",
                                UserMapper = twitterUserTracker
                            };

                        FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
                    });
                });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Get("/auth/callback",
                req =>
                {
                    req.Query("oauth_token", oauth_token);
                    req.Query("oauth_verifier", oauth_verifier);
                });

            //assert
            response.ShouldHaveRedirectedTo("/");
            A.CallTo(() => twitterUserTracker.Register(A<Guid>._, twitterApi)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
