namespace TwitterApp.Web.Nancy.Tests
{
    using System;

    using FakeItEasy;

    using FluentAssertions;

    using global::Nancy;

    using TwitterApp.Core.Ports;
    using TwitterApp.Nancy.AspNet.Auth;

    using Xunit;

    public class TwitterUserTrackerTests
    {
        [Fact]
        public void Cannot_Register_with_invalid_Guid()
        {
            var client = A.Fake<ITwitterAuthenticatedClient>();
            var sut = new TwitterUserTracker();
            var ex = Record.Exception(() => sut.Register(new Guid(), client));
            ex.Should().BeOfType<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Cannot_Register_with_null_client()
        {
            var sut = new TwitterUserTracker();
            var ex = Record.Exception(() => sut.Register(Guid.NewGuid(), null));
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void When_registered_Can_retrieve_UserIdentity_from_Session_Id()
        {
            const long TwitterUserId = 123L;
            var sessionid = Guid.NewGuid();

            var client = A.Fake<ITwitterAuthenticatedClient>();
            A.CallTo(() => client.UserId).Returns(TwitterUserId);

            var context = new NancyContext();
            var sut = new TwitterUserTracker();

            sut.Register(sessionid, client);

            var user = sut.GetUserFromIdentifier(sessionid, context);
            user.Should().NotBeNull();
            user.Should().BeOfType<TwitterUser>();
            user.As<TwitterUser>().TwitterUserId.Should().Be(TwitterUserId);
        }

        [Fact]
        public void Returns_null_UserIdentity_When_Not_registered()
        {
            var context = new NancyContext();
            var sut = new TwitterUserTracker();

            var result = sut.GetUserFromIdentifier(Guid.NewGuid(), context);
            result.Should().BeNull();
        }


        [Fact]
        public void When_registered_Can_retrieve_TwitterClient_from_UserId()
        {
            const long TwitterUserId = 123L;
            var sessionid = Guid.NewGuid();

            var client = A.Fake<ITwitterAuthenticatedClient>();
            A.CallTo(() => client.UserId).Returns(TwitterUserId);
            var sut = new TwitterUserTracker();

            sut.Register(sessionid, client);

            var result = sut.GetAuthenticatedTwitterClientForUser(TwitterUserId);
            result.Should().NotBeNull();
            result.UserId.Should().Be(TwitterUserId);
        }
    }
}
