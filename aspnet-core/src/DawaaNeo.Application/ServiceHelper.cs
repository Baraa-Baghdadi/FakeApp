using DawaaNeo.Orders;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo
{
  public static class ServiceHelper
  {
    public static decimal _calculateDeliveryCost(Guid addressId, decimal total)
    {
      return 10;
    }

    public static Task UpdateQuantity(List<OrderItemDto> items, string v)
    {
      throw new NotImplementedException();
    }

    public static double _calculateDistance(double lat1,double lat2,double lon1,double lon2)
    {
      double d_lon1 = toRadians(lon1);
      double d_lon2 = toRadians(lon2);
      double d_lat1 = toRadians(lat1);
      double d_lat2 = toRadians(lat2);

      double dlon = d_lon2 - d_lon1; 
      double dlat = d_lat2 - d_lat1;

      double a = Math.Pow(Math.Sin(dlat/2), 2) +
                  Math.Cos(d_lat1) * Math.Cos(d_lat2)*
                  Math.Pow(Math.Sin(dlon/2),2);

      double c = 2 * Math.Asin(Math.Sqrt(a));

      double r = 6371;

      return (c * r);
    }

    private static double toRadians(double angel)
    {
      return (angel * Math.PI) / 180;
    }

    public static decimal getTimeSpam(DateTime dt)
    {
            // Define the epoch (in this case, the .NET epoch: January 1, 0001)
            DateTime epoch = new DateTime(1, 1, 1);

            // Calculate the total days since the epoch
            int daysSinceEpoch = (dt.Date - epoch).Days;

            // Calculate the fractional part of the day
            double fractionalDay = (dt.Hour + (dt.Minute / 60.0) + (dt.Second / 3600.0) + (dt.Millisecond / 3600000.0)) / 24.0;

            // Combine the integer and fractional parts into a decimal
            decimal decimalRepresentation = (decimal)daysSinceEpoch + (decimal)fractionalDay;

            return decimalRepresentation;
        }

    public static string GetThumbNail(string iconBase64)
        {
            int maxWidth = 50 ,  maxHeight = 50;
            var icon = Convert.FromBase64String(iconBase64) ;
            using(MemoryStream ms = new MemoryStream(icon))
            {
                Image image = Image.FromStream(ms);

                // Calculate new distanation for the thumbnail:
                int newWidth,newHeight;
                if (image.Width > image.Height)
                {
                    newWidth = maxHeight;
                    newHeight = (int)((double) image.Height / image.Width * maxWidth);
                }
                else
                {
                    newHeight = maxHeight;
                    newWidth = (int)((double)image.Width / image.Height * maxHeight);
                }

                // Create thumbnail image:
                Image thumnail = new Bitmap(newWidth, newHeight);
                using(Graphics graphic = Graphics.FromImage(thumnail))
                {
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.DrawImage(image, 0, 0,newWidth,newHeight);
                }

                // Convert thumbnail image to base64:

                using(MemoryStream msThumbnail = new MemoryStream())
                {
                    thumnail.Save(msThumbnail, ImageFormat.Jpeg);
                    byte[] thumbnailByte = msThumbnail.ToArray();
                    return Convert.ToBase64String(thumbnailByte);
                }
            }

        }
    }
}
