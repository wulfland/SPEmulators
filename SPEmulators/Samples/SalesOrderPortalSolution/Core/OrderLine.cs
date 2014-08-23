
namespace SharePointSample.Solution.Core
{
    public class OrderLine
    {
        public string Product { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double Total
        {
            get
            {
                return Price * Quantity;
            }
        }
    }
}
