namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;
    using Microsoft.SharePoint.WebPartPages;

    internal class SimSPViewContext : Isolator<SPViewContext, ShimSPViewContext>
    {
        public SPView View
        {
            get;
            set;
        }

        public ViewType ViewType
        {
            get;
            set;
        }

        public SimSPViewContext()
            : this(ShimRuntime.CreateUninitializedInstance<SPViewContext>())
        {
        }

        public SimSPViewContext(SPViewContext instance)
            : base(instance)
        {
            base.Fake.ViewGet = (() => this.View);
            base.Fake.ViewSetSPView = (delegate(SPView view)
            {
                this.View = view;
            });
            base.Fake.ViewIdGet = (() => this.View.ID);
            base.Fake.ViewTypeGet = (() => this.ViewType);
            base.Fake.ViewTypeSetViewType = (delegate(ViewType value)
            {
                this.ViewType = value;
            });
        }

        public SimSPView SetView()
        {
            var view = new SimSPView();
            this.View = view.Instance;
            return view;
        }

        public static SimSPViewContext FromInstance(SPViewContext instance)
        {
            return InstancedPool.CastAsInstanced<SPViewContext, SimSPViewContext>(instance);
        }
    }
}
