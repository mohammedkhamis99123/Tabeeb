using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace Tabeeb.Infrastructure.Services
{
  
        public class PaymobIntentionResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = null!;

            [JsonPropertyName("intention_order_id")]
            public long IntentionOrderId { get; set; }

            [JsonPropertyName("client_secret")]
            public string ClientSecret { get; set; } = null!;
        }
    
}