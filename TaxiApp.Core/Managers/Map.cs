using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

using Windows.UI.Xaml.Controls.Maps;

namespace TaxiApp.Core.Managers
{
    class Map : IMap
    {

        private MapControl mapControl = null;

        public Map(MapControl mapControl)
        {
            this.mapControl = mapControl;

            this.mapControl.MapTapped += MapControl_MapTapped;
        }

        private void MapControl_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            //
        }

        public void ShowMyPossition(Geopoint myGeopoint)
        {
            mapControl.Center = myGeopoint;

            mapControl.ZoomLevel = 12;
            mapControl.LandmarksVisible = true;

            AddMapIcon(myGeopoint);
        }

        public void ShowRoute(MapRoute route)
        {

            Windows.UI.Xaml.Controls.Maps.MapRouteView viewOfRoute = new Windows.UI.Xaml.Controls.Maps.MapRouteView(route);
            viewOfRoute.RouteColor = Windows.UI.Colors.Blue;
            //viewOfRoute.OutlineColor = Windows.UI.Colors.Black;

            // Add the new MapRouteView to the Routes collection
            // of the MapControl.
            mapControl.Routes.Add(viewOfRoute);

        }

        private void AddMapIcon(Geopoint point)
        {
            Windows.UI.Xaml.Shapes.Ellipse fence = new Windows.UI.Xaml.Shapes.Ellipse();
            fence.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 120, 90));

            fence.Width = 15;
            fence.Height = 15;

            //MapIcon MapIcon1 = new MapIcon();
            //MapIcon1.Title = "Space Needle";

            var childObj = new Windows.UI.Xaml.Controls.Image
            {
                Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/point.png"))
            };

            Windows.UI.Xaml.Controls.Maps.MapControl.SetLocation(fence, point);
            Windows.UI.Xaml.Controls.Maps.MapControl.SetNormalizedAnchorPoint(fence, new Windows.Foundation.Point(0.5, 0.5));

            mapControl.Children.Add(fence);
        }
    }
}
