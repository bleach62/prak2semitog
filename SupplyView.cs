using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prak2semitog
{
    public class SupplyView
    {
        public int IdSupply { get; set; }
        public int IdAgent { get; set; }
        public string Agent { get; set; }
        public int IdClient { get; set; }
        public string Client { get; set; }
        public int IdRealEstate { get; set; }
        public string RealEstate { get; set; }
        public int Price { get; set; }
    }
}
