using PharmacyService.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(PaymentDTO paymentDto);
        Task<bool> RefundPaymentAsync(int paymentId);
    }
}
