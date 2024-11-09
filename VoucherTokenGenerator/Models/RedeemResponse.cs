using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherTokenGenerator.Models
{
    public class RedeemResponse
    {
        public string? TokenNumber { get; set; }
        public int RedeemedAmount { get; set; }
        public int RemainingAmount { get; set; }
        public string? Message { get; set; }
    }
}
