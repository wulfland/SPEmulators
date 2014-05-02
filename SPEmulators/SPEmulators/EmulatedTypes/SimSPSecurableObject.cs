namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPSecurableObject : Isolator<SPSecurableObject, ShimSPSecurableObject>
    {
        internal SimSPRoleAssignmentCollection RoleAssignments
        {
            get;
            private set;
        }

        protected SimSPSecurableObject(SPSecurableObject instance)
            : base(instance)
        {
            this.RoleAssignments = new SimSPRoleAssignmentCollection();
            base.Fake.RoleAssignmentsGet = () => this.RoleAssignments.Instance;
        }

        public static SimSPSecurableObject FromInstance(SPSecurableObject instance)
        {
            return InstancedPool.CastAsInstanced<SPSecurableObject, SimSPSecurableObject>(instance);
        }
        internal static void Initialize()
        {
            ShimSPSecurableObject.BehaveAsNotImplemented();
        }
    }
}
