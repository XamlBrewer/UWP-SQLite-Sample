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
        MainPageViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();

            viewModel = ((MainPageViewModel)this.DataContext);
            viewModel.PropertyChanged += MainPage_PropertyChanged;
        }

        /// <summary>
        /// TODO: Fix binding in CoverFlow. Make SelectedItem a dependency property.
        /// </summary>
        private void MainPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "SelectedPerson" && viewModel.SelectedPerson != null)
            //{
            //    this.CoverFlow.SelectedItem = viewModel.SelectedPerson;
            //    //this.CoverFlow.SelectedIndex = viewModel.Persons.Count - 1;
            //}
        }

        /// <summary>
        /// TODO: Fix binding in CoverFlow. Make SelectedItem a dependency property.
        /// </summary>
        private void CoverFlow_SelectedItemChanged(Controls.CoverFlowEventArgs e)
        {
            //viewModel.SelectedPerson = this.CoverFlow.SelectedItem as PersonViewModel;
        }
    }
}
