using System.Linq;
using Microsoft.SharePoint;
using SharePointSample.Solution.Core;
using SPEmulators;

namespace SalesOrderPortalTest
{
    public class CustomSPEmulationContext : SPEmulationContext
    {
        SPList customerList;
        bool disposed;

        public CustomSPEmulationContext(IsolationLevel isolationLevel, string url)
            : base(isolationLevel, url)
        {
            this.customerList = GetOrCreateList(CustomerListRepository.ListName, SPListTemplateType.GenericList);
        }

        public SPList CustomerList
        {
            get
            {
                return customerList;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                ClearList(customerList, 3);
                disposed = true;
            }

            base.Dispose(disposing);
        }

        protected virtual void ClearList(SPList list, int startId = 0)
        {
            if (this.IsolationLevel != IsolationLevel.Fake)
            {
                var itemsToClear = from i in list.Items.Cast<SPListItem>()
                                   where i.ID >= startId
                                   select i.ID;

                foreach (var id in itemsToClear)
                    list.Items.DeleteItemById(id);
            }
        }
    }
}
