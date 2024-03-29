﻿namespace TwitterApp.Core.Ports
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TwitterException : Exception
    {
        public TwitterException()
        {
        }

        public TwitterException(string message)
            : base(message)
        {
        }

        public TwitterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TwitterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
