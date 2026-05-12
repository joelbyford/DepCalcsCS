using System;
using System.Collections.Generic;

namespace DepCalcsCS
{
    public class AdsDepreciationResult
    {
        public string AssetName { get; set; }
        public string AssetClass { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double PurchasePrice { get; set; }
        public double ResidualValue { get; set; }
        public double RecoveryPeriod { get; set; }
        public string Convention { get; set; }
        public List<DepYear> DepreciationTable { get; set; }
    }
}
