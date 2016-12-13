namespace TwitterApp.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class Tweet
    {
        public Tweet(long id, string content, long authorUserId, string authorScreenName, DateTime createdDate)
        {
            this.Id = id;
            this.Content = content;
            this.AuthorUserId = authorUserId;
            this.AuthorScreenName = authorScreenName;
            this.CreatedDate = createdDate;
        }

        public long Id { get; }

        public string Content { get; }

        public long AuthorUserId { get; }

        public string AuthorScreenName { get; }

        public DateTime CreatedDate { get; }

        protected bool Equals(Tweet other)
        {
            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Tweet)obj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
