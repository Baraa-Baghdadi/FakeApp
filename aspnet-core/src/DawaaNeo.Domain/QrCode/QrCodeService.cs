using Microsoft.Extensions.Configuration;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Hosting;

namespace DawaaNeo.QrCode
{
    public class QrCodeService : ITransientDependency
    {
        private readonly  ICurrentUser _currentUser;
        private readonly Lazy<IConfiguration> _configuration;
        private readonly IWebHostEnvironment _Environment;

        public QrCodeService(ICurrentUser currentUser, Lazy<IConfiguration> configuration, IWebHostEnvironment Environment)
        {
            _currentUser = currentUser;
            _configuration = configuration;
            _Environment = Environment;
        }

        public async Task<string> GenerateQrCode(Guid providerId)
        {
            string Dawaa24LogoPath = Path.Combine(_Environment.WebRootPath, DawaaNeoConsts.EmailingFolderName, _configuration.Value["EmailTemplateSetting:Logo"]);
            var path = Path.Combine(_Environment.WebRootPath, DawaaNeoConsts.FilesFolderName);
            // this image we need to add it in middle barcode
            byte[] image = System.IO.File.ReadAllBytes(Dawaa24LogoPath);
            QRCodeGenerator qRGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRGenerator.CreateQrCode(providerId + "", QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrcode = new BitmapByteQRCode(qRCodeData);
            byte[] graphic = qrcode.GetGraphic(40);
            Bitmap bitmap;
            using (var ms = new MemoryStream(graphic))
            {
                bitmap = new Bitmap(ms);
                Bitmap overlay = new Bitmap(new MemoryStream(image));
                int deltaHeight = bitmap.Height - overlay.Height;
                int deltaWidth = bitmap.Width - overlay.Width;
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawImage(overlay, new System.Drawing.Point(deltaWidth / 2, deltaHeight / 2));
            }
            byte[] byteImage;
            using (MemoryStream ms1 = new MemoryStream())
            {
                bitmap.Save(ms1, System.Drawing.Imaging.ImageFormat.Png);
                byteImage = ms1.ToArray();
            }
            using (var ms2 = new MemoryStream(byteImage))
            {
                var image1 = System.Drawing.Image.FromStream(ms2);
                // will save in HttpApi layer
                image1.Save(Path.Combine(path, "qr_code" + providerId + ".png"));
            }
            return Path.Combine("qr_code" + providerId + ".png");
        }
    }
}
