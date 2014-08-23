using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using SalesOrderPortalSolution.WebParts;

namespace SalesOrderPortalSolution.Features.SalesOrderPortal
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("9a71d644-f2fb-4a1e-8ec1-9a8c3134d374")]
    public class SalesOrderPortalEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            using (var webPartManager = web.GetLimitedWebPartManager("Default.aspx", System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {
                var salesOrderWebPart = new SalesOrderWebPart();
                salesOrderWebPart.Title = "Sales Order";
                webPartManager.AddWebPart(salesOrderWebPart, "Left", 0);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            using (var webPartManager = web.GetLimitedWebPartManager("Default.aspx", System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {
                for (int i = webPartManager.WebParts.Count - 1; i >= 0; i--)
                {
                    if (webPartManager.WebParts[i].GetType() == typeof(SalesOrderWebPart))
                    {
                        webPartManager.DeleteWebPart(webPartManager.WebParts[i]);
                    }
                }
            }
        }
    }
}
