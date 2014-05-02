namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;

    internal class SimHttpRequest : Isolator<HttpRequest, ShimHttpRequest>
    {
        public Uri Url
        {
            get;
            set;
        }

        public string UserHostAddress
        {
            get;
            set;
        }

        public string UserAgent
        {
            get;
            set;
        }

        public NameValueCollection QueryString
        {
            get;
            set;
        }

        public SimHttpRequest()
            : this(ShimRuntime.CreateUninitializedInstance<HttpRequest>())
        {
        }

        public SimHttpRequest(HttpRequest instance)
            : base(instance)
        {
            this.QueryString = new NameValueCollection();
            base.Fake.UrlGet = () => this.Url;
            base.Fake.UserHostAddressGet = () => this.UserHostAddress;
            base.Fake.UserAgentGet = () => this.UserAgent;
            base.Fake.IsAuthenticatedGet = () => HttpContext.Current.User.Identity.IsAuthenticated;
            base.Fake.QueryStringGet = () => this.QueryString;
        }

        public static SimHttpRequest FromInstance(HttpRequest instance)
        {
            return InstancedPool.CastAsInstanced<HttpRequest, SimHttpRequest>(instance);
        }

        internal static void Initialize()
        {
            ShimHttpRequest.BehaveAsNotImplemented();
            ShimHttpRequest.StaticConstructor = delegate
            {
            };
        }
    }
}
