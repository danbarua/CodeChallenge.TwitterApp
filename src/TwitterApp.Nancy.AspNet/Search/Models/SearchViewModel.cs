namespace TwitterApp.Nancy.Search.Models
{
    using TwitterApp.Core.Models;

    public class SearchViewModel
    {
        public SearchViewModel()
        {
            this.Query = new SearchQuery();
        }

        public SearchViewModel(SearchQuery query, TweetSearchResult result)
        {
            this.Query = query;
            this.Result = result;
        }
        
        public SearchQuery Query { get; set; }

        public TweetSearchResult Result { get; set; }
    }
}
