namespace TwitterApp.Nancy.AspNet.Auth
{
    using System;

    using global::Nancy;
    using global::Nancy.Authentication.Forms;

    using TwitterApp.Core.Ports;

    public class AuthModule : NancyModule
    {
        public AuthModule(ITwitterAuthenticatedClient twitterApi, ITwitterUserTracker userTracker)
            : base("/auth")
        {
            this.Get["/signin", true] = async (_, __) =>
                {
                    var callbackurl = new UriBuilder(
                        Request.Url.Scheme,
                        Request.Url.HostName,
                        Request.Url.Port ?? 80,
                        "/auth/callback");

                    ViewBag.AuthUrl = await twitterApi.GetAuthorizationUri(callbackurl.ToString());
                    return View["SignIn"];
                };

            this.Get["/callback", true] = async (_, __) =>
                {
                    await twitterApi.Authenticate((string)Request.Query.oauth_token, (string)Request.Query.oauth_verifier);

                    Guid sessionId = Guid.NewGuid();
                    userTracker.Register(sessionId, twitterApi);

                    return this.LoginAndRedirect(sessionId);
                };
        }
    }
}