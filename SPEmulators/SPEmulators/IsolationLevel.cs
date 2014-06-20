namespace SPEmulators
{
    /// <summary>
    /// The level of isolation for the tests.
    /// </summary>
    public enum IsolationLevel
    {
        /// <summary>
        /// Indicates, that tests run against a faked SharePoint API.
        /// </summary>
        Fake,

        /// <summary>
        /// Indicates, that tests run against the SharePoint API as integration tests. The shim context 
        /// will be inicialized and the SPWeb and SPSite objects will be added to a fake SPContext.
        /// </summary>
        Integration,

        /// <summary>
        /// Indicates, that tests run against the SharePoint API without a shim context. No SPContext will be initialized.
        /// </summary>
        None
    }
}
