namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPContentType : Isolator<SPContentType, ShimSPContentType>
    {
        private readonly SimSPFieldLinkCollection fieldLinks;
        private bool readOnly;
        private bool readOnlyOld;

        public SPContentTypeId Id
        {
            get;
            set;
        }

        public SPContentType Parent
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Group
        {
            get;
            set;
        }

        public bool Updated
        {
            get;
            private set;
        }

        public SimSPFieldLinkCollection FieldLinks
        {
            get
            {
                return this.fieldLinks;
            }
        }

        public SimSPContentType()
            : this(ShimRuntime.CreateUninitializedInstance<SPContentType>())
        {
        }

        public SimSPContentType(SPContentType instance)
            : base(instance)
        {
            this.fieldLinks = new SimSPFieldLinkCollection();
            base.Fake.ParentGet = () => this.Parent;
            base.Fake.IdGet = () => this.Id;
            base.Fake.FieldLinksGet = () => this.fieldLinks.Instance;
            base.Fake.GroupGet = () => this.Group;
            base.Fake.GroupSetString = (string value) =>
            {
                this.Group = value;
            };
            base.Fake.NameGet = () => this.Name;
            base.Fake.NameSetString = (string value) =>
            {
                this.Name = value;
            };
            base.Fake.ReadOnlyGet = () => this.readOnly;
            base.Fake.ReadOnlySetBoolean = (bool value) =>
            {
                this.readOnlyOld = this.readOnly;
                this.readOnly = value;
            };
            base.Fake.ReadOnlyOldGet = () => this.readOnlyOld;
            base.Fake.Update = () =>
            {
                this.Updated = true;
            };
        }

        public static void Initialize()
        {
            ShimSPContentType.BehaveAsNotImplemented();
            ShimSPContentType.StaticConstructor = () =>
            {
            };
            ShimSPContentType.ConstructorSPContentTypeIdSPContentTypeCollectionString = (SPContentType me, SPContentTypeId id, SPContentTypeCollection contentTypes, string name) =>
            {
                var emSPContentType = new SimSPContentType(me)
                {
                    Id = id,
                    Name = name
                };
                emSPContentType.FieldLinks.Clear();
            };
            ShimSPContentType.ConstructorSPContentTypeSPContentTypeCollectionString = (SPContentType me, SPContentType parentContentType, SPContentTypeCollection contentTypes, string name) =>
            {
                var emSPContentType = new SimSPContentType(me)
                {
                    Name = name,
                    Parent = parentContentType
                };
                emSPContentType.FieldLinks.Clear();
            };
        }

        public static SimSPContentType FromInstance(SPContentType instance)
        {
            return InstancedPool.CastAsInstanced<SPContentType, SimSPContentType>(instance);
        }
    }
}
