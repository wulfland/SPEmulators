using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePointSample.Solution.Core
{
    public interface ISalesOrderView
    {
        IEnumerable<Customer> Customers { get; set; }

        IEnumerable<SalesOrder> SalesOrders { get; set; }
    }
}
