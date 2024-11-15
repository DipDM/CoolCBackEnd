using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PayPalHttpClient _paypalClient;

    public PaymentController(PayPalHttpClient paypalClient)
    {
        _paypalClient = paypalClient;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder(decimal amount)
    {
        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(new OrderRequest
        {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = "USD",
                        Value = amount.ToString("F2")
                    }
                }
            },
            ApplicationContext = new ApplicationContext
            {
                ReturnUrl = "https://your-frontend-url/success",
                CancelUrl = "https://your-frontend-url/cancel"
            }
        });

        var response = await _paypalClient.Execute(request);
        var result = response.Result<Order>();
        return Ok(new { OrderId = result.Id, Links = result.Links });
    }

    [HttpPost("capture-payment")]
    public async Task<IActionResult> CapturePayment(string orderId)
    {
        // Set your PayPal client ID, client secret, and the URLs
        string clientId = "ATB6UPwVMV-PwVmJaNLcDTRnIT8tLGjLOTufGPii5DkEC8THtSUihq8ctyudR5KkPpZ3-h_EmSJrsKFs";
        string clientSecret = "ATB6UPwVMV-PwVmJaNLcDTRnIT8tLGjLOTufGPii5DkEC8THtSUihq8ctyudR5KkPpZ3-h_EmSJrsKFs";
        string baseUrl = "https://api.sandbox.paypal.com"; // Use the sandbox URL for testing
        string webUrl = "https://www.sandbox.paypal.com"; // Web URL for sandbox

        // Initialize PayPal environment with all required parameters
        var environment = new PayPalEnvironment(clientId, clientSecret, baseUrl, webUrl);

        // Initialize the PayPal HTTP client with the environment
        var client = new PayPalHttpClient(environment);

        // Create capture request for the given orderId
        var request = new OrdersCaptureRequest(orderId);

        // Execute the capture request
        var response = await client.Execute(request);

        var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

        // Check if the payment was captured successfully
        if (result.Status == "COMPLETED")
        {
            // Mark the order as paid in your system, and respond to the client
            return Ok(new { message = "Payment captured successfully" });
        }

        return BadRequest(new { message = "Payment capture failed" });
    }

}
