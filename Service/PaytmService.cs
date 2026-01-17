using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoolCBackEnd.Service
{
    public class PaytmService
    {
        private readonly HttpClient _httpClient;
        private readonly string _paytmBaseUrl = "https://securegw.paytm.in/";

        public PaytmService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaytmResponse> InitiatePayment(Guid orderId, decimal amount)
        {
            var requestData = new
            {
                MID = "YOUR_MERCHANT_ID",
                ORDER_ID = orderId.ToString(),
                CUST_ID = "CUSTOMER_ID", // Example: User ID
                TXN_AMOUNT = amount.ToString("F2"),
                CHANNEL_ID = "WEB", // Channel ID for web or mobile
                INDUSTRY_TYPE_ID = "Retail",
                WEBSITE = "WEBSTAGING", // Change to actual environment
                CALLBACK_URL = "https://yourwebsite.com/payment/callback"
            };

            var response = await _httpClient.PostAsJsonAsync($"{_paytmBaseUrl}theia/api/v1/initiateTransaction", requestData);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            // Parse response to extract payment URL
            var paymentUrl = ExtractPaymentUrl(responseContent); // Implement this method based on actual response
            return new PaytmResponse { PaymentUrl = paymentUrl };
        }

        public async Task<string> GeneratePaymentRequest(Guid orderId, decimal amount)
        {
            var requestData = new
            {
                ORDER_ID = orderId.ToString(),
                TXN_AMOUNT = amount.ToString("F2"),
                // Include other fields required by Paytm
            };

            var response = await _httpClient.PostAsJsonAsync($"{_paytmBaseUrl}theia/api/v1/transaction/status", requestData);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent; // Handle response as per Paytm's API
        }

        public async Task<bool> VerifyPayment(string orderId, string paymentId)
        {
            var requestData = new
            {
                MID = "YOUR_MERCHANT_ID",
                ORDER_ID = orderId,
                TXNID = paymentId,
                CHECKSUMHASH = "GENERATED_CHECKSUM_HASH" // Include checksum if required
            };

            var response = await _httpClient.PostAsJsonAsync($"{_paytmBaseUrl}merchant-status/api/v1/verifyTransaction", requestData);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent.Contains("success"); // Adjust based on actual response
        }

        private string ExtractPaymentUrl(string responseContent)
        {
            // Implement extraction logic based on response format
            // This might involve parsing JSON or XML
            return "https://example.com/payment-url"; // Example placeholder
        }

        public class PaytmResponse
        {
            public string PaymentUrl { get; set; }
        }
    }
}
