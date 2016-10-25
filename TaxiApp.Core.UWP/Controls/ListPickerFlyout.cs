using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace TaxiApp.Core.UWP.Controls
{
    public class ListPickerFlyout : Flyout
    {
        public object ItemsSource
        {
            get
            {
                return view.ItemsSource;
            }
            set
            {
                if (value is Windows.UI.Xaml.Data.Binding)
                {
                    var binding = (Windows.UI.Xaml.Data.Binding)value;
                    BindingOperations.SetBinding(view, ListView.ItemsSourceProperty, binding);
                }
                //view.ItemsSource = value;
            }
        }

        public Windows.UI.Xaml.DataTemplate ItemTemplate
        {
            get
            {
                return view.ItemTemplate;
            }

            set
            {
                view.ItemTemplate = value;
            }
        }

        //public bool Selected { get; set; }



        private ListView view = null;

        public ListPickerFlyout()
        {
            this.Content = new Grid();

            Grid grid = this.Content as Grid;
            grid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            grid.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            //grid.Background = new SolidColorBrush(Color.FromArgb(255, 48, 179, 221));

            grid.RowDefinitions.Add(new RowDefinition() { Height = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new Windows.UI.Xaml.GridLength(50, Windows.UI.Xaml.GridUnitType.Pixel) });

            view = new ListView() { VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch, SelectionMode = ListViewSelectionMode.Multiple };
            grid.Children.Add(view);
            Grid.SetRow(view, 0);
            //view.ItemContainerStyle = Application.Current.Resources["taxiListPickerFlyoutItemStyle"] as Style;

            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch };
            grid.Children.Add(panel);
            Grid.SetRow(panel, 1);

            panel.Children.Add(new Button() { Content = "OK" });
            panel.Children.Add(new Button() { Content = "Cancel" });

            this.Placement = Windows.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Full;

            //this.FlyoutPresenterStyle = new Style()
            this.FlyoutPresenterStyle = new Style(typeof(FlyoutPresenter));

            this.FlyoutPresenterStyle.Setters.Add(new Setter(FlyoutPresenter.BorderThicknessProperty, new Thickness(0.0)));
            this.FlyoutPresenterStyle.Setters.Add(new Setter(FlyoutPresenter.MinHeightProperty, ((Frame)Window.Current.Content).ActualHeight-140));
            this.FlyoutPresenterStyle.Setters.Add(new Setter(FlyoutPresenter.MinWidthProperty, ((Frame)Window.Current.Content).ActualWidth-60));
            //this.FlyoutPresenterStyle.Setters.Add.SetValue(FlyoutPresenter.BorderThicknessProperty, new Thickness(0.0));

            this.Opening += (sender, e) =>
            {
                //((Grid)this.Content).Width = ((Frame)Window.Current.Content).ActualWidth;
                //((Grid)this.Content).Height = ((Frame)Window.Current.Content).ActualHeight;
            };

        }


    }


}
