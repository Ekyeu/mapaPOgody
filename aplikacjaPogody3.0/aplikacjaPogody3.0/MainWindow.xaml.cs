using Microsoft.Maps.MapControl.WPF;
using OpenWeather;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aplikacjaPogody3._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
                Trace.WriteLine(czesc2+" "+czesc1);

                double kordynator1 = Convert.ToDouble(czesc1);
                double kordynator2 = Convert.ToDouble(czesc2);


                // get the closest station to 29.3389, -98.4717 lat/lon
                if (!OpenWeather.StationDictionary.TryGetClosestStation(kordynator1, kordynator2, out var stationInfo))
                {
                    Console.WriteLine($@"Could not find a station.");
                    return;
                }

                Trace.WriteLine($@"Name: {stationInfo.Name}");
                Trace.WriteLine($@"ICAO: {stationInfo.ICAO}");
                Trace.WriteLine($@"Lat/Lon: {stationInfo.Latitude}, {stationInfo.Longitude}");
                Trace.WriteLine($@"Elevation: {stationInfo.Elevation}m");
                Trace.WriteLine($@"Country: {stationInfo.Country}");
                Trace.WriteLine($@"Region: {stationInfo.Region}");

                // get a MetarStation and autoupdate every 30 minutes
                // you can change this via Settings._UpdateIntervalSeconds = yourValue
                var metarStation = stationInfo.AsMetarStation(true, true);

                // subscribe to updates on the station
                _ = metarStation.Subscribe(x =>
                {
                    Trace.WriteLine("\n\nCurrent METAR Report:");
                    Trace.WriteLine($@"Temperature: {x.Temperature}C");
                    Trace.WriteLine($@"Wind Heading: {x.WindHeading}");
                    Trace.WriteLine($@"Wind Speed: {x.WindSpeed}Kts");
                    Trace.WriteLine($@"Dewpoint: {x.Dewpoint}");
                    Trace.WriteLine($@"Visibility: {x.Visibility}Km");
                    Trace.WriteLine($@"Presure: {x.Pressure}Pa");
                });

                //Console.ReadLine();

            }
            catch
            {
            }
        }
    }
}
