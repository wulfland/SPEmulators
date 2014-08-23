using System.Collections.Generic;

namespace SharePointSample.Solution.Core
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);

        IEnumerable<Customer> GetAll();

        Customer GetByName(string name);

        Customer GetById(int customerId);
    }
}
