using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharePointSample.Solution.Core;

namespace SalesOrderPortalTest
{
    public class InMemorySalesOrderRepository : ISalesOrderRepository
    {
        static IList<SalesOrder> salesOrders = new List<SalesOrder>();

        public IEnumerable<SalesOrder> GetByCustomer(Customer customer)
        {
            return salesOrders.Where(so => so.Customer.CustomerId == customer.CustomerId);
        }

        public void Add(SalesOrder order)
        {
            salesOrders.Add(order);
        }

        public static void Reset()
        {
            salesOrders = new List<SalesOrder>();
        }
    }
}
