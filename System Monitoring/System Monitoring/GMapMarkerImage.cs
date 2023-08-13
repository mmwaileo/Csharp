using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.WindowsForms;

namespace System_Monitoring
{
    public class GMapMarkerImage : GMapMarker
    {
        private Image markerImage; // The image used for the marker
        private float bearing; // The rotation angle in degrees
        private SizeF size; // The size of the marker

        public GMapMarkerImage(PointLatLng pos, Image image) : base(pos)
        {
            // Initialize the marker image, bearing, and size
            markerImage = image;
            bearing = 0.0f; // Default rotation angle is 0 degrees
            size = new SizeF(markerImage.Width, markerImage.Height); // Default size is the original image size
        }

        public Image MarkerImage
        {
            get { return markerImage; }
            set
            {
                markerImage = value;
                // Update the marker size based on the new image size
                size = new SizeF(markerImage.Width, markerImage.Height);

                // Redraw the marker with the updated image
                if (Overlay != null && Overlay.Control != null)
                    Overlay.Control.Invalidate();
            }
        }

        public float Bearing
        {
            get { return bearing; }
            set
            {
                // Normalize the rotation angle to the range [0, 360) degrees
                bearing = value % 360;
                if (bearing < 0)
                    bearing += 360;

                // Redraw the marker with the updated rotation
                if (Overlay != null && Overlay.Control != null)
                    Overlay.Control.Invalidate();
            }
        }
        /*
        public SizeF Size
        {
            get { return size; }
            set
            {
                size = value;
                // Redraw the marker with the updated size
                if (Overlay != null && Overlay.Control != null)
                    Overlay.Control.Invalidate();
            }
        }
        */
        public override void OnRender(Graphics g)
        {
            if (markerImage != null)
            {
                // Save the current state of the graphics object
                GraphicsState state = g.Save();

                // Apply the rotation and translation to the graphics object
                g.TranslateTransform((float)LocalPosition.X, (float)LocalPosition.Y);
                g.RotateTransform(bearing);
                g.TranslateTransform(-size.Width, -size.Height / 2);

                // Draw the marker's image at the marker's position with the specified size
                //g.DrawImage(markerImage, new RectangleF(0, 0, size.Width, size.Height));
                //  g.DrawImage(markerImage, markerImage.Width, markerImage.Height, markerImage.Width, markerImage.Height);
                g.DrawImage(markerImage, new Point(0, 0));

                // Restore the graphics object to its original state
                g.Restore(state);
            }
        }

        public override void Dispose()
        {
            // Dispose of the marker image and other resources (if any)

            base.Dispose();
        }
    }
}
