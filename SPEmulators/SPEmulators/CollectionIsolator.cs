namespace SPEmulators
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;

    internal abstract class CollectionIsolator<TItem, TCollection, TCollectionFake> :
        Collection<TItem>, ICanIsolate<TCollection, TCollectionFake>, IInstanced<TCollection>, IInstanced
        where TCollection : class
        where TCollectionFake : IInstanced<TCollection>
    {
        readonly TCollectionFake fake;

        public TCollection Instance
        {
            [DebuggerStepThrough]
            get
            {
                TCollection result;
                using (ShimRuntime.AcquireProtectingThreadContext())
                {
                    TCollectionFake myFake = this.fake;
                    result = ((IInstanced<TCollection>)myFake).Instance;
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

        public TCollectionFake Fake
        {
            [DebuggerStepThrough]
            get
            {
                TCollectionFake result;
                using (ShimRuntime.AcquireProtectingThreadContext())
                {
                    result = this.fake;
                }

                return result;
            }
        }

        protected CollectionIsolator(TCollection instance = null)
        {
            using (ShimRuntime.AcquireProtectingThreadContext())
            {
                if (instance == null)
                {
                    fake = (TCollectionFake)((object)Activator.CreateInstance(typeof(TCollectionFake)));
                }
                else
                {
                    fake = (TCollectionFake)((object)Activator.CreateInstance(typeof(TCollectionFake), new object[] { instance }));
                }

                InstancedPool.RegisterInstanced(this);
            }
        }

        protected virtual TItem CreateItem()
        {
            throw new NotImplementedException();
        }
    }
}
