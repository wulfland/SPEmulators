namespace SPEmulators
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;
    using SPEmulators.EmulatedTypes;

    /// <summary>
    /// The emulation context for SharePoint emulation.
    /// </summary>
    public class SPEmulationContext : IDisposable
    {
        readonly IDisposable shimContext;
        readonly IsolationLevel isolationLevel;
        bool disposed;
        SPWeb web;
        SPSite site;

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
        /// Gets the current web.
        /// </summary>
        public SPWeb Web
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return web;
            }
        }

        /// <summary>
        /// Gets the current site.
        /// </summary>
        public SPSite Site
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return site;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPEmulationContext"/> class.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        public SPEmulationContext(IsolationLevel isolationLevel)
            : this(isolationLevel, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPEmulationContext"/> class.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        public SPEmulationContext(IsolationLevel isolationLevel, string url)
        {
            if (!Environment.Is64BitProcess)
                throw new InvalidOperationException("SharePoint tests must run in 64 bit process.");

            this.isolationLevel = isolationLevel;

            switch (isolationLevel)
            {
                case IsolationLevel.Fake:
                    // create shim context
                    shimContext = ShimsContext.Create();

                    // initialize all simulated types
                    InitializeSimulatedAPI();

                    // Set reference to the simulated site and web in the context
                    site = SPContext.Current.Site;
                    web = SPContext.Current.Web;
                    break;
                case IsolationLevel.Integration:
                    // create shim context
                    shimContext = ShimsContext.Create();

                    // Load the real spite and spweb objects from sharpoint
                    site = new SPSite(url);
                    web = site.OpenWeb();

                    // Inject the real webs to the context using shims.
                    ShimSPContext.CurrentGet = () => new ShimSPContext
                    {
                        SiteGet = () => this.site,
                        WebGet = () => this.web
                    };
                    break;
                case IsolationLevel.None:
                    // Do not use shimscontext or any kind of fake. Load the real spite and spweb objects from sharpoint.
                    site = new SPSite(url);
                    web = site.OpenWeb();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static void InitializeSimulatedAPI()
        {
            SimHttpContext.Initialize();
            SimHttpRequest.Initialize();
            SimHttpResponse.Initialize();
            SimSPContext.Initialize();
            SimSPEventPropertiesBase.Initialize();
            SimSPField.Initialize();
            SimSPFieldCollection.Initialize();
            SimSPFieldIndex.Initialize();
            SimSPFieldIndexCollection.Initialize();
            SimSPFieldLink.Initialize();
            SimSPFieldLinkCollection.Initialize();
            SimSPFieldUrlValue.Initialize();
            SimSPFile.Initialize();
            SimSPFileCollection.Initialize();
            SimSPFolder.Initialize();
            SimSPFolderCollection.Initialize();
            SimSPItem.Initialize();
            SimSPItemEventDataCollection.Initialize();
            SimSPItemEventProperties.Initialize();
            SimSPList.Initialize();
            SimSPListCollection.Initialize();
            SimSPListEventProperties.Initialize();
            SimSPListItem.Initialize();
            SimSPListItemCollection.Initialize();
            SimSPSecurableObject.Initialize();
            SimSPSecurity.Initialize();
            SimSPSite.Initialize();
            SimSPWeb.Initialize();
            SimSPWebCollection.Initialize();
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
                if (web != null)
                    web.Dispose();

                if (site != null)
                    site.Dispose();

                if (shimContext != null)
                    shimContext.Dispose();

                disposed = true;
            }
        }

        /// <summary>
        /// Gets or creates a list depending of the current isolation level.
        /// If the isolation level is integration or none the function loads the list from the current web (the url that was specified in the constructor).
        /// If the isolation level is fake a list will be added to the faked web instance.
        /// </summary>
        /// <param name="name">The name of the list.</param>
        /// <param name="type">The type (SPListTemplateType) of the list.</param>
        /// <param name="fields">An optional array of strings. For each value a text field will be added to the list.</param>
        /// <returns>The list instance.</returns>
        public virtual SPList GetOrCreateList(string name, SPListTemplateType type, params string[] fields)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (isolationLevel == IsolationLevel.Integration || isolationLevel == SPEmulators.IsolationLevel.None)
            {
                return web.Lists[name];
            }
            else
            {
                var id = web.Lists.Add(name, string.Empty, type);
                var list = web.Lists[id];
                if (fields.Length > 0)
                {
                    Array.ForEach(fields, (s) =>
                    {
                        list.Fields.Add(s, SPFieldType.Text, false);
                    });

                    list.Update();
                }

                return list;
            }
        }

        /// <summary>
        /// Gets or creates a list depending of the current isolation level.
        /// If the isolation level is integration or none the function loads the list from the current web (the url that was specified in the constructor).
        /// If the isolation level is fake a list will be added to the faked web instance.
        /// </summary>
        /// <param name="pathToElementsXml">The path to the elements.xml that contains the list instance.</param>
        /// <returns>The list instance.</returns>
        public virtual SPList GetOrCreateList(string pathToElementsXml, string pathToSchemaXml = null)
        {
            if (string.IsNullOrEmpty(pathToElementsXml))
                throw new ArgumentNullException("relativePathToElementsXml");

            var elements = new Elements(pathToElementsXml);

            if (isolationLevel == IsolationLevel.Integration || isolationLevel == SPEmulators.IsolationLevel.None)
            {
                return web.Lists[elements.ListTitle];
            }
            else
            {
                var list = elements.CreateListInstance(web);

                if (!string.IsNullOrWhiteSpace(pathToSchemaXml))
                {
                    var schema = new Schema(pathToSchemaXml);
                    schema.AddFieldsToList(list);
                }

                elements.AddDefaultData(list);

                return list;
            }
        }
    }
}
