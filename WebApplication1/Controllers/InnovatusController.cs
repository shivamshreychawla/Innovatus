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

namespace WebApplication1.Controllers
{

    public class OrderReturnRootobject
    {
        public string OrderId { get; set; }
        public string emailId { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string DateTime { get; set; }
        public string Address { get; set; }
        public string currency { get; set; }
    }

    public class InnovatusController : ApiController
    {
        // GET: api/Innovatus
        public OrderReturnRootobject Get()
        {
            // Read the file as one string.
            string text = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/data.txt"));
            return new JavaScriptSerializer().Deserialize<OrderReturnRootobject>(text);

        }

        // GET: api/Innovatus/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Innovatus
        public Rootobject Post(OrderRootobject order)
        {

            //using (StreamWriter w = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/data.txt")))



            //if (order.result.resolvedQuery.Contains("Who are"))
            //{
            //    string a = "{facebook: {HELLLLLLLLOOOOOO}}";
            //    return (new Rootobject() { speech = "Shivam and Saurabh Vegetable Shop :)", data = a, contextOut = null, displayText = "Hello From API" });
            //}
            if (!string.IsNullOrWhiteSpace(order.result.parameters.Address))
            {
                Random r = new Random();
                int randNum = r.Next(1000000);
                string sixDigitNumber = randNum.ToString("D6");


                var returnOrder = new OrderReturnRootobject();


                returnOrder.Address = order.result.resolvedQuery;
                returnOrder.currency = "INR";
                returnOrder.DateTime = order.result.parameters.date.ToString("dd-MMM-yyyy HH:MM");
                returnOrder.emailId = order.result.parameters.email;
                returnOrder.OrderId = "IN" + sixDigitNumber;
                returnOrder.Price = "25";
                returnOrder.Product = order.result.parameters.vegetables;
                returnOrder.Quantity = order.result.parameters.unitweight.amount + order.result.parameters.unitweight.unit;

                var json = new JavaScriptSerializer().Serialize(returnOrder);

                using (StreamWriter w = new StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~/data.txt")))
                {
                    w.WriteLine(json);
                }

                return (new Rootobject()
                {
                    speech = "Thanks " + order.result.parameters.email + ", " + order.result.parameters.unitweight.amount + order.result.parameters.unitweight.unit + "  " + order.result.parameters.vegetables
                    + " will be sent to you on  " + order.result.parameters.date.ToString("dd-MMM-yyyy HH:MM")
                    + "to your address at " + order.result.resolvedQuery + ".  Stay Healthy :)"
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
                return (new Rootobject() { contextOut = null, displayText = "Hello From API" });
            }


        }

        static double Celcius(double f)
        {
            double c = 5.0 / 9.0 * (f - 32);

            return c;
        }

        // PUT: api/Innovatus/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Innovatus/5
        public void Delete(int id)
        {
        }
    }








    public class OrderRootobject
    {
        public string id { get; set; }
        public DateTime timestamp { get; set; }
        public Result result { get; set; }
        public Status status { get; set; }
        public string sessionId { get; set; }
    }

    public class Result
    {
        public string source { get; set; }
        public string resolvedQuery { get; set; }
        public string action { get; set; }
        public bool actionIncomplete { get; set; }
        public Parameters parameters { get; set; }
        public object[] contexts { get; set; }
        public Metadata metadata { get; set; }
        public Fulfillment fulfillment { get; set; }
        public int score { get; set; }
    }

    public class Parameters
    {
        public string Address { get; set; }
        public DateTime date { get; set; }
        public string email { get; set; }
        [JsonProperty(PropertyName = "unit-weight")]
        public UnitWeight unitweight { get; set; }
        public string vegetables { get; set; }
    }

    public class UnitWeight
    {
        public int amount { get; set; }
        public string unit { get; set; }
    }

    public class Metadata
    {
        public string intentId { get; set; }
        public string webhookUsed { get; set; }
        public string webhookForSlotFillingUsed { get; set; }
        public string intentName { get; set; }
    }

    public class Fulfillment
    {
        public string speech { get; set; }
        public Message[] messages { get; set; }
    }

    public class Message
    {
        public int type { get; set; }
        public string speech { get; set; }
        public Payload payload { get; set; }
    }

    public class Payload
    {
        public Facebook facebook { get; set; }
        public Kik kik { get; set; }
        public Slack slack { get; set; }
        public Telegram telegram { get; set; }
    }

    public class Facebook
    {
        public Attachment attachment { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Payload1 payload { get; set; }
    }

    public class Payload1
    {
    }

    public class Kik
    {
        public string type { get; set; }
        public string body { get; set; }
    }

    public class Slack
    {
        public string text { get; set; }
        public object[] attachments { get; set; }
    }

    public class Telegram
    {
        public string text { get; set; }
    }

    public class Status
    {
        public int code { get; set; }
        public string errorType { get; set; }
    }


    public class Rootobject
    {
        public string speech { get; set; }
        public string displayText { get; set; }
        public string data { get; set; }
        public string contextOut { get; set; }
        public string source { get; set; }
    }


    public class WeatherObjectRoot
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string _base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public float message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }


}
