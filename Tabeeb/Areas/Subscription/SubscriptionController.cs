using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.Subscription.Commands.CancelSubscriptionCommand;
using Tabeeb.Application.Areas.Subscription.Commands.CreateSubscriptionCommand;
using Tabeeb.Application.Areas.Subscription.Commands.RenewSubscription;
using Tabeeb.Application.Areas.Subscription.Queries.GetInvoice;
using Tabeeb.Application.Areas.Subscription.Queries.GetSubscriptionHistory;
using Tabeeb.Application.Areas.Subscription.Queries.GetSubscriptions;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Subscription
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Paginate([FromQuery] GetSubscriptions query)
        {
            return await sender.Send(query).ToGenericReponse();
        }

        [HttpGet("{ClinicId}")]

        public async Task<IActionResult> GetHistory([FromRoute] GetSubscriptionHistory query)
        {
            return await sender.Send(query).ToGenericReponse();
        }
        [HttpPost("Create")]

        public async Task<IActionResult> Create([FromBody] CreateSubscriptionCommand query)
        {
            return await sender.Send(query).ToGenericReponse();
        }

        [HttpPost("Renew")]
        public async Task<IActionResult> Renew(RenewSubscriptionCommand query)
        {
            return await sender.Send(query).ToGenericReponse();
        }

        [HttpGet("GetInvoice/{Id}")]
        public async Task<IActionResult> GetInvoice([FromRoute]GetInvoiceQuery query)
        {
            return await sender.Send(query).ToGenericReponse();

        }
        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel([FromBody]CancelSubscriptionCommand query)
        {
            return await sender.Send(query).ToGenericReponse();
        }
    }
}
