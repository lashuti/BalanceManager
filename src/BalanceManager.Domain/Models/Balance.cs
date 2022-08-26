using System;
using System.Collections.Generic;


namespace BalanceManager.Domain.Models
{
    public partial class Balance
    {
        public string Type { get; set; }
        public decimal? Amount { get; set; }
    }
}
