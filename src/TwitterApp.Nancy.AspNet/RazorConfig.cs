namespace TwitterApp.Nancy.AspNet
{
    using System.Collections.Generic;

    using global::Nancy.ViewEngines.Razor;

    using TwitterApp.Core.Models;

    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return typeof(TweetSearchResult).Assembly.FullName;
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return typeof(TweetSearchResult).Namespace;
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}