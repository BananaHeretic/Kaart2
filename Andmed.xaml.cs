using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Kaart2
{
    public sealed partial class addAndmed : Page
    {
        DatabaseHelperClass database = new DatabaseHelperClass();
        //bool isViewing;

        private Andmed mapNote;

        public addAndmed()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            Geopoint myPoint;
            if ((e.Parameter as Geopoint) != null)
            {
                System.Diagnostics.Debug.WriteLine("This is a geopoint");
                Save.Content = "Add";
                //isViewing = true;
                mapNote = new Andmed();
                myPoint = (e.Parameter as Geopoint);

            }
            else if (e.Parameter == null)
            {
                System.Diagnostics.Debug.WriteLine((e.Parameter as Geopoint) != null);
                //isViewing = false;

                var locator = new Geolocator();
                locator.DesiredAccuracyInMeters = 50;

                var position = await locator.GetGeopositionAsync();
                myPoint = position.Coordinate.Point;
            }

            else
            {
                Save.Content = "Remove";
                //isViewing = true;
                mapNote = (Andmed)e.Parameter;
                titleTextBox.Text = mapNote.Nimi;
                noteTextBox.Text = mapNote.Text;
                var myPosition = new Windows.Devices.Geolocation.BasicGeoposition();
                
                myPosition.Latitude = mapNote.Ver;
                myPosition.Longitude = mapNote.Hor;

                myPoint = new Geopoint(myPosition);

            }
            await MyMap.TrySetViewAsync(myPoint, 16D);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if((string)Save.Content == "Delete") { 
            DatabaseHelperClass.DeleteContact(mapNote.Id);
            Frame.Navigate(typeof(MainPage));
            }
            else
            {
                Andmed newMapNote = new Andmed();
                newMapNote.Nimi = titleTextBox.Text;
                newMapNote.Text = noteTextBox.Text;
                newMapNote.CreationDate = DateTime.Now;
                newMapNote.Ver = MyMap.Center.Position.Latitude;
                newMapNote.Hor = MyMap.Center.Position.Longitude;

                DatabaseHelperClass.Insert(newMapNote);
                MessageDialog msgbox = new MessageDialog("Note added");

                await msgbox.ShowAsync();
                Frame.Navigate(typeof(MainPage));
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}