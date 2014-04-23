namespace SPEmulators
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;

    internal interface ICanIsolate<TInst, out TFake> : IInstanced<TInst>, IInstanced
    {
        TFake Fake { get; }
    }
}
