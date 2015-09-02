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
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using XamlBrewer.Uwp.SqLiteSample.Models;
using Windows.Storage;
using XamlBrewer.Uwp.SqLiteSample.DataAccessLayer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XamlBrewer.Uwp.SqLiteSample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Dal.CreateDatabase();
        }
    }
}
