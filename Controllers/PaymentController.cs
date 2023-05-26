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
        private readonly  string _whSecret;
        private readonly IPaymentService _payment;
        private readonly ILogger<PaymentController> _illoger;
        public PaymentController(
            IPaymentService payment,
         ILogger<PaymentController> illoger,
          IConfiguration config)
        {
            _illoger = illoger;
            _payment = payment;
            _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
        }

        [HttpPost("{basketId}")]
         public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntent(string basketId){
            // Console.WriteLine("\n\n\n\n\n\n\n\n"+_whSecret+"\n\n\n\n\n\n\n\n");
            return await   _payment.CreateOrUpdateIntent(basketId);
         }

         [HttpPost("webhook")]
         public async Task<ActionResult> StripeWebhook(){

            // Console.WriteLine("\n\n\n\n\n\n\n\n"+225544+"\n\n\n\n\n\n\n\n");

            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            // Console.WriteLine("\n\n\n\n\n\n\n\n"+5544+"\n\n\n\n\n\n\n\n");
            // Console.WriteLine("\n\n\n\n\n\n\n\n"+Request.Headers["Stripe-Signature"]+"\n\n\n\n\n\n\n\n");
            // Console.WriteLine("\n\n\n\n\n\n\n\n"+json+"\n\n\n\n\n\n\n\n");
            var stripeEvent = EventUtility.ConstructEvent(
                    json,Request.Headers["Stripe-Signature"],_whSecret);

                    // Console.WriteLine("\n\n\n\n\n\n\n\n"+stripeEvent.Type+"\n\n\n\n\n\n\n\n");

            PaymentIntent intent;
            Order order;

            switch(stripeEvent.Type){

                case "payment_intent.succeeded":
                //  Console.WriteLine("\n\n\n\n\n\n\n\n"+223344+"\n\n\n\n\n\n\n\n");
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    // Console.WriteLine("\n\n\n\n\n\n\n\n"+intent.Id+"\n\n\n\n\n\n\n\n");
                    _illoger.LogInformation("Payment Succeeded: ",intent.Id);
                    order = await  _payment.UpdateOrderPaymentSucceeded(intent.Id);
                     Console.WriteLine("\n\n\n\n\n\n\n\n"+order+"\n\n\n\n\n\n\n\n");
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