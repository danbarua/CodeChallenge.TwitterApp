namespace TwitterApp.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class Tweet
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            return $"Id: {this.Id}, Content: {this.Content}, Author: {this.Author}, CreatedDate: {this.CreatedDate}";
        }

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
