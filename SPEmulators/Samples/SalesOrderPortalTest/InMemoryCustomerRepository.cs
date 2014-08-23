using System.Collections.Generic;
using SharePointSample.Solution.Core;
using System.Linq;

namespace SalesOrderPortalTest
{
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        static IList<Customer> customers = new List<Customer>();

        public void Add(Customer customer)
        {
            customers.Add(customer);
        }

        public IEnumerable<Customer> GetAll()
        {
            return customers;
        }

        public Customer GetByName(string name)
        {
            return customers.Where(c => c.Name == name).FirstOrDefault();
        }

        public Customer GetById(int customerId)
        {
            return customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
        }

        public static void Reset()
        {
            customers = new List<Customer>();
        }
    }
}
