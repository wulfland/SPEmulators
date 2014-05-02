namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFieldIndexCollection : CollectionIsolator<SPFieldIndex, SPFieldIndexCollection, ShimSPFieldIndexCollection>
    {
        private SPList list;

        public SPList List
        {
            get
            {
                return this.list;
            }
            set
            {
                this.list = value;
            }
        }

        public static int Capacity
        {
            get
            {
                return 20;
            }
        }

        public SimSPFieldIndexCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPFieldIndexCollection>())
        {
        }

        public SimSPFieldIndexCollection(SPFieldIndexCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.AddSPField = (SPField field) =>
            {
                if (field == null)
                {
                    throw new ArgumentNullException("field");
                }
                if (base.Count >= SPFieldIndexCollection.Capacity)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return this.AddItem(SimSPField.FromInstance(field));
            };
            base.Fake.ItemGetGuid = (Guid id) =>
            {
                foreach (SPFieldIndex current in this)
                {
                    if (current.Id == id)
                    {
                        return current;
                    }
                }
                throw new ArgumentException("The field index could not be found.");
            };
            base.Fake.ItemGetInt32 = (int index) =>
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (index >= base.Count)
                {
                    throw new ArgumentException();
                }
                return base[index];
            };
            base.Fake.DeleteGuid = (Guid id) =>
            {
                for (int i = 0; i < base.Count; i++)
                {
                    SPFieldIndex sPFieldIndex = base[i];
                    if (sPFieldIndex.Id == id)
                    {
                        base.RemoveAt(i);
                        break;
                    }
                }
            };
        }

        public SimSPFieldIndex SetNext()
        {
            var fieldIndex = this.CreateFieldIndex();
            base.Add(fieldIndex.Instance);
            return fieldIndex;
        }

        internal static void Initialize()
        {
            ShimSPFieldIndexCollection.BehaveAsNotImplemented();
            ShimSPFieldIndexCollection.CapacityGet = () => Capacity;
        }

        private Guid AddItem(SimSPField field)
        {
            var spField = this.List.Fields[field.Id];
            var fieldIndex = this.SetNext();
            fieldIndex.Id = field.Id;
            fieldIndex.Parent = base.Instance;
            spField.Indexed = true;
            spField.Update();

            return spField.Id;
        }

        private SimSPFieldIndex CreateFieldIndex()
        {
            return new SimSPFieldIndex();
        }
    }
}
