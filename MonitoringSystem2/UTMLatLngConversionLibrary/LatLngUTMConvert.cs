using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTMLatLngConversionLibrary
{
    public class LatLngUTMConvert
    {
        public class LatLng
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class UTMResult
        {
            public double Easting { get; set; }
            public double UTMEasting { get; set; }
            public double Northing { get; set; }
            public double UTMNorthing { get; set; }
            public int ZoneNumber { get; set; }
            public String ZoneLetter { get; set; }
            public String Zona
            {
                get
                {
                    return ZoneNumber + ZoneLetter;
                }
            }

            public override string ToString()
            {
                return " " + ZoneNumber + ZoneLetter + " " + Easting + " " + Northing;
            }
        }

        private double a;
        private double eccSquared;
        private bool status;
        private string datumName = "WGS 84";

        public LatLngUTMConvert(String datumNameIn)//LatLngUTMConvert constructor
        {
            if (!String.IsNullOrEmpty(datumNameIn))
            {
                datumName = datumNameIn;
            }

            this.setEllipsoid(datumName);
        }

        private double toRadians(double grad)
        {
            return grad * Math.PI / 180;
        }

        private String getUtmLetterDesignator(double latitude)
        {
            if ((84 >= latitude) && (latitude >= 72))
                return "X";
            else if ((72 > latitude) && (latitude >= 64))
                return "W";
            else if ((64 > latitude) && (latitude >= 56))
                return "V";
            else if ((56 > latitude) && (latitude >= 48))
                return "U";
            else if ((48 > latitude) && (latitude >= 40))
                return "T";
            else if ((40 > latitude) && (latitude >= 32))
                return "S";
            else if ((32 > latitude) && (latitude >= 24))
                return "R";
            else if ((24 > latitude) && (latitude >= 16))
                return "Q";
            else if ((16 > latitude) && (latitude >= 8))
                return "P";
            else if ((8 > latitude) && (latitude >= 0))
                return "N";
            else if ((0 > latitude) && (latitude >= -8))
                return "M";
            else if ((-8 > latitude) && (latitude >= -16))
                return "L";
            else if ((-16 > latitude) && (latitude >= -24))
                return "K";
            else if ((-24 > latitude) && (latitude >= -32))
                return "J";
            else if ((-32 > latitude) && (latitude >= -40))
                return "H";
            else if ((-40 > latitude) && (latitude >= -48))
                return "G";
            else if ((-48 > latitude) && (latitude >= -56))
                return "F";
            else if ((-56 > latitude) && (latitude >= -64))
                return "E";
            else if ((-64 > latitude) && (latitude >= -72))
                return "D";
            else if ((-72 > latitude) && (latitude >= -80))
                return "C";
            else
                return "Z";
        }

        public UTMResult convertLatLngToUtm(double latitude, double longitude)
        {
            if (status)
            {
                throw new Exception("No ecclipsoid data associated with unknown datum: " + datumName);
            }

            int ZoneNumber;
            var LongTemp = longitude;
            var LatRad = toRadians(latitude);
            var LongRad = toRadians(LongTemp);

            if (LongTemp >= 8 && LongTemp <= 13 && latitude > 54.5 && latitude < 58)
            {
                ZoneNumber = 32;
            }
            else if (latitude >= 56.0 && latitude < 64.0 && LongTemp >= 3.0 && LongTemp < 12.0)
            {
                ZoneNumber = 32;
            }
            else
            {
                ZoneNumber = (int)((LongTemp + 180) / 6) + 1;

                if (latitude >= 72.0 && latitude < 84.0)
                {
                    if (LongTemp >= 0.0 && LongTemp < 9.0)
                    {
                        ZoneNumber = 31;
                    }
                    else if (LongTemp >= 9.0 && LongTemp < 21.0)
                    {
                        ZoneNumber = 33;
                    }
                    else if (LongTemp >= 21.0 && LongTemp < 33.0)
                    {
                        ZoneNumber = 35;
                    }
                    else if (LongTemp >= 33.0 && LongTemp < 42.0)
                    {
                        ZoneNumber = 37;
                    }
                }
            }
            //conversion start
            //var semi_maj_axis = 6378137;
            //var semi_Min_axis = 6356752.31424518;
            //var e1 = Math.Sqrt(Math.Pow(semi_maj_axis, 2) - Math.Pow(semi_Min_axis, 2)) / semi_maj_axis;
            //var e2 = Math.Sqrt(Math.Pow(semi_maj_axis, 2) - Math.Pow(semi_Min_axis, 2)) / semi_Min_axis;
            var eccSquare = 0.006739497;//Math.Pow(e2, 2);
            var c = 6399593.626; //+(Math.Pow(semi_maj_axis, 2)) / semi_Min_axis;

            //var UTMZoneNumber = Truncate((lng/6)+31);
            var UTMBandLetter = getUtmLetterDesignator(latitude);

            var zoneMedidian = 6 * ZoneNumber - 183;
            var LongRadian = longitude * Math.PI / 180;
            var delta_lambda = +LongRadian - (zoneMedidian * Math.PI / 180);
            var latRadian = latitude * Math.PI / 180;

            var A = Math.Cos(latRadian) * Math.Sin(delta_lambda);
            var xi = (0.5) * Math.Log((1 + A) / (1 - A));

            var eta = Math.Atan(Math.Tan(latRadian) / Math.Cos(delta_lambda)) - latRadian;
            var ni = c / Math.Sqrt(1 + eccSquare * Math.Pow(Math.Cos(latRadian), 2)) * 0.9996;
            var zeta = (eccSquare / 2) * Math.Pow(xi, 2) * Math.Pow(Math.Cos(latRadian), 2);

            var A1 = Math.Sin(2 * latRadian);
            var A2 = +A1 * Math.Pow(Math.Cos(latRadian), 2);
            var J2 = latRadian + (A1 / 2);
            var J4 = ((3 * J2) + A2) / 4;

            var J6 = (5 * J4 + A2 * Math.Pow(Math.Cos(latRadian), 2)) / 3;
            var alpha = (0.75) * eccSquare;
            var beta = (1.666666667) * Math.Pow(alpha, 2);
            var gamma = (1.296296296) * Math.Pow(alpha, 3);
            var B = 0.9996 * c * (latRadian - (alpha * J2) + (beta * J4) - (gamma * J6));

            var UTMEasting = xi * ni * (1 + zeta / 3) + 500000;

            var UTMNorthing = eta * ni * (1 + zeta) + B;
            //conversion end

            if (latitude < 0)
                UTMNorthing += 10000000.0;

            return new UTMResult { Easting = UTMEasting, Northing = UTMNorthing, ZoneNumber = ZoneNumber, ZoneLetter = UTMBandLetter };
        }

        private void setEllipsoid(String name)
        {
            switch (name)
            {
                case "Airy":
                    a = 6377563;
                    eccSquared = 0.00667054;
                    break;
                case "Australian National":
                    a = 6378160;
                    eccSquared = 0.006694542;
                    break;
                case "Bessel 1841":
                    a = 6377397;
                    eccSquared = 0.006674372;
                    break;
                case "Bessel 1841 Nambia":
                    a = 6377484;
                    eccSquared = 0.006674372;
                    break;
                case "Clarke 1866":
                    a = 6378206;
                    eccSquared = 0.006768658;
                    break;
                case "Clarke 1880":
                    a = 6378249;
                    eccSquared = 0.006803511;
                    break;
                case "Everest":
                    a = 6377276;
                    eccSquared = 0.006637847;
                    break;
                case "Fischer 1960 Mercury":
                    a = 6378166;
                    eccSquared = 0.006693422;
                    break;
                case "Fischer 1968":
                    a = 6378150;
                    eccSquared = 0.006693422;
                    break;
                case "GRS 1967":
                    a = 6378160;
                    eccSquared = 0.006694605;
                    break;
                case "GRS 1980":
                    a = 6378137;
                    eccSquared = 0.00669438;
                    break;
                case "Helmert 1906":
                    a = 6378200;
                    eccSquared = 0.006693422;
                    break;
                case "Hough":
                    a = 6378270;
                    eccSquared = 0.00672267;
                    break;
                case "International":
                    a = 6378388;
                    eccSquared = 0.00672267;
                    break;
                case "Krassovsky":
                    a = 6378245;
                    eccSquared = 0.006693422;
                    break;
                case "Modified Airy":
                    a = 6377340;
                    eccSquared = 0.00667054;
                    break;
                case "Modified Everest":
                    a = 6377304;
                    eccSquared = 0.006637847;
                    break;
                case "Modified Fischer 1960":
                    a = 6378155;
                    eccSquared = 0.006693422;
                    break;
                case "South American 1969":
                    a = 6378160;
                    eccSquared = 0.006694542;
                    break;
                case "WGS 60":
                    a = 6378165;
                    eccSquared = 0.006693422;
                    break;
                case "WGS 66":
                    a = 6378145;
                    eccSquared = 0.006694542;
                    break;
                case "WGS 72":
                    a = 6378135;
                    eccSquared = 0.006694318;
                    break;
                case "ED50":
                    a = 6378388;
                    eccSquared = 0.00672267;
                    break; // International Ellipsoid
                case "WGS 84":
                case "EUREF89": // Max deviation from WGS 84 is 40 cm/km see http://ocq.dk/euref89 (in danish)
                case "ETRS89": // Same as EUREF89 
                    //a = 6378137;
                    //eccSquared = 0.006739497; //0.006739497 //0.00669438;
                    break;
                default:
                    status = true;
                    break;
            }
        }

        public LatLng convertUtmToLatLng(double UTMEasting, double UTMNorthing, int UTMZoneNumber, String UTMZoneLetter)
        {
            //start of conversion
            var ZoneNumber = UTMZoneNumber;
            var y = UTMNorthing;
            //int NorthernHemisphere;

            //var semi_maj_axis = 6378137;
            //var semi_Min_axis = 6356752.31424518;
            //var e1 = Math.Sqrt(Math.Pow(semi_maj_axis, 2) - Math.Pow(semi_Min_axis, 2)) / semi_maj_axis;
            //var e2 = Math.Sqrt(Math.Pow(semi_maj_axis, 2) - Math.Pow(semi_Min_axis, 2)) / semi_Min_axis;
            var eccSquare = 0.006739497;// Math.Pow(e2, 2); //0.006739497;
            var c = 6399593.626;//+(Math.Pow(semi_maj_axis, 2)) / semi_Min_axis; //6399593.626;//

            if ("N" == UTMZoneLetter)
            {
                //NorthernHemisphere = 1;
            }
            else
            {
                //NorthernHemisphere = 0;
                y -= 10000000.0;
            }

            var merit_central = 6 * ZoneNumber - 183;
            var alpha = 0.005054623;// (3 / 4) * eccSquare;
            var fi = y / (6366197.724 * 0.9996);
            var ni = (c / Math.Sqrt(1 + eccSquare * Math.Pow(Math.Cos(fi), 2))) * 0.9996;
            var a = (UTMEasting - 500000) / ni;
            var gamma = 1.67406E-7;// (35 / 27) * Math.Pow(alpha, 3);
            var beta = 0.000042582;// (5 / 3) * Math.Pow(alpha, 2);

            var zeta = ((eccSquare * Math.Pow(a, 2)) / 2) * Math.Pow(Math.Cos(fi), 2);
            var xi = a * (1 - (zeta / 3));


            var A1 = Math.Sin(2 * fi);
            var A2 = A1 * Math.Pow(Math.Cos(fi), 2);

            var J2 = fi + (A1 / 2);
            var J4 = (3 * J2 + A2) / 4;
            var J6 = (5 * J4 + A2 * Math.Pow(Math.Cos(fi), 2)) / 3;
            var B = 0.9996 * c * (fi - (alpha * J2) + (beta * J4) - (gamma * J6));
            var b = (y - B) / ni;
            var eta = b * (1 - zeta) + fi;

            var senhXi = (Math.Exp(xi) - Math.Exp(-xi)) / 2;
            var delta_lambda = Math.Atan(senhXi / Math.Cos(eta));
            var tau = Math.Atan(Math.Cos(delta_lambda) * Math.Tan(eta));
            var latRadian = fi + (1 + eccSquare * Math.Pow(Math.Cos(fi), 2) - (3 / 2) * eccSquare * Math.Sin(fi) * Math.Cos(fi) * (tau - fi)) * (tau - fi);

            var longitude = (delta_lambda / Math.PI) * 180 + merit_central;
            var latitude = (latRadian / Math.PI) * 180;
            //End of conversion
            return new LatLng { Lat = latitude, Lng = longitude };
        }

        private double toDegrees(double rad)
        {
            return rad / Math.PI * 180;
        }
    }
}
