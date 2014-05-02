namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPViewCollection : CollectionIsolator<SPView, SPViewCollection, ShimSPViewCollection>
    {
        public SPList List
        {
            get;
            set;
        }

        public SPView DefaultView
        {
            get
            {
                int defaultViewIndex = this.DefaultViewIndex;
                SPView result;
                if (defaultViewIndex < 0 || defaultViewIndex >= base.Count)
                {
                    result = null;
                }
                else
                {
                    result = base[defaultViewIndex];
                }

                return result;
            }
        }

        public int DefaultViewIndex
        {
            get;
            set;
        }
        public SimSPViewCollection()
            : this(null)
        {
        }

        public SimSPViewCollection(SPViewCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.DefaultViewGet = (() => this.DefaultView);
            base.Fake.ItemGetInt32 = ((int index) => base[index]);
            base.Fake.ItemAtIndexInt32 = ((int index) => base[index]);
            base.Fake.ItemGetGuid = (delegate(Guid id)
            {
                foreach (SPView current in this)
                {
                    if (current.ID == id)
                    {
                        return current;
                    }
                }
                throw new ArgumentOutOfRangeException();
            });
            base.Fake.ItemGetString = (delegate(string url)
            {
                foreach (SPView current in this)
                {
                    if (current.Title == url)
                    {
                        return current;
                    }
                }
                throw new ArgumentOutOfRangeException();
            });
        }

        public SimSPView SetOne()
        {
            var view = new SimSPView
            {
                ParentList = this.List
            };
            base.Clear();
            base.Add(view.Instance);

            return view;
        }
    }
}
