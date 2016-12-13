namespace TwitterApp.Nancy.AspNet.Auth
{
    using System;

    using global::Nancy.Authentication.Forms;

    using TwitterApp.Core.Ports;

    public interface ITwitterUserTracker : IUserMapper
    {
        void Register(Guid id, ITwitterAuthenticatedClient twitterApi);

        ITwitterAuthenticatedClient GetAuthenticatedTwitterClientForUser(long userId);
    }
}