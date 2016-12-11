using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterApp.Nancy.Search.Models
{
    public class SearchQuery
    {
        public string Query { get; set; }

        public long? MaxId { get; set; }

        public int? Count { get; set; }
    }
}
