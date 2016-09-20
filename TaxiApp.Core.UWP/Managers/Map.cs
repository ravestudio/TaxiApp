using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.Managers;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace TaxiApp.Core.UWP.Managers
{
    public class Map : TaxiApp.Core.Managers.IMap
    {

        private MapControl _mapControl = null;

        public Map()
        {
        }

        private void MapControl_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            //
        }

        private MapControl GetMapControl()
        {
            if (this._mapControl == null)
            {
                this._mapControl = ChildFinder.FindChild<MapControl>(Window.Current.Content, "RouteMapControl");
                this._mapControl.MapTapped += MapControl_MapTapped;
            }
            return _mapControl;
        }

        public void ShowMyPossition(Geopoint myGeopoint)
        {
            MapControl mapControl = GetMapControl();

            mapControl.Center = myGeopoint;

            mapControl.ZoomLevel = 12;
            mapControl.LandmarksVisible = true;

            Windows.UI.Xaml.Shapes.Ellipse fence = new Windows.UI.Xaml.Shapes.Ellipse();
            fence.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 120, 90));

            fence.Width = 15;
            fence.Height = 15;

            AddMapIcon(myGeopoint, fence);
        }

        public void ShowMarker(Geopoint geopoint)
        {
            AddOrderPoint(geopoint);
        }

        public void ShowRoute(IRoute route)
        {
            MapControl mapControl = GetMapControl();

            MapRoute mapRoute = ((Route)route).MapRoute;

            Windows.UI.Xaml.Controls.Maps.MapRouteView viewOfRoute = new Windows.UI.Xaml.Controls.Maps.MapRouteView(mapRoute);
            viewOfRoute.RouteColor = Windows.UI.Colors.Blue;
            //viewOfRoute.OutlineColor = Windows.UI.Colors.Black;

            // Add the new MapRouteView to the Routes collection
            // of the MapControl.

            mapControl.Routes.Add(viewOfRoute);

        }

        private void AddOrderPoint(Geopoint point)
        {
            var icon = new Windows.UI.Xaml.Controls.Image()
            {
                Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/marker_next.png")),
                Width = 50,
                Height = 50
            };

            AddMapIcon(point, icon);
        }

        private void AddMapIcon(Geopoint point, DependencyObject obj)
        {
            MapControl mapControl = GetMapControl();

            Windows.UI.Xaml.Controls.Maps.MapControl.SetLocation(obj, point);
            Windows.UI.Xaml.Controls.Maps.MapControl.SetNormalizedAnchorPoint(obj, new Windows.Foundation.Point(0.5, 0.5));

            mapControl.Children.Add(obj);
        }

        public void Clear()
        {
            MapControl mapControl = GetMapControl();

            mapControl.Children.Clear();
            mapControl.Routes.Clear();
        }
    }
}
