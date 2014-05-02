namespace SPEmulators.EmulatedTypes
{
    using System.Web;
    using System.Web.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;


    internal class SimHttpResponse : Isolator<HttpResponse, ShimHttpResponse>
    {
        public SimHttpResponse() : this(ShimRuntime.CreateUninitializedInstance<HttpResponse>())
        {
        }

        public SimHttpResponse(HttpResponse instance) : base(instance)
        {
        }

        public static SimHttpResponse FromInstance(HttpResponse instance)
        {
            return InstancedPool.CastAsInstanced<HttpResponse, SimHttpResponse>(instance);
        }
        internal static void Initialize()
        {
            ShimHttpResponse.BehaveAsNotImplemented();
            ShimHttpResponse.StaticConstructor = delegate
            {
            };
        }
    }
}
