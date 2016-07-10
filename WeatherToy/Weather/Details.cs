using System.Collections.Generic;

namespace WeatherToy.Weather
{
    public class Details
    {
        public DayDetail temp { get; set; }
        public int humidity { get; set; }
        public string pressure { get; set; }
        public List<Weather> weather { get; set; }
    }
}