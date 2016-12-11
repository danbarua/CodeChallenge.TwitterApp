using System;
using System.Linq;
using System.Web;

namespace TwitterApp.Nancy.AspNet
{
    using System.Configuration;

    using global::Nancy.Conventions;
    using global::Nancy.Hosting.Aspnet;
    using global::Nancy.TinyIoc;

    using TwitterApp.Core.Ports;
    using TwitterApp.Core.TweetSharpAdapter;

    public class Bootstrapper : DefaultNancyAspNetBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            //plug in twitter api

            var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            var accessToken = ConfigurationManager.AppSettings["AccessToken"];
            var accessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];

            var twitterApi = 
                TweetSharpAdapter.Create(consumerKey, consumerSecret, accessToken, accessTokenSecret)
                    .GetAwaiter()
                    .GetResult();

            container.Register<ITwitterApiFacade>(twitterApi);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            // set up feature folder layout
            nancyConventions.ViewLocationConventions.Insert(
                0,
                (viewName, model, context) => string.Concat(context.ModuleName, "/Views/", viewName));
            base.ConfigureConventions(nancyConventions);
        }
    }
}