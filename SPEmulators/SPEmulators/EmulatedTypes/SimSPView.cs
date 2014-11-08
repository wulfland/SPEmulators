namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPView : Isolator<SPView, ShimSPView>
    {
        private string serverRelativeUrl;
        private string title;
        private string url;
        private readonly SimSPViewFieldCollection viewFields = 
            new SimSPViewFieldCollection();

        public Guid ID
        {
            get;
            set;
        }

        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        public string ServerRelativeUrl
        {
            get
            {
                return this.serverRelativeUrl;
            }
            set
            {
                this.serverRelativeUrl = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public SPList ParentList
        {
            get;
            set;
        }

        public SimSPView()
            : this(ShimRuntime.CreateUninitializedInstance<SPView>())
        {
        }

        public SimSPView(SPView instance)
            : base(instance)
        {
            viewFields.View = this.Instance;
            base.Fake.IDGet = (() => this.ID);
            base.Fake.UrlGet = (() => this.Url);
            base.Fake.ServerRelativeUrlGet = (() => this.ServerRelativeUrl);
            base.Fake.TitleGet = (() => this.Title);
            base.Fake.ParentListGet = (() => this.ParentList);
            base.Fake.ViewFieldsGet = () => this.viewFields.Instance;
        }

        public static SimSPView FromInstance(SPView instance)
        {
            return InstancedPool.CastAsInstanced<SPView, SimSPView>(instance);
        }
    }
}
