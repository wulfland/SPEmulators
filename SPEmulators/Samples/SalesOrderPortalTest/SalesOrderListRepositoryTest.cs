using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesOrderPortalTest.Properties;
using SharePointSample.Solution.Core;
using SPEmulators;

namespace SalesOrderPortalTest
{
    [TestClass]
    public class SalesOrderListRepositoryTest
    {
        [TestMethod]
        public void CanGetSalesOrderByCustomer()
        {
            using (var context = new SPEmulationContext(Settings.Default.IsolationLevel, Settings.Default.Url))
            {
                var solutionRelativePath = @"..\..\..\SalesOrderPortalSolution\Lists\";
                var salesOrderList = context.GetOrCreateList(solutionRelativePath + "SalesOrders\\Elements.xml", solutionRelativePath + "SalesOrders\\schema.xml");
                var orderLinesList = context.GetOrCreateList(solutionRelativePath + "OrderLines\\Elements.xml", solutionRelativePath + "OrderLines\\schema.xml");

                var repository = new SalesOrderListRepository();

                // CAML not yet supported. Fake queries.
                if (context.IsolationLevel == IsolationLevel.Fake)
                {
                    new ShimSPList(salesOrderList)
                    {
                        GetItemsSPQuery = (q) =>
                            {
                                var shim = new ShimSPListItemCollection();
                                shim.Bind(salesOrderList.Items.Cast<SPListItem>().Where(i => (int)i["Customer"] == 1));

                                return shim.Instance;
                            }
                    };

                    new ShimSPList(orderLinesList)
                    {
                        GetItemsSPQuery = (q) =>
                        {
                            var shim = new ShimSPListItemCollection();
                            var match = Regex.Match(q.Query, "<FieldRef Name='SalesOrder' LookupId='TRUE' /><Value Type='Lookup' >.*?</Value>");
                            var lookupId = int.Parse(match.Value.Replace("<FieldRef Name='SalesOrder' LookupId='TRUE' /><Value Type='Lookup' >", "").Replace("</Value>", ""));
                            shim.Bind(orderLinesList.Items.Cast<SPListItem>().Where(i => (int)i["SalesOrder"] == 1));

                            return shim.Instance;
                        }
                    };
                }

                var customer1 = new Customer
                {
                    CustomerId = 1,
                    Name = "Customer 1"
                };

                var salesOrdersOfCustomer1 = repository.GetByCustomer(customer1);
                Assert.AreEqual<int>(2, salesOrdersOfCustomer1.Count());
                Assert.AreEqual<int>(2, salesOrdersOfCustomer1.First().Lines.Count);
            }
        }

        [TestMethod]
        public void CanCreateSalesOrder()
        {
            using (var context = new SPEmulationContext(Settings.Default.IsolationLevel, Settings.Default.Url))
            {
                var salesOrderList = context.GetOrCreateList(
                    SalesOrderListRepository.SalesOrderListName,
                    SPListTemplateType.GenericList,
                    "OrderDate", "Customer", "Status");

                var orderLinesList = context.GetOrCreateList(
                    SalesOrderListRepository.OrderLinesListName,
                    SPListTemplateType.GenericList,
                    "SalesOrder", "Price", "Quantity");

                var repository = new SalesOrderListRepository();

                var customer = new Customer
                {
                    CustomerId = 2,
                    Name = "Customer 2"
                };

                var order = new SalesOrder
                {
                    SalesOrderId = 0,
                    Customer = customer,
                    OrderDate = DateTime.Today,
                    Status = OrderStatus.New
                };


                order.Lines.Add(new OrderLine
                {
                    Product = "Product 1",
                    Quantity = 5,
                    Price = 5.5
                });

                order.Lines.Add(new OrderLine
                {
                    Product = "Product 2",
                    Quantity = 2,
                    Price = 106.5
                });

                int salesOrderCount = salesOrderList.ItemCount;
                int orderLineCount = orderLinesList.ItemCount;

                repository.Add(order);

                Assert.AreEqual<int>(salesOrderCount + 1, salesOrderList.Items.Count);
                Assert.AreEqual<int>(orderLineCount + 2, orderLinesList.Items.Count);
            }
        }
    }
}
