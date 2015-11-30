using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Kaart2
{
    public sealed partial class Page3 : Page
    {
        DatabaseHelperClass database = new DatabaseHelperClass();
        Settings settings = new Settings();
        public Page3()
        {
            this.InitializeComponent();

            var item = DatabaseHelperClass.ReadContacts();
            for (int i = 0; i < item.Count(); i++)
            {
                Andmed obj = new Andmed();
                obj.Id = item.ElementAt(i).Id;
                obj.Hor = item.ElementAt(i).Hor;
                obj.Ver = item.ElementAt(i).Ver;
                obj.Nimi = item.ElementAt(i).Nimi;
                obj.Text = item.ElementAt(i).Text;
                obj.CreationDate = item.ElementAt(i).CreationDate;
                list.Items.Add(obj);
            }
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

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

        private void Selectionchanged_Eventhandler_of_Listbox(object sender, SelectionChangedEventArgs e)
        {
            //Get the data object that represents the current selected item
            Frame.Navigate(typeof(addAndmed), (sender as ListView).SelectedValue);
            //var myobject = (sender as ListView).SelectedValue as Andmed;
            System.Diagnostics.Debug.WriteLine("net");

        }
    }
}