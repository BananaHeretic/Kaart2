using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.CSharp;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using Windows.Storage;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.Services.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Kaart2
{



    public sealed partial class MainPage : Page
    {
        Geopoint mapTappedPoint;
        public static readonly string DB_PATH = System.IO.Path.Combine(System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "sample.sqlite"));
        public MainPage()
        {
            this.InitializeComponent();
            DatabaseHelperClass dbhelp = new DatabaseHelperClass();
            dbhelp.OnCreate("sample2.sqlite");

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        private void L_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "L")
            {
                //    //geopoint = this.maperino.Center;
                //    this.maperino.Center = geopoint;
                //    this.maperino.ZoomLevel = 14;
                //    Andmed andmed = new Andmed("Anstla", geopoint.Position.Latitude, geopoint.Position.Longitude, "Madis");
                Frame.Navigate(typeof(addAndmed));
            }
            if (((Button)sender).Name == "VK")
            {
                Frame.Navigate(typeof(Page3));
            }
            if (((Button)sender).Name == "P")
            {
                DatabaseHelperClass.DeleteAllContact();
            }
        }
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }

        }
        
        bool isDone = true;
        async private void maperino_Tapped(MapControl sender, MapInputEventArgs e)
        {
            string COUNTRY = "Asukoht pole saadaval";
            if (isDone == true)
            {
                isDone = false;
            mapTappedPoint = e.Location;
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(e.Location);
            var pin = new Grid()
            {
                Width = 25,
                Height = 25,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            pin.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.DodgerBlue),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 3,
                Width = 25,
                Height = 25
            });
            maperino.Children.Clear();

            MapControl.SetLocation(pin, new Geopoint(e.Location.Position));
            maperino.Children.Add(pin);
                string msgBox1;
            if (result.Locations.Count != 0 && result.Locations[0].Address.Country != "")
            {
                COUNTRY = result.Locations[0].Address.Country;
                if (result.Locations[0].Address.Town != "")
                {
                    COUNTRY += ", " + result.Locations[0].Address.Town;
                }
                    msgBox1 = "Do you want to add a note at " + COUNTRY + "?";
                }
            else
                {
                    msgBox1 = "Asukoht pole saadaval";
                }
                MessageDialog msgbox = new MessageDialog(msgBox1);
            msgbox.Commands.Add(new UICommand("Add", new UICommandInvokedHandler(this.CommandInvokedHandler)));
            msgbox.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.CommandInvokedHandler)));
            await msgbox.ShowAsync();
                isDone = true;
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Megalo != null)
            {
                if (Megalo.Value > 0 && Megalo.Value < 20)
                    System.Diagnostics.Debug.WriteLine(Megalo.Value);
                maperino.ZoomLevel = Megalo.Value;
            }

        }
        private void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Add")
            {
                Frame.Navigate(typeof(addAndmed), mapTappedPoint);
            }
        }

        private void maperino_ZoomLevelChanged(MapControl sender, object args)
        {
            Megalo.Value = maperino.ZoomLevel;
        }


    }
}