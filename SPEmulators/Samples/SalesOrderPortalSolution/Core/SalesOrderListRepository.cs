using System;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace SharePointSample.Solution.Core
{
    public class SalesOrderListRepository : ISalesOrderRepository
    {
        public const string SalesOrderListName = "Sales Orders";
        public const string OrderLinesListName = "Order Lines";

        public IEnumerable<SalesOrder> GetByCustomer(Customer customer)
        {
            var web = SPContext.Current.Web;
            var salesOrderList = web.Lists[SalesOrderListName];
            var orderLineList = web.Lists[OrderLinesListName];

            var query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name='Customer' LookupId='TRUE' /><Value Type='Lookup' >" + customer.CustomerId.ToString() + "</Value></Eq></Where>";
            query.RowLimit = 100;
            var results = salesOrderList.GetItems(query);

            var salesOrders = new List<SalesOrder>();
            foreach (SPListItem item in results)
            {
                var salesOrder = GetSalesOrderHeader(customer, item);
                GetOrderLines(orderLineList, salesOrder);
                salesOrders.Add(salesOrder);
            }

            return salesOrders;
        }

        private static SalesOrder GetSalesOrderHeader(Customer customer, SPListItem item)
        {
            return new SalesOrder
            {
                Customer = customer,
                OrderDate = (DateTime)item["OrderDate"],
                SalesOrderId = item.ID,
                Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), item["Status"].ToString())
            };
        }

        private static void GetOrderLines(SPList orderLineList, SalesOrder salesOrder)
        {
            var lineQuery = new SPQuery();
            lineQuery.Query = "<Where><Eq><FieldRef Name='SalesOrder' LookupId='TRUE' /><Value Type='Lookup' >" + salesOrder.SalesOrderId + "</Value></Eq></Where>";
            var orderLines = orderLineList.GetItems(lineQuery);

            foreach (SPListItem line in orderLines)
            {
                salesOrder.Lines.Add(new OrderLine { Product = line.Title, Price = (double)line["Price"], Quantity = Convert.ToInt32(line["Quantity"]) });
            }
        }

        public void Add(SalesOrder order)
        {
            var web = SPContext.Current.Web;
            var salesOrderList = web.Lists[SalesOrderListName];
            var orderLineList = web.Lists[OrderLinesListName];

            AddSalesOrderHeader(order, salesOrderList);

            foreach (var line in order.Lines)
            {
                AddOrderLine(orderLineList, line);
            }
        }

        private static void AddSalesOrderHeader(SalesOrder order, SPList salesOrderList)
        {
            var salesOrder = salesOrderList.AddItem();
            salesOrder["Title"] = order.Customer.Name + "_" + order.OrderDate.ToString("yyyy-MM-dd");
            salesOrder["OrderDate"] = order.OrderDate;
            salesOrder["Status"] = order.Status.ToString();
            salesOrder["Customer"] = order.Customer.CustomerId;
            salesOrder.Update();
        }

        private static void AddOrderLine(SPList orderLineList, OrderLine line)
        {
            var orderLine = orderLineList.AddItem();
            orderLine["Title"] = line.Product;
            orderLine["Price"] = line.Price;
            orderLine["Quantity"] = line.Quantity;
            orderLine.Update();
        }
    }
}
