namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPListItemCollection : CollectionIsolator<SPListItem, SPListItemCollection, ShimSPListItemCollection>
    {
        public SPList List
        {
            get;
            set;
        }
        public SPListItem this[Guid uniqueId]
        {
            get
            {
                foreach (SPListItem current in this)
                {
                    if (current.UniqueId == uniqueId)
                    {
                        return current;
                    }
                }

                throw new ArgumentException();
            }
        }
        public SimSPListItemCollection() : this(ShimRuntime.CreateUninitializedInstance<SPListItemCollection>())
        {
        }

        public SimSPListItemCollection(SPListItemCollection instance) : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.DeleteInt32 = new FakesDelegates.Action<int>(this.Delete);
            base.Fake.DeleteItemByIdInt32 = new FakesDelegates.Action<int>(this.DeleteItemById);
            base.Fake.FieldsGet = () => this.List.Fields;
            base.Fake.ItemGetGuid = (Guid uniqueId) => this[uniqueId];
            base.Fake.ItemGetInt32 = (int index) => base[index];
            base.Fake.ItemAtIndexInt32 = (int index) => base[index];
            base.Fake.GetItemIndexByIdInt32 = new FakesDelegates.Func<int, int>(this.GetItemIndexById);
            base.Fake.GetItemByIdInt32 = new FakesDelegates.Func<int, SPListItem>(this.GetItemById);
            base.Fake.ListGet = () => this.List;
            base.Fake.Add = () => this.Add().Instance;
        }

        public SimSPListItem Add()
        {
            return this.CreateListItem();
        }

        public void Delete(int index)
        {
            base[index].Delete();
        }

        public void DeleteItemById(int id)
        {
            this.GetItemById(id).Delete();
        }

        public SPListItem GetItemById(int id)
        {
            foreach (SPListItem current in this)
            {
                if (current.ID == id)
                {
                    return current;
                }
            }

            throw new ArgumentException();
        }

        internal int GetItemIndexById(int id)
        {
            int result;
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].ID == id)
                {
                    result = i;
                    return result;
                }
            }

            result = -1;
            return result;
        }

        public SimSPListItem SetOne()
        {
            var simSPListItem = this.CreateListItem();
            base.Clear();
            base.Add(simSPListItem.Instance);

            return simSPListItem;
        }

        public SimSPListItem SetNext()
        {
            var simSPListItem = this.CreateListItem();
            base.Add(simSPListItem.Instance);
            return simSPListItem;
        }

        protected override SPListItem CreateItem()
        {
            return this.CreateListItem().Instance;
        }

        internal static void Initialize()
        {
            ShimSPListItemCollection.BehaveAsNotImplemented();
        }

        private SimSPListItem CreateListItem()
        {
            return new SimSPListItem
            {
                ListItems = this
            };
        }
    }
}
