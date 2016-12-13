using System;
using System.Linq;
using System.Web;

namespace TwitterApp.Nancy.AspNet
{
    using System.Configuration;

    using global::Nancy;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.Bootstrapper;
    using global::Nancy.Conventions;
    using global::Nancy.Hosting.Aspnet;
    using global::Nancy.TinyIoc;

    using TwitterApp.Core.Ports;
    using TwitterApp.Core.TweetSharpAdapter;
    using TwitterApp.Nancy.AspNet.Auth;

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
                TweetSharpPublicClientAdapter.Create(consumerKey, consumerSecret, accessToken, accessTokenSecret)
                    .GetAwaiter()
                    .GetResult();

            // public twitter client singleton
            container.Register<ITwitterPublicClient>(twitterApi);

            // factory method for authed twitter client
            // instantiated on sign in then cached in-mem
            // for each user
            container.Register<ITwitterAuthenticatedClient>((_, __) => new TweetSharpAuthenticatedClientAdapter(consumerKey, consumerSecret));

            // user tracker singleton
            // maps session guids to IUserIdentity
            // retrieves authenticated twitter client for a given user
            var twitterUserTracker = new TwitterUserTracker();
            container.Register<ITwitterUserTracker>(twitterUserTracker);
            container.Register<IUserMapper>(twitterUserTracker);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            // set up feature folder layout
            nancyConventions.ViewLocationConventions.Insert(
                0,
                (viewName, model, context) => string.Concat(context.ModuleName, "/Views/", viewName));
            base.ConfigureConventions(nancyConventions);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/Auth/SignIn",
                    UserMapper = container.Resolve<IUserMapper>()
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }
}