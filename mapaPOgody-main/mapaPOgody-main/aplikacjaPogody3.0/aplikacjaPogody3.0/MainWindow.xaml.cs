using Microsoft.Maps.MapControl.WPF;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.IO;
using OpenWeatherMap.Cache;
using static OpenWeatherMap.Cache.Enums;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using OpenWeatherMap.Cache.Models;
using System.Drawing;

namespace aplikacjaPogody3._0
{
    public partial class MainWindow : Window
    {

        string cords;

        public MainWindow()
        {
            InitializeComponent();
            myMap.Focus();
            //Set map to Aerial mode with labels
            myMap.Mode = new AerialMode(true);
        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates

            System.Windows.Point mousePosition = e.GetPosition(this);
            //lol
            var location = myMap.ViewportPointToLocation(mousePosition);
            //Convert the mouse coordinates to a locatoin on the map
            Microsoft.Maps.MapControl.WPF.Location pinLocation = myMap.ViewportPointToLocation(mousePosition);


            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            cords = pinLocation.ToString();
            Trace.WriteLine(pinLocation);

            // Adds the pushpin to the map. i usuwa poprzednie piny
            myMap.Children.Clear();
            myMap.Children.Add(pin);
            var queryLocation = "&lat=" + pin.Location.Latitude + "&lon=" + pin.Location.Longitude;
            Query(queryLocation);
            /*
            try
            {
                string[] czesci = cords.Split(',');

                string czesc1 = czesci[0] + "," + czesci[1];
                string czesc2 = czesci[2] + "," + czesci[3];
                Trace.WriteLine(czesc2 + " " + czesc1);

                double kordynator1 = Convert.ToDouble(czesc1);
                double kordynator2 = Convert.ToDouble(czesc2);


                //bc18bb44ffc23c93706f5655fa470332
                string part=  "minutely,daily" ;
                string call = $"https://api.openweathermap.org/data/3.0/onecall?lat={kordynator1}&lon={kordynator2}&appid=bc18bb44ffc23c93706f5655fa470332&exclude={part}";
                HttpWebRequest zapytanie = (HttpWebRequest)WebRequest.Create(call);
                HttpWebResponse wynik = (HttpWebResponse)zapytanie.GetResponse();
                Stream resStream = wynik.GetResponseStream();
                StreamReader aaa = new StreamReader(resStream);
                string zaza = aaa.ReadToEnd();
                JObject bbb = JObject.Parse(zaza);

                foreach(var okej in bbb)
                {
                    Trace.WriteLine(okej.Key+" "+okej.Value);
                }
            }
            catch
            {
            }*/
        }

        private void Query(string text)
        {
            HttpClient client = new HttpClient();

            string uriString = "https://api.openweathermap.org/data/2.5/forecast?appid=bc18bb44ffc23c93706f5655fa470332&cnt=3&units=metric" + text;

            Uri uri = new Uri(uriString);

            var response = client.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                //error
            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                OWApiResponse forecast = JsonConvert.DeserializeObject<OWApiResponse>(responseContent);

                weather.Text = "";

                for (int i = 0; i < 3; i++)
                {
                    weather.Text += $"{forecast.list[i].dt_txt}\n" +
                        $"  temperature: {forecast.list[i].main.temp}°C" +
                        $"  sensed temperature {forecast.list[i].main.feels_like}°C\n" +
                        $"  pressure: {forecast.list[i].main.pressure}hPa\n" +
                        $"  weather: {forecast.list[i].weather[0].description}\n" +
                        $"  humidity: {forecast.list[i].main.humidity}%" +
                        $"  propability of rain: {forecast.list[i].pop}%\n" +
                        $"  cloudiness: {forecast.list[i].clouds.all}%\n" +
                        $"\n";
                }
            }
        }

        public void textQuery(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();

            string uriString = "http://api.openweathermap.org/geo/1.0/direct?limit=1&appid=bc18bb44ffc23c93706f5655fa470332&q=" + textInput.Text.Replace(' ', '+');

            Uri uri = new Uri(uriString);

            var response = client.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                //error
            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                GeoApiResponse geoApiResponse = JsonConvert.DeserializeObject<GeoApiResponse[]>(responseContent)[0];

                Query("&lat=" + geoApiResponse.lat + "&lon=" + geoApiResponse.lon);
            }
        }


    }
    public class GeoApiResponse
    {
        public string name;
        public object local_names;
        public double lon;
        public double lat;
        public string country;
        public string? state;
    }
    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }

    }
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }

    }
    public class Clouds
    {
        public int all { get; set; }

    }
    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
        public double gust { get; set; }

    }
    public class Rain
    {
        public double h { get; set; }

    }
    public class Sys
    {
        public string pod { get; set; }

    }
    public class List
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public IList<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public int visibility { get; set; }
        public double pop { get; set; }
        public Rain rain { get; set; }
        public Sys sys { get; set; }
        public DateTime dt_txt { get; set; }

    }
    public class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }

    }
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }
        public int population { get; set; }
        public int timezone { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }

    }

    public class OWApiResponse
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public IList<List> list { get; set; }
        public City city { get; set; }

    }
}
