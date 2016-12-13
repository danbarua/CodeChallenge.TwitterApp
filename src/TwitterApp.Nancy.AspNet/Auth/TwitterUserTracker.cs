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
        private readonly ConcurrentDictionary<Guid, long> sessionUserIdMap = new ConcurrentDictionary<Guid, long>();

        private readonly ConcurrentDictionary<long, ITwitterAuthenticatedClient> userClientMap = new ConcurrentDictionary<long, ITwitterAuthenticatedClient>();  

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

            this.sessionUserIdMap.TryAdd(id, twitterApi.UserId.Value);
            this.userClientMap.TryAdd(twitterApi.UserId.Value, twitterApi);
        }

        public ITwitterAuthenticatedClient GetAuthenticatedTwitterClientForUser(long userId)
        {
            ITwitterAuthenticatedClient value;

            if (this.userClientMap.TryGetValue(userId, out value))
            {
                return value;
            }

            return null;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            long value;

            if (this.sessionUserIdMap.TryGetValue(identifier, out value))
            {
                return new TwitterUser(value);
            }

            return null;
        }
    }
}