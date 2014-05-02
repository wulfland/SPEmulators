namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFieldLink : Isolator<SPFieldLink, ShimSPFieldLink>
    {
        private SPField field;

        public SPField Field
        {
            get
            {
                if (this.field == null)
                {
                    this.field = new SimSPField().Instance;
                }

                return this.field;
            }
            set
            {
                this.field = value;
            }
        }

        public SimSPFieldLink()
            : this(ShimRuntime.CreateUninitializedInstance<SPFieldLink>())
        {
        }

        public SimSPFieldLink(SPFieldLink instance)
            : base(instance)
        {
            base.Fake.FieldGet = () => this.Field;
            base.Fake.NameGet = () => this.Field.InternalName;
            base.Fake.IdGet = () => this.Field.Id;
        }

        public static void Initialize()
        {
            ShimSPFieldLink.BehaveAsNotImplemented();
            ShimSPFieldLink.StaticConstructor = delegate
            {
            };
            ShimSPFieldLink.ConstructorSPField = delegate(SPFieldLink me, SPField field)
            {
                var fieldLink = new SimSPFieldLink(me);
                fieldLink.Field = field;
            };
        }
    }
}
