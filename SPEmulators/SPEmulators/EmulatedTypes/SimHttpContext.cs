namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;

    internal class SimHttpContext : Isolator<HttpContext, ShimHttpContext>
    {
        private static SimHttpContext current;
        private readonly Dictionary<object, object> itSims = new Dictionary<object, object>();
        private HttpRequest request;
        private HttpResponse response;

        public static SimHttpContext Current
        {
            get
            {
                if (SimHttpContext.current == null)
                {
                    SimHttpContext.SetCurrent();
                }

                return SimHttpContext.current;
            }
        }

        public HttpRequest Request
        {
            get
            {
                if (this.request == null)
                {
                    this.request = new SimHttpRequest().Instance;
                }
                return this.request;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.request = value;
            }
        }

        public HttpResponse Response
        {
            get
            {
                if (this.response == null)
                {
                    this.response = new SimHttpResponse().Instance;
                }
                return this.response;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.response = value;
            }
        }

        public DateTime Timestamp
        {
            get;
            set;
        }

        public IDictionary<object, object> ItSims
        {
            get
            {
                return this.itSims;
            }
        }

        public SimHttpContext() : this(ShimRuntime.CreateUninitializedInstance<HttpContext>())
        {
        }

        public SimHttpContext(HttpContext instance) : base(instance)
        {
            base.Fake.RequestGet = () => this.Request;
            base.Fake.ResponseGet = () => this.Response;
            base.Fake.TimestampGet = () => this.Timestamp;
            base.Fake.ItemsGet = () => this.itSims;
        }

        public static void Initialize()
        {
            ShimHttpContext.BehaveAsNotImplemented();
            SimHttpContext.SetCurrent();
        }

        private static SimHttpContext SetCurrent()
        {
            if (SimHttpContext.current == null)
            {
                SimHttpContext.current = new SimHttpContext();
            }
            ShimHttpContext.CurrentGet = () => SimHttpContext.Current.Instance;
            ShimRuntime.RegisterStateCleaner(delegate
            {
                SimHttpContext.current = null;
            });

            return SimHttpContext.current;
        }

        public static void ResetCurrent()
        {
            if (SimHttpContext.current != null)
            {
                SimHttpContext.current = null;
                ShimHttpContext.CurrentGet = () => null;
            }
        }

        public static SimHttpContext FromInstance(HttpContext instance)
        {
            return InstancedPool.CastAsInstanced<HttpContext, SimHttpContext>(instance);
        }
    }
}
