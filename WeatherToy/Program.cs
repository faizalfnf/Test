using System;
using System.Net;
using WeatherToy.Weather;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Configuration;


namespace WeatherToy
{
    class Program
    {

        static void Main(string[] args)
        {
            Dictionary<string, string> stations = PopulateStations();

            Console.WriteLine("The weather informations are as below");
            string indicator = "1";
            GetWeatherInfo(stations, indicator);
        }

        /// <summary>
        /// This prints the weather information for the passed stations
        /// </summary>
        /// <param name="stations"></param>
        protected static void GetWeatherInfo(Dictionary<string, string> stations,string indicator)
        {
            string Apikey = ConfigurationManager.AppSettings.Get("Apikey");//This the API key google API provides
            string url;
            
            using (WebClient client = new WebClient())
            {
                foreach (string station in stations.Keys)
                {
                    url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&units=metric&cnt=1&APPID={1}", station, Apikey);
                    string json = client.DownloadString(url);
                    WeatherInfo weatherInfo = (new JavaScriptSerializer()).Deserialize<WeatherInfo>(json);
                    string result = PopulateWeatherInfo(weatherInfo, stations[station]);
                    Console.WriteLine(result);
                }
                
                Console.WriteLine();
                if (indicator != "2") //Not to run continuosly
                {
                    Console.WriteLine("Enter the below codes to Continue");
                    Console.WriteLine("1- To Continue Fetch for the stations");
                    Console.WriteLine("2- Run Continously.");
                    Console.WriteLine("3- Exit");
                    indicator = Console.ReadLine();
                    if (indicator == "1" || indicator == "2") { GetWeatherInfo(stations, indicator); } //recursive call to repeat the station call
                }
                else
                {
                    GetWeatherInfo(stations, indicator); //recursive call to repeat the station call
                }
            }
        }

        /// <summary>
        /// Populate the weather details in the provided format
        /// </summary>
        /// <param name="WeatherData"></param>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        protected static string PopulateWeatherInfo(WeatherInfo WeatherData, string stationCode)
        {

            string result = string.Concat(stationCode, "|",Math.Round(WeatherData.city.coord.lat,2), ",",
                Math.Round(WeatherData.city.coord.lon,2), "|",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                "|", WeatherData.list[0].weather[0].main, "|", WeatherData.list[0].temp.day, ",",
                WeatherData.list[0].pressure, ",", WeatherData.list[0].humidity);
            return result;
        }

        /// <summary>
        /// This will populate the stations. This is hardcoded now.
        /// This can actually be retrived from a Lookup table.
        /// </summary>
        /// <returns></returns>
        protected static Dictionary<string, string> PopulateStations()
        {
            Dictionary<string, string> Stations = new Dictionary<string, string>();
            Stations.Add("Baltimore", "BWI");
            Stations.Add("Los Angeles", "LAX");
            Stations.Add("Paris", "PAR");
            Stations.Add("Chennai", "MAA");
            Stations.Add("Berlin", "BER");
            Stations.Add("Moscow", "MOW");
            Stations.Add("Sydney", "SYD");
            Stations.Add("Cape Town", "CPT");
            Stations.Add("Riyadh", "RUH");
            Stations.Add("Wellington", "WLG");
            Stations.Add("Cochin", "COK");
            return Stations;
        }
    }
}
