namespace TwitterApp.Web.Nancy.Tests.Auth
{
    using System;
    using System.Runtime.Remoting.Contexts;

    using FluentAssertions;

    using global::Nancy;

    using Ploeh.AutoFixture;

    using TwitterApp.Nancy.AspNet.Auth;

    using Xunit;

    public class TwitterUserMapperTests
    {
        [Fact]
        public void When_Registered_can_retrieve_UserIdentity_via_Session_Guid()
        {
            var fixture = new Fixture();
            var sessionGuid = fixture.Create<Guid>();
            var context = new NancyContext();
            var user = fixture.Create<TwitterUser>();

            var sut = new TwitterUserMapper();
            sut.Register(sessionGuid, user);

            var userResult = sut.GetUserFromIdentifier(sessionGuid, context);
            userResult.Should().Be(user);
        }

        [Fact]
        public void When_retrieving_unknown_Session_Guid_returns_null()
        {
            var fixture = new Fixture();
            var sessionGuid = fixture.Create<Guid>();
            var context = new NancyContext();

            var sut = new TwitterUserMapper();
            sut.GetUserFromIdentifier(sessionGuid, context).Should().BeNull();
        }
    }
}