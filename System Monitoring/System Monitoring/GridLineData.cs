using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Monitoring
{
    public class GridLineData
    {
        public string area { get; set; }
        public double target_height { get; set; }
        public double tolerance { get; set; }
        public double x1 { get; set; }
        public double y1 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double x3 { get; set; }
        public double y3 { get; set; }
        public double x4 { get; set; }
        public double y4 { get; set; }
        //public double min_lat { get; set; }
        //public double max_lat { get; set; }
        //public double min_lng { get; set; }
        //public double max_lng { get; set; }
        // public List<PointLatLng> gridPointsList { get; set; }
        public PointLatLng gridPoint1 { get; set; }
        public PointLatLng gridPoint2 { get; set; }
        public PointLatLng gridPoint3 { get; set; }
        public PointLatLng gridPoint4 { get; set; }
    }
}
