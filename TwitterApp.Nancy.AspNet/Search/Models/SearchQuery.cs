namespace TwitterApp.Nancy.Search.Models
{
    public class SearchQuery
    {
        public string Query { get; set; }

        public long? MaxId { get; set; }

        public int? Count { get; set; }
    }
}
