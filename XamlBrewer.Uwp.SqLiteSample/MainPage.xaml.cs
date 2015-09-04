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
using XamlBrewer.Uwp.SqLiteSample.ViewModels;

namespace XamlBrewer.Uwp.SqLiteSample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// TODO: Fix binding in CoverFlow. Make SelectedItem a dependency property?
        /// </summary>
        private void CoverFlow_SelectedItemChanged(Controls.CoverFlowEventArgs e)
        {
            ((MainPageViewModel)this.DataContext).SelectedPerson = this.CoverFlow.SelectedItem as PersonViewModel;
        }
    }
}
