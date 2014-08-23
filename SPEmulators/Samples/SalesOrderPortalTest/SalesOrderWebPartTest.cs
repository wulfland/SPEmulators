using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesOrderPortalSolution.WebParts;
using SharePointSample.Solution.Core;

namespace SalesOrderPortalTest
{
    [TestClass]
    public class SalesOrderWebPartTest
    {
        SalesOrderWebPart webPart;
        IDisposable shimsContext;

        [TestInitialize]
        public void SetUp()
        {
            shimsContext = ShimsContext.Create();

            ICustomerRepository customerRepository = new InMemoryCustomerRepository();
            customerRepository.Add(new Customer { CustomerId = 0, Name = "Customer 1" });
            customerRepository.Add(new Customer { CustomerId = 1, Name = "Customer 2" });
            customerRepository.Add(new Customer { CustomerId = 2, Name = "Customer 3" });

            ISalesOrderRepository salesOrderRepository = new InMemorySalesOrderRepository();
            salesOrderRepository.Add(new SalesOrder { Customer = new Customer { CustomerId = 0, Name = "Customer 1" }, SalesOrderId = 0, Lines = new List<OrderLine> { new OrderLine { Price = 5, Quantity = 5 } } });

            webPart = new SalesOrderWebPart();
            var page = new ShimPage() { IsPostBackGet = () => { return false; } };
            page.BehaveAsDefaultValue();

            var molesWebPart = new ShimControl((Control)webPart) { PageGet = () => { return page; } };
            webPart.Inject(customerRepository, salesOrderRepository);
        }

        [TestMethod]
        public void CanListCustomer()
        {
            var accessor = new PrivateObject(webPart);
            accessor.Invoke("CreateChildControls");

            var baseAccessor = new PrivateObject(webPart, new PrivateType(typeof(System.Web.UI.Control)));
            baseAccessor.Invoke("OnLoad", new EventArgs());

            Assert.AreEqual(3, webPart.CustomersDropDownList.Items.Count);
            Assert.AreEqual(3, webPart.Customers.Count());
            Assert.AreEqual(1, webPart.SalesOrderGrid.Rows.Count);
            Assert.AreEqual(1, webPart.SalesOrders.Count());
        }

        [TestMethod]
        public void CanSwitchCustomer()
        {
            var accessor = new PrivateObject(webPart);
            accessor.Invoke("CreateChildControls");

            var baseAccessor = new PrivateObject(webPart, new PrivateType(typeof(System.Web.UI.Control)));
            baseAccessor.Invoke("OnLoad", new EventArgs());

            Assert.AreEqual(3, webPart.CustomersDropDownList.Items.Count);
            Assert.AreEqual(1, webPart.SalesOrderGrid.Rows.Count);

            webPart.CustomersDropDownList.SelectedIndex = 1;
            webPart.OnCustomersDropDownListSelectedIndexChanged(null, null);
            Assert.AreEqual(0, webPart.SalesOrderGrid.Rows.Count);
        }

        [TestCleanup]
        public void TearDown()
        {
            InMemoryCustomerRepository.Reset();
            InMemorySalesOrderRepository.Reset();
            shimsContext.Dispose();
        }
    }
}
