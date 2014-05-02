namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFieldIndex : Isolator<SPFieldIndex, ShimSPFieldIndex>
    {
        private readonly List<Guid> ids;
        private SPFieldIndexCollection parent;

        public SPFieldIndexCollection Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        public Guid Id
        {
            get
            {
                return this.ids[0];
            }
            set
            {
                this.ids.Clear();
                this.ids.Add(value);
            }
        }

        public IList<Guid> Ids
        {
            get
            {
                return this.ids;
            }
        }

        public SimSPFieldIndex()
            : this(ShimRuntime.CreateUninitializedInstance<SPFieldIndex>())
        {
        }

        public SimSPFieldIndex(SPFieldIndex instance)
            : base(instance)
        {
            this.ids = new List<Guid>();
            base.Fake.IdGet = () => this.Id;
            base.Fake.Delete = () =>
            {
                this.Parent.Delete(this.Id);
            };
            base.Fake.FieldCountGet = () => this.Ids.Count;
            base.Fake.GetFieldInt32 = (int index) => this.Ids[index];
        }

        public static SimSPFieldIndex FromInstance(SPFieldIndex instance)
        {
            return InstancedPool.CastAsInstanced<SPFieldIndex, SimSPFieldIndex>(instance);
        }

        internal static void Initialize()
        {
            ShimSPFieldIndex.BehaveAsNotImplemented();
        }
    }
}
