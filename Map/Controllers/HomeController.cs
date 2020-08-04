using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Dadata;
using Dadata.Model;
using Newtonsoft.Json.Linq;

namespace Map.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult Go(string address, string limit)
        {
            var x = Convert.ToInt32(limit) * 0.001;
            string res = Convert.ToString(x).Replace(",",".");
            try
            { 
            var url = "https://nominatim.openstreetmap.org/search?q="+ address + "&format=json&polygon_geojson=1&polygon_threshold="+res;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Referer = "https://nominatim.openstreetmap.org";
            request.Method = "POST";
            request.ContentType = "application/json";

            Stream dataStream = request.GetRequestStream();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            List<string> geo = new List<string>();
            var parse = JToken.Parse(responseFromServer);
                for (int i=0; i< parse.Count(); i++)
                {
                    if (parse[i]["geojson"]["type"].ToString() != "Point")
                    {
                        geo.Add(parse[i]["lat"].ToString() + " " + parse[i]["lon"].ToString()+"!");
                        geo.Add(parse[i]["geojson"]["coordinates"].ToString());
                        
                    }                   
                }
           
         
                return Json(geo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) { return null; }
        }
    }
}