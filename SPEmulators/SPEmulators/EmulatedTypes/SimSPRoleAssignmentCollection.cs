namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPRoleAssignmentCollection : CollectionIsolator<SPRoleAssignment, SPRoleAssignmentCollection, ShimSPRoleAssignmentCollection>
    {
        public SimSPRoleAssignmentCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPRoleAssignmentCollection>())
        {
        }

        public SimSPRoleAssignmentCollection(SPRoleAssignmentCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
        }
    }
}
