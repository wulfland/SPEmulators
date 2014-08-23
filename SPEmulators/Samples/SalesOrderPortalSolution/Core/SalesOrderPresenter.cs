using System.Collections.Generic;
using System.Linq;

namespace SharePointSample.Solution.Core
{
    public class SalesOrderPresenter
    {
        ISalesOrderView view;
        ICustomerRepository customerRepository;
        ISalesOrderRepository salesOrderRepository;

        public SalesOrderPresenter(ISalesOrderView view, ICustomerRepository customerRepository, ISalesOrderRepository salesOrderRepository)
        {
            this.view = view;
            this.customerRepository = customerRepository;
            this.salesOrderRepository = salesOrderRepository;
        }


        public void Initialize()
        {
            view.Customers = this.customerRepository.GetAll();
            SelectCustomer(view.Customers.First().CustomerId);
        }

        public void SelectCustomer(int customerId)
        {
            var selectedCustomer = this.customerRepository.GetById(customerId);
            view.SalesOrders = salesOrderRepository.GetByCustomer(selectedCustomer);
        }
    }
}
