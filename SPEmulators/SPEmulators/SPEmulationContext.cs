namespace SPEmulators
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;

    /// <summary>
    /// The emulation context for SharePoint emulation.
    /// </summary>
    public class SPEmulationContext : IDisposable
    {
        readonly IDisposable shimContext;
        readonly IsolationLevel isolationLevel;
        bool disposed;

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        /// <value>
        /// The isolation level.
        /// </value>
        public IsolationLevel IsolationLevel
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return isolationLevel;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPEmulationContext"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public SPEmulationContext(IsolationLevel level)
        {
            isolationLevel = level;
            shimContext = ShimsContext.Create();

            if (!Environment.Is64BitProcess)
                throw new InvalidOperationException("SharePoint tests must run in 64 bit process.");

            if (isolationLevel == SPEmulators.IsolationLevel.Fake)
            {
                // todo: initialize fakes
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (shimContext != null)
                    shimContext.Dispose();

                disposed = true;
            }
        }
    }
}
