using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebApplication1.Controllers
{
    public class InnovatusController : ApiController
    {

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

     
        // GET: api/Innovatus
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Innovatus/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Innovatus
        public Rootobject Post(InnovatusOrder order)
        {
            var json = new JavaScriptSerializer().Serialize(order);
            using (StreamWriter w = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/data.txt")))
            {
                Log(json, w);

            }
            return (new Rootobject() { data = "{\"facebook\": {HELLLLLLLLOOOOOO}}", contextOut=null,displayText="Hello From API" });
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




    public class InnovatusOrder
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
        public string address { get; set; }
        public string quantity { get; set; }
        public string time { get; set; }
        public string[] vegetables { get; set; }
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



}
