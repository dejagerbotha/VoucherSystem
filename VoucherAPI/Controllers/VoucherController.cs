using Microsoft.AspNetCore.Mvc;
using VoucherTokenGenerator.Services;
using VoucherTokenGenerator.Models;

namespace VoucherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public VoucherController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // Endpoint to generate a new voucher token
        [HttpPost("generate")]
        public IActionResult GenerateToken([FromQuery] int validMonths = 18, [FromQuery] int value = 0)
        {
            var token = _tokenService.GenerateToken(validMonths, value);
            return Ok(token);
        }

        // Endpoint to get a token by its TokenNumber
        [HttpGet("view/{tokenNumber}")]
        public IActionResult GetToken(string tokenNumber)
        {
            var token = _tokenService.GetTokenByNumber(tokenNumber);

            if (token == null)
            {
                return NotFound(new { message = $"Token with number {tokenNumber} not found" });
            }

            return Ok(token);
        }

        // Endpoint to redeem a token
        [HttpPost("redeem/{tokenNumber}")]
        public IActionResult RedeemToken(string tokenNumber, [FromQuery] int? redeemAmount)
        {
            try
            {
                var response = _tokenService.RedeemToken(tokenNumber, redeemAmount);

                return Ok(response); // Return the RedeemResponse model
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}