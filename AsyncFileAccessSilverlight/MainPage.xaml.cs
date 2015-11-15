using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AsyncFileAccessSilverlight.Resources;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace AsyncFileAccessSilverlight
{
    public partial class MainPage : PhoneApplicationPage
    {
        public ObservableCollection<string> Log = new ObservableCollection<string>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.logViewer.ItemsSource = this.Log;
            this.Log.Add("Page loaded!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Log.Add(string.Format("{0} : Start!", DateTime.Now.ToShortTimeString()));

            var tasks = new List<Task>();

            for (int i = 0; i < 5; i++)
            {
                var fileName = string.Format("{0}.txt", i);

                var task = StorageManager.CreateFileStatic(fileName);
                task.ContinueWith((createTask) =>
                {
                    var file = fileName;
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                        this.Log.Add(string.Format("{0} : File {1} created!", DateTime.Now.ToLongTimeString(), file)));
                });
                tasks.Add(task);
            }

            //Task.WaitAll(tasks.ToArray());
            
            this.Log.Add(string.Format("{0} : All files created!", DateTime.Now.ToLongTimeString()));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Log.Add(string.Format("{0} : Start!", DateTime.Now.ToShortTimeString()));

            using (var storageManager = new StorageManager())
            {
                var tasks = new List<Task>();

                for (int i = 0; i < 5; i++)
                {
                    var fileName = string.Format("{0}.txt", i);

                    var task = storageManager.CreateFile(fileName);
                    task.ContinueWith(
                        (createTask) =>
                        {
                            var file = fileName;
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                this.Log.Add(string.Format("{0} : File {1} created!", DateTime.Now.ToLongTimeString(), file)));
                        });
                    tasks.Add(task);
                }

                //Task.WaitAll(tasks.ToArray());
            }

            this.Log.Add(string.Format("{0} : All files created!", DateTime.Now.ToLongTimeString()));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                var fileName = string.Format("{0}.txt", i);
                var task = ReadFile(fileName);
                task.ContinueWith(
                    (createTask) =>
                    {
                        var file = fileName;
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                            this.Log.Add(string.Format("{0} : File {1} read!", DateTime.Now.ToLongTimeString(), file)));
                    });
                tasks.Add(task);
            }

            this.Log.Add(string.Format("{0} : All files created!", DateTime.Now.ToLongTimeString()));
        }

        private async Task ReadFile(string fileName)
        {
            var stream = StorageManager.ReadFileStatic(fileName);
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}