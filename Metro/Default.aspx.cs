using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Metro
{
    public class Station
    {
        public List<Train> Trains { get; set; }
    }
    public class Train
    {
        public string Car { get; set; }
        public string Destination { get; set; }
        public string DestinationCode { get; set; }
        public string DestinationName { get; set; }
        public string Group { get; set; }
        public string Line { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Min { get; set; }

    }
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create("http://api.wmata.com/StationPrediction.svc/json/GetPrediction/C09?api_key=h786ssdrh57gd9g8eadw8et6");
            //HttpWebRequest _request = (HttpWebRequest)WebRequest.Create("http://api.wmata.com/StationPrediction.svc/GetPrediction/C09?api_key=h786ssdrh57gd9g8eadw8et6");
            _request.Method = WebRequestMethods.Http.Get;
            _request.Accept = "application/json";

            using (WebResponse _response = _request.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(_response.GetResponseStream()))
                {
                    string _responseText = streamReader.ReadToEnd();

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    Station _stationInfo = new Station();
                    _stationInfo = jsonSerializer.Deserialize<Station>(_responseText);

                    List<Train> _trains = _stationInfo.Trains
                        .Where(train => train.Line == "YL").ToList<Train>();

                    Label1.Text = "";
                    foreach (Train _train in _trains)
                    {
                        Label1.Text += String.Format("Train to {0} arriving in {1} minute(s)<br />", _train.DestinationName, _train.Min);
                        Response.Write(String.Format("Train to {0} arriving in {1} minute(s){2}", _train.DestinationName, _train.Min, Environment.NewLine));
                    }
                    //JToken _jToken = JObject.Parse(_responseText);
                    //JArray _trains = (JArray)_jToken["Trains"];

                    //List<Station> _station = _jToken
                    //    .Select(train => new Train
                    //    {
                    //        Car = Convert.ToInt32(train["Car"]),
                    //        Destination = (string)train["Destination"],
                    //        DestinationCode = (string)train["DestinationCode"],
                    //        DestinationName = (string)train["DestinationName"],
                    //        Group = Convert.ToInt32(train["Group"]),
                    //        Line = (string)train["Line"],
                    //        LocationCode = (string)train["LocationCode"],
                    //        LocationName = (string)train["LocationName"],
                    //        Min = (string)train["Min"]
                    //    }).ToList<Station>();

                    string _stop1 = "here";

                    //XDocument xdoc = XDocument.Parse(_responseText);

                    //List<Station> _yellowLineAll = xdoc.Descendants("Trains")
                    //    .Select(train => new Station
                    //    {
                    //        Car = Convert.ToInt32(train.Element("Car").Value),
                    //        Destination = (string)train.Element("Destination").Value,
                    //        DestinationCode = (string)train.Element("DestinationCode").Value,
                    //        DestinationName = (string)train.Element("DestinationName").Value,
                    //        Group = Convert.ToInt32(train.Element("Group").Value),
                    //        Line = (string)train.Element("Line").Value,
                    //        LocationCode = (string)train.Element("LocationCode"),
                    //        LocationName = (string)train.Element("LocationName"),
                    //        Min = (string)train.Element("Min")
                    //    }).ToList<Station>();

                    //List<Station> _yellowLine = xdoc.Descendants("AIMPredictionTrainInfo")
                    //    .Where(train => (string)train.Element("DestinationName") == "Fort Totten")
                    //    .Select(train => new Station
                    //    {
                    //        Car = Convert.ToInt32(train.Element("Car").Value),
                    //        Destination = (string)train.Element("Destination").Value,
                    //        DestinationCode = (string)train.Element("DestinationCode").Value,
                    //        DestinationName = (string)train.Element("DestinationName").Value,
                    //        Group = Convert.ToInt32(train.Element("Group").Value),
                    //        Line = (string)train.Element("Line").Value,
                    //        LocationCode = (string)train.Element("LocationCode"),
                    //        LocationName = (string)train.Element("LocationName"),
                    //        Min = (string)train.Element("Min")
                    //    }).ToList<Station>();
                    string _stop = "here";
                }

            }

        }

        private static string getXMLFromUri(string uri)
        {
            Uri url = new Uri(uri);
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception a)
            {
                Console.WriteLine(a.ToString());
                throw a;
            }
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

    }
}
