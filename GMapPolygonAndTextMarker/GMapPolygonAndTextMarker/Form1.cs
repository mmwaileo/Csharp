using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMapPolygonAndTextMarker
{
    public partial class Form1 : Form
    {
        private GMapOverlay overlay;
        public Form1()
        {
            InitializeComponent();

            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton = MouseButtons.Left;
            gmap.GrayScaleMode = true;
            gmap.MarkersEnabled = true;
            gmap.MaxZoom = 18;
            gmap.MinZoom = 2;
            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;

            gmap.NegativeMode = false;
            gmap.PolygonsEnabled = true;
            gmap.RoutesEnabled = true;
            gmap.ShowTileGridLines = false;
            gmap.Zoom = 16;

            // Initialize the map
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            //gmap.SetPositionByKeywords("New York, USA");
            gmap.Position = new PointLatLng(40.7110, -74.0060);
            //gmap.Zoom = 16;

            // Replace these points with your desired four-point polygons
            List<PointLatLng> polygon1Points = new List<PointLatLng>
            {
                new PointLatLng(40.7120, -74.0060),
                new PointLatLng(40.7120, -74.0050),
                new PointLatLng(40.7130, -74.0050),
                new PointLatLng(40.7130, -74.0060)
            };

            List<PointLatLng> polygon2Points = new List<PointLatLng>
            {
                new PointLatLng(40.7110, -74.0060),
                new PointLatLng(40.7110, -74.0050),
                new PointLatLng(40.7120, -74.0050),
                new PointLatLng(40.7120, -74.0060)
            };

            // Call this method to draw four-point polygons on the map
            DrawPolygons(polygon1Points, "A1");
            DrawPolygons(polygon2Points, "A2");
        }

        private void DrawPolygons(List<PointLatLng> polygon1Points, string text)
        {
            

            // Draw the first polygon and add text at its center
            DrawPolygonWithCenterText(polygon1Points, text);

            // Draw the second polygon and add text at its center
           // DrawPolygonWithCenterText(polygon2Points, "A2");

            // Refresh the map to update the display
            gmap.Refresh();
        }
        private void DrawPolygonWithCenterText(List<PointLatLng> points, string text)
        {
            // Create the overlay for polygons and markers
            overlay = new GMapOverlay("polygons");
            gmap.Overlays.Add(overlay);

            // Add the polygon to the overlay
            GMapPolygon polygon = new GMapPolygon(points, text);
            overlay.Polygons.Add(polygon);

            // Calculate the center of the polygon
            PointLatLng center = CalculatePolygonCenter(points);

            // Add text marker at the center of the grid
            GMarkerText marker = new GMarkerText(center, text, new Font(FontFamily.GenericSansSerif, 12), Brushes.Red)
            {
                //Offset = new Point(-20, -20)
                Offset = new Point(0, 0)
            };
            overlay.Markers.Add(marker);
        }

        private PointLatLng CalculatePolygonCenter(List<PointLatLng> points)
        {
            double lat = 0;
            double lng = 0;

            foreach (var point in points)
            {
                lat += point.Lat;
                lng += point.Lng;
            }

            int count = points.Count;
            return new PointLatLng(lat / count, lng / count);
        }
    }
}
