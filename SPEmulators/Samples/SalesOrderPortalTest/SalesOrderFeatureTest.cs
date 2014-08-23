using System;
using System.Linq;
using Microsoft.SharePoint.Fakes;
using Microsoft.SharePoint.WebPartPages.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesOrderPortalSolution.WebParts;
using SalesOrderPortalTest.Properties;
using SPEmulators;

namespace SalesOrderPortalTest
{
    [TestClass]
    public class SalesOrderFeatureTest
    {
        static readonly Guid featureId = new Guid("f28fad6f-c8df-4fcf-98f9-6c509b85c25d");

        [TestMethod]
        public void FeatureReceiverAddsWebPartTest()
        {
            using (var context = new SPEmulationContext(Settings.Default.IsolationLevel, Settings.Default.Url))
            {
                if (context.IsolationLevel == IsolationLevel.Fake)
                {
                    var webPartManager = new ShimSPLimitedWebPartManager();
                    new ShimSPWeb(context.Web)
                    {
                        GetLimitedWebPartManagerStringPersonalizationScope = (s, c) =>
                        {
                            return webPartManager;
                        },
                    };

                    webPartManager.WebPartsGet = () =>
                    {
                        var webPartCollection = new ShimSPLimitedWebPartCollection();
                        webPartCollection.ItemGetInt32 = (i) => { return new SalesOrderWebPart(); };

                        return webPartCollection.Instance;
                    };
                }

                var feature = context.Web.Features.FirstOrDefault(f => f.DefinitionId == featureId);
                if (feature == null)
                    context.Web.Features.Add(featureId);

                AssertIfWebpartOnPage(context, true);

                context.Web.Features.Remove(featureId);
            }
        }

        private void AssertIfWebpartOnPage(SPEmulationContext context, bool expected)
        {
            Assert.AreEqual<bool>(expected, IsWebpartOnPage(context));
        }

        private bool IsWebpartOnPage(SPEmulationContext context)
        {
            using (var webPartManager = context.Web.GetLimitedWebPartManager("Default.aspx",
                System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {

                var count = context.IsolationLevel != IsolationLevel.Fake ? webPartManager.WebParts.Count : 1;

                for (int i = count - 1; i >= 0; i--)
                {
                    if (webPartManager.WebParts[i].GetType() == typeof(SalesOrderWebPart))
                        return true;
                }

                return false;
            }
        }
    }
}
