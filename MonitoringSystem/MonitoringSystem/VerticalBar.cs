using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitoringSystem
{
    public class VerticalBar : Control
    {
        private int maximumValue;
        private int minimumValue;
        private int currentValue;
        private int targetValue;
        private int tolerance;


        public VerticalBar()
        {
            // Set default values
            maximumValue = 100;
            minimumValue = 0;
            currentValue = 0;
            targetValue = 75;
            tolerance = 10;


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
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(BackColor);

            // Calculate the bar dimensions
            int height = (int)(Height * (float)(currentValue - minimumValue) / (maximumValue - minimumValue));
            Rectangle barRect = new Rectangle(10, Height - height, Width - 10, height);
            Rectangle barRect1 = new Rectangle(10, 0, Width - 11, Height);
            int targetHeight = (int)(Height * (float)(targetValue - minimumValue) / (maximumValue - minimumValue));
            targetHeight = Height - targetHeight; //reverse the value
            //
            using (Pen pen = new Pen(Color.Green, 1))
            {
                g.DrawRectangle(pen, barRect1);
            }

            //Draw target Line
            using (Pen pen = new Pen(Color.Red, 1) { DashPattern = new float[] { 5, 1.5f } })
            {
                g.DrawLine(pen, 10, targetHeight, Width, targetHeight);
            }

            //Draw tolerance Line
            using (Pen pen = new Pen(Color.Blue, 1) { DashPattern = new float[] { 2, 1.5f } })
            {
                g.DrawLine(pen, 10, targetHeight + tolerance, Width, targetHeight + tolerance);
                g.DrawLine(pen, 10, targetHeight - tolerance, Width, targetHeight - tolerance);
            }
            // Draw the scale
            using (Pen pen = new Pen(ForeColor))
            {
                int numTicks = 10;
                int tickSpacing = Height / numTicks;

                for (int i = 0; i <= numTicks; i++)
                {
                    int y = Height - i * tickSpacing;
                    if (i % 5 == 0)
                    {
                        g.DrawLine(pen, 0, y, 10, y);
                    }
                    else
                    {
                        g.DrawLine(pen, 5, y, 10, y);
                    }


                }
            }

            // Draw the bar
            using (Brush brush = new SolidBrush(Color.Green))
            {
                g.FillRectangle(brush, barRect);
            }
        }
    }
}
