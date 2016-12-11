using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterApp.Core.TweetSharpAdapter
{
    using TwitterApp.Core.Ports;

    using Xunit;

    public abstract class TwitterApiFacadeAcceptanceTests
    {
        protected abstract ITwitterApiFacade GetFixture();

        [Fact]
        public Task Can_Search_Tweets()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public Task Can_Post_A_Tweet()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public Task Can_Delete_A_Tweet()
        {
            throw new NotImplementedException();
        }
    }
}
