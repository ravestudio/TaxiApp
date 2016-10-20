using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.UWP.Controls
{
    public class ListPickerFlyout : Flyout
    {
        public object Items
        {
            get
            {
                return view.ItemsSource;
            }
            set
            {
                view.ItemsSource = value;
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


        private ListView view = null;

        public ListPickerFlyout()
        {
            this.Content = new Grid();

            Grid grid = this.Content as Grid;
            grid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            grid.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            grid.Background = new SolidColorBrush(Color.FromArgb(255, 48, 179, 221));

            grid.RowDefinitions.Add(new RowDefinition() { Height = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new Windows.UI.Xaml.GridLength(50, Windows.UI.Xaml.GridUnitType.Pixel) });

            view = new ListView() { VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch };
            grid.Children.Add(view);
            Grid.SetRow(view, 0);

            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch };
            grid.Children.Add(panel);
            Grid.SetRow(panel, 1);

            panel.Children.Add(new Button() { Content = "OK" });
            panel.Children.Add(new Button() { Content = "Cancel" });

            this.Placement = Windows.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Full;

        }

        public object load()
        {
            return view.ItemTemplate.LoadContent();
        }
    }
}
