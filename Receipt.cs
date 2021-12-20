 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amco
{
   public class Receipt
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public double Price { get; set; }
        public int Length { get; set; }
        public int Quantity { get; set; }
        public string Total { get { return string.Format("¢{0}", Length * Price * Quantity); } }
    }
}
