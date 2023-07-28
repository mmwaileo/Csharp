using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMapPolygonAndTextMarker
{
    public class GMarkerText : GMapMarker
    {
        public string Text { get; set; }
        public Font Font { get; set; }
        public Brush TextBrush { get; set; }

        public GMarkerText(PointLatLng pos, string text, Font font, Brush textBrush)
            : base(pos)
        {
            Text = text;
            Font = font;
            TextBrush = textBrush;
        }

        public override void OnRender(Graphics g)
        {
            // Get the size of the text
            SizeF textSize = g.MeasureString(Text, Font);

            // Calculate the position for the text
            float x = LocalPosition.X - textSize.Width / 2;
            float y = LocalPosition.Y - textSize.Height / 2;

            // Draw the text on the marker
            g.DrawString(Text, Font, TextBrush, x, y);
        }
    }
}
