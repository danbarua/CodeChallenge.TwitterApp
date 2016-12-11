namespace TwitterApp.Nancy.Search
{
    using global::Nancy;
    using global::Nancy.ModelBinding;

    using TwitterApp.Core.Models;
    using TwitterApp.Core.Ports;
    using TwitterApp.Nancy.Search.Models;

    public class SearchModule : NancyModule
    {
        public SearchModule(ITwitterApiFacade twitterApi)
        {
            this.Get["/search"] = _ => this.View["Search.cshtml", new SearchViewModel()];

            this.Post["/search/", true] = async (_, __) =>
                {
                    var query = this.BindAndValidate<SearchQuery>();
                    TweetSearchResult result = await twitterApi.SearchTweets(query.Query, query.Count ?? 15, query.MaxId);

                    return this.Negotiate
                                    .WithModel(new SearchViewModel(query, result))
                                    .WithView("Search.cshtml");
                };
        }
    }
}
