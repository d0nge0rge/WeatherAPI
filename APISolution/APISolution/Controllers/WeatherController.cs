
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{

    private readonly Observationsss _weatherStations;

    
    [HttpGet("{wmo}")]
    public ActionResult<Observationsss> GetDataByStation(int wmo)
    {
        Observationsss _weatherStations= GetWeatherDataFromApi(wmo).Result;
        var station = _weatherStations.observations.data.Select(s => s.wmo == wmo).ToList();
        if (station == null)
            return NotFound();

        return Ok(_weatherStations);
    }

    //[HttpGet("{wmo}/observations")]
    //public ActionResult<IEnumerable<WeatherObservation>> GetObservations(string wmo)
    //{
    //    var station = _weatherStations.FirstOrDefault(s => s.wmo == wmo);
    //    if (station == null)
    //        return NotFound();

    //    return Ok(station.data);
    //}

    [HttpGet("{wmo}/observations/{property}")]
    public ActionResult<IEnumerable<double>> GetObservation(int wmo, string property)
    {

        Observationsss _weatherStations = GetWeatherDataFromApi(wmo).Result;
        var station = _weatherStations.observations.data.FirstOrDefault(s => s.wmo == wmo);
        if (station == null)
            return NotFound();

        var observations = _weatherStations.observations.data.Select(o => o.GetType().GetProperty(property)?.GetValue(o)).ToList();
        if (observations == null || observations.All(o => o == null))
            return NotFound();

        return Ok(observations);
    }
    private static async Task<Observationsss> GetWeatherDataFromApi(int wmo)
    {
        string apiUrl = "http://www.bom.gov.au/fwo/IDS60901/IDS60901."+ wmo+".json";
       
        Observationsss observations;
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
              observations = JsonConvert.DeserializeObject<Observationsss>(jsonResult);
            }
            else
            {
                throw new Exception($"Failed to fetch data from API. Status code: {response.StatusCode}");
            }
        }

        return observations;
    }

    


    public class Notice
    {
        public string copyright { get; set; }
        public string copyright_url { get; set; }
        public string disclaimer_url { get; set; }
        public string feedback_url { get; set; }
    }

    public class Header
    {
        public string refresh_message { get; set; }
        public string ID { get; set; }
        public string main_ID { get; set; }
        public string name { get; set; }
        public string state_time_zone { get; set; }
        public string time_zone { get; set; }
        public string product_name { get; set; }
        public string state { get; set; }
    }

    public class Data
    {
        //public Nullable<int> sort_order { get; set; }
        public Nullable<int> wmo { get; set; }
        public string name { get; set; }
        public string history_product { get; set; }
        public string local_date_time { get; set; }
        public string local_date_time_full { get; set; }
        public string aifstime_utc { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double apparent_t { get; set; }
        public string cloud { get; set; }
        public Nullable<int> cloud_base_m { get; set; }
        public Nullable<int> cloud_oktas { get; set; }
        public Nullable<int> cloud_type_id { get; set; }
        public string cloud_type { get; set; }
        public double delta_t { get; set; }
        public Nullable<int> gust_kmh { get; set; }
        public Nullable<int> gust_kt { get; set; }
        public double air_temp { get; set; }
        public double dewpt { get; set; }
        public double press { get; set; }
        public double press_qnh { get; set; }
        public double press_msl { get; set; }
        public string press_tend { get; set; }
        public string rain_trace { get; set; }
        public Nullable<int> rel_hum { get; set; }
        public string sea_state { get; set; }
        public string swell_dir_worded { get; set; }
        public object swell_height { get; set; }
        public object swell_period { get; set; }
        public string vis_km { get; set; }
        public string weather { get; set; }
        public string wind_dir { get; set; }
        public Nullable<int> wind_spd_kmh { get; set; }
        public Nullable<int> wind_spd_kt { get; set; }
    }

    public class Observationsss
    {
        public Observations observations { get; set; }

      
    }
    public class Observations
    {
        public List<Notice> notice { get; set; }
        public List<Header> header { get; set; }
        public List<Data> data { get; set; }
    }
}
