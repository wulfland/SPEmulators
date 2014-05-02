namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration.Fakes;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPWebCollection : CollectionIsolator<SPWeb, SPWebCollection, ShimSPWebCollection>
    {
        public SimSPWeb this[Guid id]
        {
            get
            {
                var web = this.FirstOrDefault((SPWeb w) => w.ID == id);
                if (web == null)
                {
                    throw new ArgumentException();
                }

                return SimSPWeb.FromInstance(web);
            }
        }

        public SimSPWeb this[string url]
        {
            get
            {
                var simWeb = this.ParentWeb;
                string[] array = url.Split(new char[] { '/' });
                for (int i = 0; i < array.Length; i++)
                {
                    var webName = array[i];
                    var web = simWeb.Webs.FirstOrDefault((SPWeb w) => w.Name == webName);
                    if (web == null)
                    {
                        var simWeb2 = this.Add(webName);
                        simWeb2.Exists = false;
                        web = simWeb2.Instance;
                    }

                    simWeb = SimSPWeb.FromInstance(web);
                }

                return simWeb;
            }
        }

        public string[] Names
        {
            get
            {
                return (
                    from w in this
                    select w.Name).ToArray<string>();
            }
        }

        internal SimSPWeb ParentWeb
        {
            get;
            private set;
        }

        internal SimSPWebCollection(SimSPWeb parentWeb = null)
            : base(null)
        {
            this.ParentWeb = parentWeb;
            base.Fake.Bind((IEnumerable)this);
            base.Fake.CountGet = (() => base.Count);
            base.Fake.ItemGetInt32 = ((int index) => base[index]);
            base.Fake.ItemAtIndexInt32 = ((int index) => base[index]);
            base.Fake.ItemGetGuid = ((Guid id) => this[id].Instance);
            base.Fake.ItemGetString = ((string webUrl) => this[webUrl].Instance);
            base.Fake.AddString = ((string webUrl) => this.Add(webUrl).Instance);
            base.Fake.DeleteString = (new FakesDelegates.Action<string>(this.Delete));
            base.Fake.NamesGet = (() => this.Names);
        }

        public SimSPWeb Add(string webUrl)
        {
            var simWeb = this.ParentWeb;
            string name = webUrl;
            int num = webUrl.LastIndexOf('/');
            if (num >= 0)
            {
                name = webUrl.Substring(num + 1);
                var url = webUrl.Substring(0, num);
                simWeb = this[url];
            }

            if (!simWeb.Exists)
            {
                throw new InvalidOperationException();
            }

            var simWeb2 = new SimSPWeb
            {
                ParentWeb = simWeb.Instance,
                Name = name
            };

            simWeb.Webs.Add(simWeb2.Instance);

            return simWeb2;
        }

        public void Delete(string webUrl)
        {
            var simWeb = this[webUrl];
            if (simWeb.Exists)
            {
                var simSPWeb2 = SimSPWeb.FromInstance(simWeb.ParentWeb);
                simSPWeb2.Webs.Remove(simWeb.Instance);
                return;
            }
            throw new FileNotFoundException();
        }

        internal static void Initialize()
        {
            ShimSPAutoSerializingObject.BehaveAsNotImplemented();
            ShimSPBaseCollection.BehaveAsNotImplemented();
            ShimSPWebCollection.BehaveAsNotImplemented();
        }
    }
}
