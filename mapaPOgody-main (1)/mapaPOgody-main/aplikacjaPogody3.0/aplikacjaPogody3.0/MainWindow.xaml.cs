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
using static System.Net.Mime.MediaTypeNames;

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
            //usuwa bazowe funkcje double click
            e.Handled = true;
            //pobiera pozyvcje myszki z mapy
            System.Windows.Point pozycjaMyszki = e.GetPosition(this);


            //lol
            var location = myMap.ViewportPointToLocation(pozycjaMyszki);


            //pozycja myszki na koordynaty
            Microsoft.Maps.MapControl.WPF.Location koordynaty = myMap.ViewportPointToLocation(pozycjaMyszki);


            //dodanie znacznika do mapy
            Pushpin pin = new Pushpin();
            pin.Location = koordynaty;

            //cords = koordynaty.ToString();
            Trace.WriteLine(koordynaty);

            //dodaje znacznik i usuwa poprzednie znaczniki (maks 1)
            myMap.Children.Clear();
            myMap.Children.Add(pin);

            var zapytanieLokalizacja = "&lat=" + pin.Location.Latitude + "&lon=" + pin.Location.Longitude;
            pytanieOLokalizacje(zapytanieLokalizacja);
        }

        private void pytanieOLokalizacje(string text)
        {
            HttpClient polaczeniezserwerami = new HttpClient();

            //$"https://api.openweathermap.org/data/3.0/onecall?lat={kordynator1}&lon={kordynator2}&appid=bc18bb44ffc23c93706f5655fa470332&exclude={part}"
            Uri uri = new Uri("https://api.openweathermap.org/data/2.5/forecast?appid=bc18bb44ffc23c93706f5655fa470332&cnt=3&units=metric" + text);

            var odpowiedzAPI = polaczeniezserwerami.GetAsync(uri).Result;

            if (odpowiedzAPI.IsSuccessStatusCode)
            {
                OWApiResponse forecast = JsonConvert.DeserializeObject<OWApiResponse>(odpowiedzAPI.Content.ReadAsStringAsync().Result);
                pokazanieAPI.Text = "";

                pokazanieAPI.Text = $"{forecast.list[0].dt_txt}\n" +
                    $"temp -> {forecast.list[0].main.temp}\n" +
                    $"feels like -> {forecast.list[0].main.feels_like}\n" +
                    $"temp min -> {forecast.list[0].main.temp_min}\n" +
                    $"temp max -> {forecast.list[0].main.temp_max}\n" +
                    $"pressure -> {forecast.list[0].main.pressure}\n" +
                    $"wind speed - > {forecast.list[0].wind.speed}\n" +
                    $"cloudiness(%) - > {forecast.list[0].clouds.all}\n" +
                    $"humidity(%) - > {forecast.list[0].main.humidity}\n";

                pokazanieAPI.Text += $"\n\n\n{forecast.list[1].dt_txt}\n" +
                    $"temp -> {forecast.list[1].main.temp}\n" +
                    $"feels like -> {forecast.list[1].main.feels_like}\n" +
                    $"temp min -> {forecast.list[1].main.temp_min}\n" +
                    $"temp max -> {forecast.list[1].main.temp_max}\n" +
                    $"pressure -> {forecast.list[1].main.pressure}\n" +
                    $"wind speed - > {forecast.list[1].wind.speed}\n" +
                    $"cloudiness(%) - > {forecast.list[1].clouds.all}\n" +
                    $"humidity(%) - > {forecast.list[1].main.humidity}\n";

            }
            else
            {
                //blad
            }
        }

        public void przyciskLokacja(object sender, EventArgs e)
        {
            HttpClient polaczeniezserwerami = new HttpClient();

            Uri uri = new Uri("http://api.openweathermap.org/geo/1.0/direct?limit=1&appid=bc18bb44ffc23c93706f5655fa470332&q=" + textInput.Text.Replace(' ', '+'));

            var odpoiwedzApi = polaczeniezserwerami.GetAsync(uri).Result;


            var reposneconett = odpoiwedzApi.Content.ReadAsStringAsync().Result;

            GeoApiResponse xxxxxxx = JsonConvert.DeserializeObject<GeoApiResponse[]>(reposneconett)[0];
            Uri kkkkk = new Uri("https://api.openweathermap.org/data/2.5/forecast?appid=bc18bb44ffc23c93706f5655fa470332&cnt=3&units=metric" + "&lat=" + xxxxxxx.lat + "&lon=" + xxxxxxx.lon);


            var bbbbbba = polaczeniezserwerami.GetAsync(kkkkk).Result;


            if (bbbbbba.IsSuccessStatusCode)
            {
                OWApiResponse forecast = JsonConvert.DeserializeObject<OWApiResponse>(bbbbbba.Content.ReadAsStringAsync().Result);
                pokazanieAPI.Text = "";

                pokazanieAPI.Text = $"{forecast.list[0].dt_txt}\n" +
                    $"temp -> {forecast.list[0].main.temp}\n" +
                    $"feels like -> {forecast.list[0].main.feels_like}\n" +
                    $"temp min -> {forecast.list[0].main.temp_min}\n" +
                    $"temp max -> {forecast.list[0].main.temp_max}\n" +
                    $"pressure -> {forecast.list[0].main.pressure}\n" +
                    $"wind speed - > {forecast.list[0].wind.speed}\n" +
                    $"cloudiness(%) - > {forecast.list[0].clouds.all}\n" +
                    $"humidity(%) - > {forecast.list[0].main.humidity}\n";

            }
            else
            {
                //blad
            }
        }
    }









}


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


/*for (int i = 0; i < 3; i++)
{
    pokazanieAPI.Text += $"{forecast.list[i].dt_txt}\n" +
        $"  temperature: {forecast.list[i].main.temp}°C" +
        $"  sensed temperature {forecast.list[i].main.feels_like}°C\n" +
        $"  pressure: {forecast.list[i].main.pressure}hPa\n" +
        $"  weather: {forecast.list[i].weather[0].description}\n" +
        $"  humidity: {forecast.list[i].main.humidity}%" +
        $"  propability of rain: {forecast.list[i].pop}%\n" +
        $"  cloudiness: {forecast.list[i].clouds.all}%\n" +
        $"\n";
}*/


/*if (!response.IsSuccessStatusCode)
{
    //error
}
else
{
    var responseContent = response.Content.ReadAsStringAsync().Result;
    GeoApiResponse geoApiResponse = JsonConvert.DeserializeObject<GeoApiResponse[]>(responseContent)[0];

    pytanieOLokalizacje("&lat=" + geoApiResponse.lat + "&lon=" + geoApiResponse.lon);
}*/