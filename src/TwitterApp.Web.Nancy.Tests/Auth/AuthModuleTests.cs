using System;

namespace TwitterApp.Web.Nancy.Tests.Auth
{

    using FakeItEasy;

    using FluentAssertions;

    using global::Nancy;
    using global::Nancy.Testing;

    using TwitterApp.Core.Ports;
    using TwitterApp.Nancy.AspNet.Auth;

    using Xunit;

    public class AuthModuleTests
    {
        [Fact]
        public void Get_Auth_Sign_In_Populates_Link()
        {
            //arrange
            var twitterApi = A.Fake<ITwitterAuthenticatedClient>();
            var callBackUrl = new Uri("http://api.faketwitter.com/?foo=bar");
            A.CallTo(() => twitterApi.GetAuthorizationUri(A<string>._)).Returns(callBackUrl);

            var bootstrapper = new TestBootstrapper(
                with =>
                    {
                        with.Module<AuthModule>();
                        with.Dependency<ITwitterAuthenticatedClient>(twitterApi);
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
    }
}
