using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesOrderPortalTest.Properties;
using SharePointSample.Solution.Core;
using SPEmulators;

namespace SalesOrderPortalTest
{
    [TestClass]
    public class CustomerListRepositoryTest
    {
        CustomSPEmulationContext context;
        CustomerListRepository repository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new CustomSPEmulationContext(Settings.Default.IsolationLevel, Settings.Default.Url);
            repository = new CustomerListRepository();
        }

        [TestMethod]
        public void CanAddCustomer()
        {
            var itemCount = context.CustomerList.ItemCount;
            repository.Add(new Customer { Name = "Customer 3" });

            Assert.AreEqual(itemCount + 1, context.CustomerList.Items.Count);
        }

        [TestMethod]
        public void CanGetCustomerByName()
        {
            if (context.IsolationLevel == IsolationLevel.Fake)
            {
                var item2 = context.CustomerList.Items.Add();
                item2["Title"] = "Customer 1";
                item2.Update();

                new ShimSPList(context.CustomerList)
                {
                    GetItemsSPQuery = (q) =>
                    {
                        var shim = new ShimSPListItemCollection();
                        shim.Bind(new[] { item2 });
                        return shim.Instance;
                    }
                };
            }

            var result = repository.GetByName("Customer 1");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCustomerByNameReturnsNull()
        {
            if (context.IsolationLevel == IsolationLevel.Fake)
            {
                var item = context.CustomerList.Items.Add();
                item["Title"] = "Customer 1";
                item.Update();

                new ShimSPList(context.CustomerList)
                {
                    GetItemsSPQuery = (q) =>
                    {
                        var shim = new ShimSPListItemCollection();
                        shim.Bind(new SPListItem[0]);
                        return shim.Instance;
                    }
                };
            }

            var result = repository.GetByName("Customer XXX");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CanGetCustomerById()
        {
            if (context.IsolationLevel == IsolationLevel.Fake)
            {
                var item = context.CustomerList.Items.Add();
                item["Title"] = "Customer 1";
                item.Update();
            }

            var result = repository.GetById(1);
            Assert.IsNotNull(result);
            Assert.AreEqual("Customer 1", result.Name);
        }

        [TestMethod]
        public void GetCustomerByIdReturnsNull()
        {
            var repository = new CustomerListRepository();
            var result = repository.GetById(10);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CanGetListOfCustomers()
        {
            if (context.IsolationLevel == IsolationLevel.Fake)
            {
                var item = context.CustomerList.Items.Add();
                item["Title"] = "Customer 1";
                item.Update();

                var item2 = context.CustomerList.Items.Add();
                item2["Title"] = "Customer 1";
                item2.Update();

                new ShimSPList(context.CustomerList)
                {
                    GetItemsSPQuery = (q) =>
                    {
                        var shim = new ShimSPListItemCollection();
                        shim.Bind(new[] { item, item2 });
                        return shim.Instance;
                    }
                };
            }

            var result = repository.GetAll();
            Assert.AreEqual(2, result.Count());
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            context.Dispose();
        }
    }
}
