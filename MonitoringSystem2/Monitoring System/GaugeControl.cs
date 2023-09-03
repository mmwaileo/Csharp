using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitoring_System
{
    public class GaugeControl : Control
    {
        private int maximumValue;
        private int minimumValue;
        private int currentValue;

        public GaugeControl()
        {
            // Set default values
            maximumValue = 100;
            minimumValue = 0;
            currentValue = 0;

            // Set control styles
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        public int MaximumValue
        {
            get { return maximumValue; }
            set
            {
                if (value < minimumValue)
                    throw new ArgumentException("Maximum value must be greater than or equal to minimum value.");

                maximumValue = value;
                Invalidate();
            }
        }

        public int MinimumValue
        {
            get { return minimumValue; }
            set
            {
                if (value > maximumValue)
                    throw new ArgumentException("Minimum value must be less than or equal to maximum value.");

                minimumValue = value;
                Invalidate();
            }
        }

        public int Value
        {
            get { return currentValue; }
            set
            {
                if (value < minimumValue || value > maximumValue)
                    throw new ArgumentOutOfRangeException("Value must be within the minimum and maximum range.");

                currentValue = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Set the SmoothingMode property to smooth the line.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(BackColor);

            // Calculate the angle range
            float angleRange = 270f;
            // float angleRange = 360f;

            // Calculate the current angle
            float currentAngle = angleRange * (currentValue - minimumValue) / (maximumValue - minimumValue);

            // Calculate the dimensions
            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = Math.Min(Width, Height) / 2 - 10;

            // Draw the gauge arc
            using (Pen pen = new Pen(ForeColor, 2))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                // g.DrawArc(pen, centerX - radius, centerY - radius, radius * 2, radius * 2, 135, angleRange);
                g.DrawArc(pen, centerX - radius, centerY - radius, radius * 2, radius * 2, 135, angleRange);
            }

            // Draw the gauge arc for the danger zone
            float angleRangeDanger = 60f;
            using (Pen pen = new Pen(Color.Red, 6))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                // g.DrawArc(pen, centerX - radius + 8, centerY - radius + 8, radius * 2 - 16, radius * 2 - 16, 330, angleRangeDanger);
                g.DrawArc(pen, centerX - radius + 6, centerY - radius + 6, radius * 2 - 12, radius * 2 - 12, 300, angleRangeDanger);
            }

            // Draw the needle
            Point[] needlePoints = new Point[3];
            needlePoints[0] = new Point(centerX - 5, centerY);
            needlePoints[1] = new Point(centerX, centerY - radius);
            needlePoints[2] = new Point(centerX + 5, centerY);

            // Rotate the graphics object to draw the needle at the correct angle
            g.TranslateTransform(centerX, centerY);
            g.RotateTransform(currentAngle - 135);
            g.TranslateTransform(-centerX, -centerY);

            // Draw the needle
            g.FillPolygon(Brushes.Black, needlePoints);

            // Reset the graphics transformations
            g.ResetTransform();
            // Change the SmoothingMode to none.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        }
    }
}
