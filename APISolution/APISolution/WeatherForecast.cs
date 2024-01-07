namespace APISolution
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    //public class WeatherObservation
    //{
    //    public string Time { get; set; }
    //    public double Temperature { get; set; }
    //}

    //public class WeatherStation
    //{
    //    public string WMO { get; set; }
    //    public List<WeatherObservation> Observations { get; set; }
    //}
}