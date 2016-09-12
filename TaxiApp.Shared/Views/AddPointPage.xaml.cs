using TaxiApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TaxiApp.Core.DataModel.Order;
using TaxiApp.Core.ViewModel;

namespace TaxiApp.Views
{

    public sealed partial class AddPointPage : Page
    {

        public AddPointPage()
        {
            this.InitializeComponent();

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OrderPoint point = (OrderPoint)e.Parameter;

            ((MapViewModel)this.DataContext).SetOrderPoint(point);

        }


    }
}
