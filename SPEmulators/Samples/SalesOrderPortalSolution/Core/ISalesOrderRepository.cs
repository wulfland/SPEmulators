using System.Collections.Generic;

namespace SharePointSample.Solution.Core
{
    public interface ISalesOrderRepository
    {
        IEnumerable<SalesOrder> GetByCustomer(Customer customer);

        void Add(SalesOrder order);
    }
}
