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

            AddMapIcon(myGeopoint);
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

        private void AddMapIcon(Geopoint point)
        {
            MapControl mapControl = GetMapControl();

            Windows.UI.Xaml.Shapes.Ellipse fence = new Windows.UI.Xaml.Shapes.Ellipse();
            fence.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 120, 90));

            fence.Width = 15;
            fence.Height = 15;

            //MapIcon MapIcon1 = new MapIcon();
            //MapIcon1.Title = "Space Needle";

            //var childObj = new Windows.UI.Xaml.Controls.Image
            //{
            //    Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/point.png"))
            //};

            Windows.UI.Xaml.Controls.Maps.MapControl.SetLocation(fence, point);
            Windows.UI.Xaml.Controls.Maps.MapControl.SetNormalizedAnchorPoint(fence, new Windows.Foundation.Point(0.5, 0.5));

            mapControl.Children.Add(fence);
        }
    }
}
