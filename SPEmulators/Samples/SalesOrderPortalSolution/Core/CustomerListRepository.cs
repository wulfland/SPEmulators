using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace SharePointSample.Solution.Core
{
    public class CustomerListRepository : ICustomerRepository
    {
        public const string ListName = "Customers";

        public void Add(Customer customer)
        {
            var web = SPContext.Current.Web;
            var list = web.Lists[ListName];
            var item = list.AddItem();
            item["Title"] = customer.Name;

            item.Update();
        }

        public IEnumerable<Customer> GetAll()
        {
            var web = SPContext.Current.Web;
            var list = web.Lists[ListName];
            var results = new List<Customer>();

            foreach (SPListItem item in list.Items)
            {
                results.Add(new Customer { Name = item.Title, CustomerId = item.ID });
            }

            return results.AsReadOnly();
        }

        public Customer GetByName(string name)
        {
            var web = SPContext.Current.Web;
            var list = web.Lists[ListName];
            var query = new SPQuery();
            query.Query = @"
    <Where>
        <Eq><FieldRef Name='Title' />
            <Value Type='Text'>" + name + @"</Value>
        </Eq>
    </Where>";
            query.RowLimit = 1;

            var results = list.GetItems(query);
            if (results.Count == 0)
            {
                return null;
            }

            var item = results.Cast<SPListItem>().First();

            return new Customer
            {
                Name = item.Title,
                CustomerId = item.ID
            };
        }


        public Customer GetById(int customerId)
        {
            var web = SPContext.Current.Web;
            var list = web.Lists[ListName];

            try
            {
                var item = list.GetItemById(customerId);
                return new Customer { Name = item.Title, CustomerId = item.ID };
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
