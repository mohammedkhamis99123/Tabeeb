using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Tabeeb.Application.Common;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Common;

namespace Tabeeb.Infrastructure.Services
{
    public class PaymentGateway : IPaymentGateway
    {
        private readonly HttpClient _httpClient;
        private readonly PaymobOptions _options;

        public PaymentGateway(
            HttpClient httpClient,
            IOptions<PaymobOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<PaymentInitiationResult> CreatePaymentAsync(
     PaymentInitiationRequest request,
     CancellationToken cancellationToken)
        {
            // 1. Validate amount
            if (request.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            if (decimal.Round(request.Amount, 2) != request.Amount)
                throw new ArgumentException("Payment amount cannot exceed two decimal places.");

            // 2. Convert EGP to cents
            var amountCents = (long)(request.Amount * 100);

            // 3. Split customer name
            var nameParts = request.CustomerName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var firstName = nameParts.FirstOrDefault() ?? "Customer";
            var lastName = nameParts.Length > 1
                ? string.Join(" ", nameParts.Skip(1))
                : "Customer";

            // 4. Prepare Paymob request
            var payload = new
            {
                amount = amountCents,

                currency = request.Currency,

                payment_methods = new[]
                {
            _options.IntegrationId
        },

                items = new[]
                {
            new
            {
                name = request.ItemName,
                amount = amountCents,
                description = request.ItemName,
                quantity = 1
            }
        },

                billing_data = new
                {
                    apartment = "NA",
                    first_name = firstName,
                    last_name = lastName,
                    street = "NA",
                    building = "NA",
                    phone_number = request.CustomerPhone,
                    city = "Cairo",
                    country = "EG",
                    email = request.CustomerEmail,
                    floor = "NA",
                    state = "Cairo"
                },

                special_reference = request.Reference.ToString(),

                notification_url = _options.NotificationUrl,

                redirection_url = _options.RedirectUrl
            };

            // 5. Create HTTP request
            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_options.BaseUrl.TrimEnd('/')}/v1/intention/");

            httpRequest.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Token",
                    _options.SecretKey);

            httpRequest.Content = JsonContent.Create(payload);

            // 6. Send request to Paymob
            using var response = await _httpClient.SendAsync(
                httpRequest,
                cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync(
                cancellationToken);

            // 7. Handle errors
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Paymob payment initiation failed. " +
                    $"Status: {(int)response.StatusCode}. " +
                    $"Response: {responseContent}");
            }

            // 8. Deserialize Paymob response
            var paymobResponse = JsonSerializer.Deserialize<PaymobIntentionResponse>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (paymobResponse is null ||
                string.IsNullOrWhiteSpace(paymobResponse.Id) ||
                string.IsNullOrWhiteSpace(paymobResponse.ClientSecret))
            {
                throw new InvalidOperationException(
                    "Invalid response received from Paymob.");
            }

            // 9. Build checkout URL
            var checkoutUrl =
                $"https://eg.checkout.paymob.com/" +
                $"?publicKey={Uri.EscapeDataString(_options.PublicKey)}" +
                $"&clientSecret={Uri.EscapeDataString(paymobResponse.ClientSecret)}";

            // 10. Return result to Application
            return new PaymentInitiationResult
            {
                IntentionId = paymobResponse.Id,
                OrderId = paymobResponse.IntentionOrderId.ToString(),
                ClientSecret = paymobResponse.ClientSecret,
                CheckoutUrl = checkoutUrl
            };
        }
    }
}
