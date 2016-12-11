using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterApp.Core.TweetSharpAdapter.AcceptanceTests
{
    using TwitterApp.Core.AcceptanceTests;
    using TwitterApp.Core.Ports;

    public class TweetSharpAdapterAcceptanceTests : TwitterApiFacadeAcceptanceTests
    {
        protected override ITwitterApiFacade GetFixture()
        {
            return new TweetSharpAdapter();
        }
    }
}
