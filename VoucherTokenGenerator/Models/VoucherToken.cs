namespace VoucherTokenGenerator.Models
{
    public class VoucherToken
    {
        // Unique identifier for the token
        public Guid TokenId { get; set; }

        // 16-digit token number
        public string TokenNumber { get; set; }
        
        // Value or amount of the voucher
        public int Value { get; set; }
        
        // Remaining value of the orginal amount
        public int RemainingValue { get; set; }

        // Date and time the token was created
        public DateTime CreationDate { get; set; }

        // Date and time the token will expire
        public DateTime ExpirationDate { get; set; }

        // Indicates if the token has been redeemed
        public bool IsRedeemed { get; set; }

        // Date and time the token was redeemed, if applicable
        public DateTime? RedemptionDate { get; set; }

        // Constructor to initialize the model with default values
        public VoucherToken()
        {
            TokenId = Guid.NewGuid();
            TokenNumber = string.Empty;
            CreationDate = DateTime.UtcNow;
            IsRedeemed = false;
        }
    }
}