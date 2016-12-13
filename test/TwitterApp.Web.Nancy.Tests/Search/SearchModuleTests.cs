namespace TwitterApp.Web.Nancy.Tests
{
    using FakeItEasy;

    using FluentAssertions;

    using global::Nancy;
    using global::Nancy.Testing;

    using Ploeh.AutoFixture;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;
    using TwitterApp.Nancy.Search;
    using TwitterApp.Nancy.Search.Models;

    using Xunit;

    public class SearchModuleTests
    {
        [Fact]
        public void Get_index_should_redirect_to_search()
        {
            //arrange
            var twitterApi = A.Fake<ITwitterPublicClient>();

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<SearchModule>();
                    with.Dependency(twitterApi);
                });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Get("/");

            //assert
            response.ShouldHaveRedirectedTo("/search");
        }

        [Fact]
        public void Get_search_should_return_empty_view()
        {
            //arrange
            var twitterApi = A.Fake<ITwitterPublicClient>();

            var bootstrapper = new TestBootstrapper(
                with =>
                    {
                        with.Module<SearchModule>();
                        with.Dependency(twitterApi);
                    });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Get("/search");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var viewModel = response.GetModel<SearchViewModel>();

            viewModel.Query.Should().NotBeNull();
            viewModel.Query.ShouldBeEquivalentTo(new SearchQuery());

            viewModel.Result.Should().BeNull();

            response.GetViewName().Should().Be("Search");
        }

        [Fact]
        public void Post_search_should_return_view_populated_with_query_and_result()
        {
            //arrange
            var fixture = new Fixture();
            var query = fixture.Create<SearchQuery>();
            var result = fixture.Create<TweetSearchResult>();

            var twitterApi = A.Fake<ITwitterPublicClient>();
            A.CallTo(
                () =>
                twitterApi.SearchTweets(
                    A<string>.That.Matches(x => x == query.Query),
                    A<int>.That.Matches(x => x == query.Count.Value),
                    A<long?>.That.Matches(x => x == query.MaxId)))
                .Returns(result);

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<SearchModule>();
                    with.Dependency(twitterApi);
                });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Post(
                "/search",
                req =>
                    {
                        req.FormValue("Query", query.Query);
                        req.FormValue("MaxId", query.MaxId?.ToString() ?? string.Empty);
                        req.FormValue("Count", query.Count?.ToString() ?? string.Empty);
                    });

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var viewModel = response.GetModel<SearchViewModel>();

            viewModel.Query.Should().NotBeNull();
            viewModel.Result.Should().NotBeNull();

            viewModel.Query.ShouldBeEquivalentTo(query);
            viewModel.Result.ShouldBeEquivalentTo(result);

            response.GetViewName().Should().Be("Search");
        }

        [Fact]
        public void If_count_is_not_specified_it_should_default_to_15()
        {
            //arrange
            const int DefaultPageSize = 15;
            var fixture = new Fixture();
            var query = fixture.Create<SearchQuery>();
            var result = fixture.Create<TweetSearchResult>();

            var twitterApi = A.Fake<ITwitterPublicClient>();
            A.CallTo(
                () =>
                twitterApi.SearchTweets(
                    A<string>.That.Matches(x => x == query.Query),
                    A<int>.That.Matches(x => x == DefaultPageSize),
                    A<long?>.That.Matches(x => x == query.MaxId)))
                .Returns(result);

            var bootstrapper = new TestBootstrapper(
                with =>
                {
                    with.Module<SearchModule>();
                    with.Dependency(twitterApi);
                });

            var browser = new Browser(bootstrapper);

            //act
            var response = browser.Post(
                "/search",
                req =>
                {
                    req.FormValue("Query", query.Query);
                    req.FormValue("MaxId", query.MaxId?.ToString() ?? string.Empty);
                });

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            A.CallTo(
                () =>
                twitterApi.SearchTweets(
                    A<string>.That.Matches(x => x == query.Query),
                    A<int>.That.Matches(x => x == DefaultPageSize), //check that search was called with default page size
                    A<long?>.That.Matches(x => x == query.MaxId)))
            .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
