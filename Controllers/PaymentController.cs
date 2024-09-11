using System;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Payment;
using CoolCBackEnd.Models;
using CoolCBackEnd.Service;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaytmService _paytmService;
        private readonly ApplicationDBContext _context;

        public PaymentController(ApplicationDBContext context, PaytmService paytmService)
        {
            _context = context;
            _paytmService = paytmService;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentRequestDto paymentRequest)
        {
            // Fetch the order from the database
            var order = await _context.Orders.FindAsync(paymentRequest.OrderId);
            if (order == null)
                return NotFound("Order not found");

            // Call Paytm API to initiate payment
            var paytmResponse = await _paytmService.InitiatePayment(order.OrderId, order.TotalAmount);

            // Check if Paytm returned a valid response
            if (paytmResponse == null || string.IsNullOrEmpty(paytmResponse.PaymentUrl))
                return StatusCode(500, "Payment initiation failed");

            // Return the redirect URL for Paytm's payment page
            return Ok(new { redirectUrl = paytmResponse.PaymentUrl });
        }

        [HttpPost("payment-callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] PaytmCallbackDto paytmCallback)
        {
            // Validate the payment callback
            if (paytmCallback == null || string.IsNullOrEmpty(paytmCallback.TransactionId))
                return BadRequest("Invalid payment callback data");

            // Save payment details to the database
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                OrderId = paytmCallback.OrderId,
                PaymentMethod = "Paytm",
                TransactionId = paytmCallback.TransactionId,
                PaymentStatus = paytmCallback.Status,
                AmountPaid = paytmCallback.Amount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Update order status if payment is successful
            var order = await _context.Orders.FindAsync(paytmCallback.OrderId);
            if (order != null && paytmCallback.Status == "Success")
            {
                order.OrderStatus = "Paid";
                await _context.SaveChangesAsync();
            }

            return Ok(new { success = true });
        }
    }
}
