namespace TwitterApp.Core.AcceptanceTests
{
    using System;

    using Nancy;

    public class HorribleOAuthHacks : NancyModule
    {
        public const string OAuthCallBack = "auth/oauth_callback";

        public static string OAuthToken;

        public static string OAuthVerifier;

        public static bool IsOAuthSigninCompleted => !string.IsNullOrEmpty(OAuthToken)
                                                     && !string.IsNullOrEmpty(OAuthVerifier);

        public HorribleOAuthHacks()
            : base("/" + OAuthCallBack)
        {
            this.Get["/"] = _ =>
                {
                    OAuthToken = Request.Query.oauth_token;
                    OAuthVerifier = Request.Query.oauth_verifier;

                    return "Authenticated OK";
                };
        }

        public static void Reset()
        {
            OAuthToken = null;
            OAuthVerifier = null;
        }
    }
}
