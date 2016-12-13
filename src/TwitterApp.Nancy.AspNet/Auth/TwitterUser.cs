namespace TwitterApp.Nancy.AspNet.Auth
{
    using System.Collections.Generic;

    using global::Nancy.Security;

    public class TwitterUser : IUserIdentity
    {
        private readonly List<string> claims = new List<string>();

        public TwitterUser(long twitterUserId)
        {
            this.UserName = twitterUserId.ToString();
        }

        public long TwitterUserId => long.Parse(this.UserName);

        public string UserName { get; }

        public IEnumerable<string> Claims => this.claims;
    }
}