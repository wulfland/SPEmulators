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
        /// Indicates, that tests run against the SharePoint API as integration tests.
        /// </summary>
        Integration
    }
}
