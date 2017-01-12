using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApplication1.Handler;

namespace WebApplication1.Controllers
{

    //[Route("api/TEST")]
    public class InnovatusController : ApiController
    {
        OrderHandler orderHandle = new OrderHandler();
        public InnovatusController()
        {
            priceDictionary.Add("onion", 25);
            priceDictionary.Add("potato", 20);
            priceDictionary.Add("tomato", 35);

        }
        Dictionary<string, int> priceDictionary = new Dictionary<string, int>();
        List<OrderReturnRootobject> ordersPending = new List<OrderReturnRootobject>();
        List<OrderReturnRootobject> ordersCompleted = new List<OrderReturnRootobject>();
        /// <summary>
        /// this api would return orders
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public List<OrderReturnRootobject> Get(string orderType)
        {
            return orderHandle.AllOrders(orderType);

        }


        //[ActionName("api/Innovatus/GetAllOrders")]
        // GET: api/Innovatus/5
        //[Route("api/Innovatus/GetAllOrders")]
        // GET: api/Innovatus/5
        //public string Get()
        //{
        //    return "value";
        //}



        public Rootobject Post(OrderRootobject order)
        {

            if (!string.IsNullOrWhiteSpace(order.result.parameters.Address))
            {
                Random r = new Random();
                int randNum = r.Next(1000000);
                string sixDigitNumber = randNum.ToString("D6");


                var returnOrder = new OrderReturnRootobject();
                returnOrder.Address = order.result.resolvedQuery;
                returnOrder.currency = "INR";
                returnOrder.DateTime = order.result.parameters.date[0].ToString("dd-MMM-yyyy hh:mm tt");
                returnOrder.emailId = order.result.parameters.email;
                returnOrder.OrderId = "IN" + sixDigitNumber;
                returnOrder.Price = orderHandle.PriceDetails(order.result.parameters.vegetables);
                returnOrder.Product = order.result.parameters.vegetables;
                returnOrder.Quantity = order.result.parameters.unitweight.amount.ToString();
                returnOrder.Unit = order.result.parameters.unitweight.unit;
                ordersPending = orderHandle.AllOrders("ordersPending");
                ordersPending.Add(returnOrder);
                orderHandle.WriteOrder(ordersPending, "ordersPending");

                return (new Rootobject()
                {
                    speech = "Thanks " + order.result.parameters.email + ", " + order.result.parameters.unitweight.amount + order.result.parameters.unitweight.unit + " " + order.result.parameters.vegetables
                    + " will be sent to you on " + order.result.parameters.date[0].ToString("dd-MMM-yyyy HH:mm tt")
                    + " to your address at " + order.result.resolvedQuery + ". Order ID: " + returnOrder.OrderId + ". Stay Healthy :)"
                    ,
                    displayText = "Hello From API"
                });
            }
            else if (order.result.resolvedQuery.Contains("current temperature"))
            {
                HttpClient client = new HttpClient();
                var rsult = client.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=Delhi,india&appid=eac3f002d569916e04d2ed17c3f457d0").Result;
                var contents = rsult.Content.ReadAsStringAsync().Result;
                var res = new JavaScriptSerializer().Deserialize<WeatherObjectRoot>(contents);

                string a = "{facebook: {HELLLLLLLLOOOOOO}}";
                return (new Rootobject() { speech = "Temprature in Delhi in °C: " + (res.main.temp - 273), data = a, contextOut = null, displayText = "Hello From API" });
            }
            else
            {
                //return (new Rootobject() { contextOut = null, displayText = "Hello From API" });
                return null;
            }


        }


        // PUT: api/Innovatus/5
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// This api is to delete the orders.
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete(string orderid)
        {
            var response = new HttpResponseMessage();
            ordersPending = orderHandle.AllOrders("ordersPending");
            ordersCompleted = orderHandle.AllOrders("ordersCompleted");
            if (!string.IsNullOrEmpty(orderid) && ordersPending.Where(x => x.OrderId.ToUpper().Trim() == orderid.ToUpper().Trim()).Any())
            {
                var orderCompleted = ordersPending.Where(x => x.OrderId.ToUpper().Trim() == orderid.ToUpper().Trim()).FirstOrDefault();
                ordersPending.Remove(orderCompleted);
                ordersCompleted.Add(orderCompleted);
                orderHandle.WriteOrder(ordersPending, "ordersPending");
                orderHandle.WriteOrder(ordersCompleted, "ordersCompleted");
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Add("Message", "Succsessfuly Deleted!!!");
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Add("Message", "Nothing to Delete!!!");
            }
            return response;
        }
    }

}
