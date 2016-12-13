namespace TwitterApp.Nancy.AspNet.Auth
{
    using System;
    using System.Collections.Concurrent;

    using global::Nancy;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.Security;

    public class TwitterUserMapper : IUserMapper
    {
        private readonly ConcurrentDictionary<Guid, TwitterUser> map = new ConcurrentDictionary<Guid, TwitterUser>();

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            TwitterUser user;
            return this.map.TryGetValue(identifier, out user) ? user : null;
        }

        public void Register(Guid sessionGuid, TwitterUser user)
        {
            this.map.TryAdd(sessionGuid, user);
        }
    }
}