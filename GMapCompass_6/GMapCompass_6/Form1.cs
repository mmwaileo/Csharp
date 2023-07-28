using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMapCompass_6
{
    public partial class Form1 : Form
    {
        private GMapOverlay compassOverlay;
        private CompassMarker compassMarker;
        //private Point compassLocation;
     
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton = MouseButtons.Left;
            gmap.GrayScaleMode = true;
            gmap.MarkersEnabled = true;
            gmap.MaxZoom = 18;
            gmap.MinZoom = 2;
            //gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
           
            gmap.NegativeMode = false;
            gmap.PolygonsEnabled = true;
            gmap.RoutesEnabled = true;
            gmap.ShowTileGridLines = false;
            gmap.Zoom = 10;
          
            // Initialize the GMap.NET control
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap.SetPositionByKeywords("New York, USA");

            CreateCompass();
        }
        private void CreateCompass()
        {
            // Initialize the custom overlay
            compassOverlay = new GMapOverlay("compassOverlay");

            // Add the custom overlay to the map control
            gmap.Overlays.Add(compassOverlay);

            // Add a custom marker to the bottom right corner of the screen
            compassMarker = new CompassMarker(gmap.ViewArea.LocationRightBottom);
            PointLatLng position = gmap.ViewArea.LocationRightBottom; //new PointLatLng(gmap.ViewArea.MaxLat, gmap.ViewArea.MaxLng);
            compassMarker.Image = Image.FromFile("img/image.ico");
            compassMarker.Offset = new Point(-compassMarker.Image.Width, -compassMarker.Image.Height);
            compassOverlay.Markers.Add(compassMarker);

            // Subscribe to the map control's OnPositionChanged event
            gmap.OnPositionChanged += gMapControl1_OnPositionChanged;
            gmap.OnMapZoomChanged += gMapControl1_OnMapZoomChanged;
        }

        private void gMapControl1_OnPositionChanged(PointLatLng point)
        {
            // Update the custom marker position to the bottom right corner of the screen
            compassMarker.Position = gmap.ViewArea.LocationRightBottom;//new PointLatLng(gmap.ViewArea.Right, gmap.ViewArea.Bottom); 

            // Update the custom marker rotation based on the current map bearing
            compassMarker.Rotation = -(float)gmap.Bearing;
        }

        private void gMapControl1_OnMapZoomChanged()
        {
            // Update the custom marker position when the map zoom changes

            // Update the custom marker position to the bottom right corner of the screen
            //compassMarker.Position = gmap.ViewArea.LocationRightBottom;//new PointLatLng(gmap.ViewArea.Right, gmap.ViewArea.Bottom); 
            
            //Update the custom marker rotation based on the current map bearing
            //compassMarker.Rotation = (float)gmap.Bearing;
        }
    }

    public class CompassMarker : GMapMarker
    {
        private Image image;
        private float rotation;

        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                Size = new Size(image.Width/10, image.Height/10);
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if (rotation < 0)
                    rotation += 360;
                if (rotation >= 360)
                    rotation -= 360;
            }
        }

        public CompassMarker(PointLatLng p)
            : base(p)
        {
        }

        public override void OnRender(Graphics g)
        {
            if (Image != null)
            {
                g.TranslateTransform((float)LocalPosition.X, (float)LocalPosition.Y);
                g.RotateTransform(Rotation);
                g.DrawImage(Image, Image.Width/2, Image.Height/2, Image.Width/4, Image.Height/4);
                g.ResetTransform();
            }
        }
    }
}
