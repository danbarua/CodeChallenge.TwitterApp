namespace TwitterApp.Nancy.AspNet.Auth
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;

    using global::Nancy;
    using global::Nancy.Security;

    using TwitterApp.Core.Ports;

    public class TwitterUserTracker : ITwitterUserTracker
    {
        private readonly ConcurrentDictionary<Guid, ITwitterAuthenticatedClient> map =
            new ConcurrentDictionary<Guid, ITwitterAuthenticatedClient>();

        public void Register(Guid id, ITwitterAuthenticatedClient twitterApi)
        {
            if (id == default(Guid))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id must be initialized.");
            }

            if (twitterApi == null)
            {
                throw new ArgumentNullException(nameof(twitterApi));
            }

            if (!twitterApi.UserId.HasValue)
            {
                throw new InvalidOperationException("Cannot register an anauthorized Twitter Client");
            }

            this.map.TryAdd(id, twitterApi);
        }

        public ITwitterAuthenticatedClient GetAuthenticatedTwitterClientForUser(long userId)
        {
            return this.map.Values.SingleOrDefault(x => x.UserId == userId);
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            ITwitterAuthenticatedClient value;

            if (this.map.TryGetValue(identifier, out value))
            {
                return new TwitterUser(value.UserId.Value);
            }

            return null;
        }
    }
}