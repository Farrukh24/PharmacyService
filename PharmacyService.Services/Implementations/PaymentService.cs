using AutoMapper;
using Microsoft.Extensions.Logging;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Enums;
using PharmacyService.Contracts.Interfaces;
using PharmacyService.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private List<Payment> _payments = new List<Payment>();

        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> ProcessPaymentAsync(PaymentDTO paymentDto)
        {
            try
            {
                // Simulate payment processing
                _payments.Add(new Payment
                {
                    Id = _payments.Count + 1, // auto-incrementing ID
                    OrderId = paymentDto.OrderId,
                    Amount = paymentDto.Amount,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = paymentDto.PaymentMethod,
                    Status = PaymentStatus.Completed
                });

                _logger.LogInformation("Payment processed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment.");
                throw; 
            }
        }

        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            try
            {
                var payment = _payments.FirstOrDefault(p => p.Id == paymentId);
                if (payment == null)
                {
                    _logger.LogError("Payment with ID {PaymentId} not found.", paymentId);
                    return false;
                }

                payment.IsRefunded = true;

                _logger.LogInformation("Payment refunded successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refunding payment with ID {PaymentId}.", paymentId);
                throw; 
            }
        }
    }
}
