using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Interfaces;
using Serilog;

namespace PharmacyService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly Serilog.ILogger _logger;

        public PaymentController(IPaymentService paymentService, Serilog.ILogger logger)
        {
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // POST: api/payment/process
        [HttpPost("process")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentDto)
        {
            try
            {
                bool success = await _paymentService.ProcessPaymentAsync(paymentDto);
                if (success)
                    return Ok("Payment processed successfully.");
                else
                    return BadRequest("Failed to process payment.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error processing payment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // POST: api/payment/refund
        [HttpPost("refund")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessRefund(int paymentId, decimal refundAmount)
        {
            try
            {
                var result = await _paymentService.RefundPaymentAsync(paymentId);
                if (result)
                    return Ok("Payment refunded successfully.");
                else
                    return NotFound("Payment not found or already refunded.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error refunding payment with ID {PaymentId}.", paymentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
