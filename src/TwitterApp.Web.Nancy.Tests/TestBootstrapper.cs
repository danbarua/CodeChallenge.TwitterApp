namespace TwitterApp.Web.Nancy.Tests
{
    using System;

    using global::Nancy.Conventions;
    using global::Nancy.Testing;

    public class TestBootstrapper : ConfigurableBootstrapper
    {
        public TestBootstrapper(Action<ConfigurableBootstrapperConfigurator> with)
            : base(innerConfig =>
                {
                    ApplyConfiguration(innerConfig);
                    with(innerConfig);
                })
        {
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.ViewLocationConventions.Insert(0, (viewName, model, context) => string.Concat(context.ModuleName, "/Views/", viewName));
            base.ConfigureConventions(nancyConventions);
        }

        private static void ApplyConfiguration(ConfigurableBootstrapperConfigurator config)
        {
            config.ViewFactory<TestingViewFactory>();
        }
    }
}