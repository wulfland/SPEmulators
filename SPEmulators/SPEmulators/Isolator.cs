namespace SPEmulators
{
    using System;
    using System.Diagnostics;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;

    internal abstract class Isolator<TInst, TFake> : ICanIsolate<TInst, TFake>, IInstanced<TInst>, IInstanced 
        where TInst : class 
        where TFake : IInstanced<TInst>
    {
        readonly TFake fake;

        public TFake Fake
        {
            [DebuggerStepThrough]
            get
            {
                TFake result;
                using (ShimRuntime.AcquireProtectingThreadContext())
                {
                    result = this.fake;
                }

                return result;
            }
        }

        public TInst Instance
        {
            [DebuggerStepThrough]
            get
            {
                TInst result;
                using (ShimRuntime.AcquireProtectingThreadContext())
                {
                    TFake myFake = this.fake;
                    result = ((IInstanced<TInst>)myFake).Instance;
                }

                return result;
            }
        }

        object IInstanced.Instance 
        {
            get
            {
                return this.Instance;
            }
        }

        protected Isolator(TInst instance = null)
        {
            using (ShimRuntime.AcquireProtectingThreadContext())
            {
                if (instance == null)
                {
                    fake = (TFake)((object)Activator.CreateInstance(typeof(TFake)));
                }
                else
                {
                    fake = (TFake)((object)Activator.CreateInstance(typeof(TFake), new object[] { instance }));
                }

                InstancedPool.RegisterInstanced(this);
            }
        }
    }
}
