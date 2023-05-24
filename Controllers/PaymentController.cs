using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core;
using core.Entities.Interfaces;
using core.Entities.Oders;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private const string whSecrete = "whsec_767de115f2622dc7546b2a8e5a7e3c055ad30311d321420d873617d817ec1981";
        private readonly IPaymentService _payment;
        private readonly ILogger<PaymentController> _illoger;
        public PaymentController(IPaymentService payment, ILogger<PaymentController> illoger)
        {
            _illoger = illoger;
            _payment = payment;
        }

        [HttpPost("{basketId}")]
         public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntent(string basketId){
            return await   _payment.CreateOrUpdateIntent(basketId);
         }

         [HttpPost("webhook")]
         public async Task<ActionResult> StripeWebhook(){

            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(
                    json,Request.Headers["Stripe-Signature"],whSecrete);

            PaymentIntent intent;
            Order order;

            switch(stripeEvent.Type){
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _illoger.LogInformation("Payment Succeeded: ",intent.Id);
                    order = await  _payment.UpdateOrderPaymentSucceeded(intent.Id);
                     _illoger.LogInformation("Payment updated to payment received: ",order.id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _illoger.LogInformation("Payment failed: ",intent.Id);
                    order = await  _payment.UpdateOrderPaymentFailed(intent.Id);
                     _illoger.LogInformation("Payment updated to payment failed: ",order.id);
                    break;
            }

            return new EmptyResult();
         }
    }
}