namespace TwitterApp.Nancy.Search
{
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using global::Nancy.Security;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;
    using TwitterApp.Nancy.Search.Models;

    public class SearchModule : NancyModule
    {
        public SearchModule(ITwitterPublicClient twitterSearch)
        {
            this.Get["/"] = _ => this.Response.AsRedirect("/search");

            this.Get["/search"] = _ => this.View["Search", new SearchViewModel()];

            this.Post["/search", true] = async (_, __) =>
                {
                    var query = this.Bind<SearchQuery>();
                    TweetSearchResult result = await twitterSearch.SearchTweets(query.Query, query.Count ?? 15, query.MaxId);

                    return this.Negotiate
                                    .WithModel(new SearchViewModel(query, result))
                                    .WithView("Search");
                };
        }
    }
}
