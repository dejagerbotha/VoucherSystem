using System;
using System.Collections.Generic;
using VoucherTokenGenerator.Models;

namespace VoucherTokenGenerator.Services
{
    public class TokenService
    {
        private readonly Dictionary<string, VoucherToken> _tokens = new();
        private readonly HashSet<string> existingTokenNumbers = new HashSet<string>();

        // Method to generate a new voucher token
        public VoucherToken GenerateToken(int validMonths = 18, int value = 0)
        {
            var token = new VoucherToken
            {
                TokenNumber = GenerateUniqueTokenNumber(),
                Value = value,
                RemainingValue = value,  // Set initial remaining value to full value
                ExpirationDate = DateTime.UtcNow.AddMonths(validMonths)
            };

            // Add the token to the in-memory dictionary using TokenNumber as the key
            _tokens[token.TokenNumber] = token;

            return token;
        }

        // Method to redeem a token by its TokenNumber
        public RedeemResponse RedeemToken(string tokenNumber, int? redeemAmount = 0)
        {
            var token = GetTokenByNumber(tokenNumber);

            if (token == null)
            {
                throw new InvalidOperationException($"Token {tokenNumber} does not exist.");
            }

            if (token.IsRedeemed)
            {
                throw new InvalidOperationException($"Token {tokenNumber} has already been redeemed.");
            }

            // Determine the redeem amount
            var amountToRedeem = redeemAmount ?? token.Value;  // Default to full value if redeemAmount is not provided

            if (amountToRedeem > token.Value)
            {
                throw new InvalidOperationException($"Cannot redeem more than the voucher value ({token.Value}).");
            }

            // Update the token value after redemption
            token.Value -= amountToRedeem;

            // If the entire value has been redeemed, mark the token as redeemed
            if (token.Value == 0)
            {
                token.IsRedeemed = true;
                token.RedemptionDate = DateTime.UtcNow;
            }

            // Return the response with redeemed amount, remaining balance, and a message
            return new RedeemResponse
            {
                TokenNumber = tokenNumber,
                RedeemedAmount = amountToRedeem,
                RemainingAmount = token.Value,
                Message = $"Successfully redeemed {amountToRedeem}. Remaining balance: {token.Value}."
            };
        }

        // Helper method to create a unique 16-digit token number
        private string GenerateUniqueTokenNumber()
        {
            string tokenNumber;
            do
            {
                tokenNumber = GenerateTokenNumber();
            } while (existingTokenNumbers.Contains(tokenNumber));

            existingTokenNumbers.Add(tokenNumber); // Track the generated number
            return tokenNumber;
        }

        // Basic random 16-digit generator method
        private string GenerateTokenNumber()
        {
            var random = new Random();
            var tokenNumber = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                tokenNumber += random.Next(1000, 9999); // Append a random 4-digit segment
            }
            return tokenNumber;
        }
        
        // Method to get a token by TokenNumber
        public VoucherToken? GetTokenByNumber(string tokenNumber)
        {
            // Return the token if found, or null if not found
            _tokens.TryGetValue(tokenNumber, out var token);
            return token;
        }
    }
}
