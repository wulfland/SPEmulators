namespace SPEmulators.EmulatedTypes
{
    using System.Threading;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal static class SimSPSecurity
    {
        public static void Initialize()
        {
            ShimSPSecurity.BehaveAsNotImplemented();
            ShimSPSecurity.RunWithElevatedPrivilegesWaitCallbackObject = delegate(WaitCallback c, object o)
            {
                c(o);
            };
            ShimSPSecurity.RunWithElevatedPrivilegesSPSecurityCodeToRunElevated = delegate(SPSecurity.CodeToRunElevated d)
            {
                d.Invoke();
            };
        }
    }
}
