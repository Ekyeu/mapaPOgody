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
            Point mousePosition = e.GetPosition(this);
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
            }
        }
    }
}
