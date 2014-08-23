using System;
using System.Collections.Generic;
using System.Linq;

namespace SharePointSample.Solution.Core
{
    public class SalesOrder
    {
        public SalesOrder()
        {
            this.Lines = new List<OrderLine>();
        }

        public int SalesOrderId { get; set; }

        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public IList<OrderLine> Lines { get; set; }

        public OrderStatus Status { get; set; }

        public double Ordervalue
        {
            get
            {
                return Lines.Sum(o => o.Total);
            }
        }
    }
}
