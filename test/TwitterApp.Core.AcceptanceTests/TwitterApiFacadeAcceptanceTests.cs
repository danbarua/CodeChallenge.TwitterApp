namespace TwitterApp.Core.TweetSharpAdapter
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using TwitterApp.Core.Ports;

    using Xunit;

    public abstract class TwitterApiFacadeAcceptanceTests
    {
        protected abstract Task<ITwitterApiFacade> GetFixture();

        [Fact]
        public async Task Can_Search_Tweets()
        {
            var fixture = await this.GetFixture().ConfigureAwait(false);
            var result = await fixture.SearchTweets("#DreamJob").ConfigureAwait(false);

            result.Tweets.Should().NotBeEmpty();
            result.MinId.Should().NotBe(default(long), "Expect MaxId to be populated");
            result.MinId.Should().Be(result.Tweets.Min(t => t.Id));

            foreach (var tweet in result.Tweets)
            {
                tweet.Id.Should().NotBe(default(long), "Expect Id to be populated");
                tweet.Author.Should().NotBeNullOrEmpty("expect Author to be populated.");
                tweet.CreatedDate.Should().NotBe(default(DateTime), "Expect CreatedDate to be populated");
                tweet.Content.Should().NotBeNullOrEmpty("Expect Content to be populated");

                Console.WriteLine(tweet);
            }
        }

        [Fact]
        public async Task When_Searching_Tweets_If_Count_Not_Specified_Defaults_To_15()
        {
            var fixture = await this.GetFixture().ConfigureAwait(false);
            var result = await fixture.SearchTweets("#DreamJob", 15).ConfigureAwait(false);
            result.Tweets.Length.Should().Be(15);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        public async Task Can_Limit_Search_Results_Using_Count(int count)
        {
            var fixture = await this.GetFixture().ConfigureAwait(false);
            var result = await fixture.SearchTweets("#DreamJob", count).ConfigureAwait(false);
            result.Tweets.Length.Should().Be(count);
        }

        [Fact]
        public async Task Can_Page_Through_Tweets_Using_MaxId()
        {
            var fixture = await this.GetFixture().ConfigureAwait(false);
            var firstPage = await fixture.SearchTweets("#DreamJob", 5).ConfigureAwait(false);

            //todo: need to add -1 to minId to avoid overlap - push this logic into the api?
            var secondPage = await fixture.SearchTweets("#DreamJob", 5, firstPage.MinId - 1).ConfigureAwait(false);

            secondPage.MinId.Should().BeLessThan(firstPage.MinId);

            foreach (var tweetId in secondPage.Tweets.Select(x => x.Id))
            {
                firstPage.Tweets.Select(x => x.Id).Should().NotContain(tweetId);
            }
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
